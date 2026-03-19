using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using user_microservice.Data;
using user_microservice.Dtos;
using user_microservice.Interfaces;
using user_microservice.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace user_microservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<AppUser> userManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
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
                Username = user.UserName!
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
                Username = user.UserName!
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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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