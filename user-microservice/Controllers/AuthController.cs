using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using user_microservice.Data;
using user_microservice.Dtos;
using user_microservice.Interfaces;
using user_microservice.Models;

namespace user_microservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly int _refreshTokenExpirationInDays;

        public AuthController(
            IConfiguration configuration,
            UserManager<AppUser> userManager,
            ITokenService tokenService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _tokenService = tokenService;
            _refreshTokenExpirationInDays = _configuration.GetValue<int>("RefreshToken:ExpirationInDays", 7);
        }

        private CookieOptions GetRefreshTokenCookieOptions()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(_refreshTokenExpirationInDays),
                SameSite = SameSiteMode.Lax, // pozwala na działanie między localhostami
                Secure = false // z https musi być true
            };

            return cookieOptions;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUserReadDto>> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userDto = new AppUserReadDto
            {
                Id = user.Id,
                Username = user.UserName!,
                Email = user.Email
            };

            return Ok(userDto);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<AppUserReadDto>> GetMe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId == null)
            {
                return Unauthorized("Invalid token.");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new AppUserReadDto
            {
                Id = user.Id,
                Username = user.UserName!,
                Email = user.Email
            });
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            var rawToken = dto.Token;

            if (string.IsNullOrEmpty(rawToken))
            {
                return BadRequest("Token is required.");
            }

            ClaimsPrincipal principal = null;

            try
            {
                principal = _tokenService.GetPrincipalFromExpiredToken(rawToken);
            }catch(Exception ex)
            {
                return BadRequest($"Invalid token: {ex.Message}");
            }

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier)
              ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId == null)
                return Unauthorized();
            
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var rToken = Request.Cookies["refreshToken"];

            if(rToken == null || user.RefreshToken != rToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return Unauthorized();
            }

            var newJwtToken = await _tokenService.CreateToken(user);
            string newRefreshToken = _tokenService.CreateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_refreshTokenExpirationInDays);
            await _userManager.UpdateAsync(user);

            Response.Cookies.Append("refreshToken", newRefreshToken, GetRefreshTokenCookieOptions());

            return Ok(new AuthResponseDto
            {
                Id = user.Id,
                Email = user.Email!,
                Token = newJwtToken
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterInputDto model)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return BadRequest("You are already logged in.");
            }

            var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existingUserByEmail != null)
            {
                return BadRequest("Email already exists.");
            }

            var existingUserByUsername = await _userManager.FindByNameAsync(model.Username);
            if (existingUserByUsername != null)
            {
                return BadRequest("Username already exists.");
            }

            var user = new AppUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, UserRoles.User);

            return Ok(new AuthResponseDto
            {
                Id = user.Id,
                Email = user.Email!,
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginInputDto model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest("You are already logged in.");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!isPasswordValid)
            {
                return Unauthorized("Invalid email or password.");
            }

            try
            {
                var token = await _tokenService.CreateToken(user);
                string refreshToken = _tokenService.CreateRefreshToken();
                int refreshTokenExpirationInDays = _configuration.GetValue<int>("RefreshToken:ExpirationInDays", 7);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenExpirationInDays);
                await _userManager.UpdateAsync(user);


                var cookieOptions = GetRefreshTokenCookieOptions();

                Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

                return Ok(new AuthResponseDto
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Token = token
                });
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error generating token: {ex.Message}");
            }
        }
    }
}