using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using user_microservice.Dtos;
using user_microservice.Interfaces;
using user_microservice.Models;
using user_microservice.Services;

namespace user_microservice.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordValidator<AppUser> _passwordValidator;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<AppUser> userManager, 
                                    IPasswordValidator<AppUser> passwordValidator, 
                                    SignInManager<AppUser> signInManager,
                                    ITokenService tokenService)
        {
            _userManager = userManager;
            _passwordValidator = passwordValidator;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<AppUserReadDto>> GetUserById(string id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new AppUserReadDto()
            {
                Id = user.Id,
                Username = user.UserName
            };

            return Ok(userDto);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginInputDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return Unauthorized("Nieprawidłowy email lub hasło.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!isPasswordValid)
            {
                return Unauthorized("Nieprawidłowy email lub hasło.");
            }

            var token = _tokenService.CreateToken(user);

            // 4. Zwracamy paczkę JSON do Vue.js
            return Ok(new AuthResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Token = token
            });
        }
    }



    //[HttpPost]
    //public async Task<IActionResult> Register(RegisterViewModel registerVM)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return View(registerVM);
    //    }

    //    var user = await _userManager.FindByEmailAsync(registerVM.Email);
    //    if (user != null)
    //    {
    //        TempData["Error"] = "Konto o podanym adresie e-mail już istnieje.";
    //        return View(registerVM);
    //    }

    //    var userN = await _userManager.FindByNameAsync(registerVM.Username);
    //    if (userN != null)
    //    {
    //        TempData["Error"] = "Konto o podanej nazwie użytkownika już istnieje";
    //        return View(registerVM);
    //    }

    //    var tempUser = new AppUser();
    //    var result = await _passwordValidator.ValidateAsync(_userManager, tempUser, registerVM.Password);

    //    if (!result.Succeeded)
    //    {
    //        TempData["Error"] = "Hasło musi się składać z co najmniej 6 znaków, zawierać co najmniej jedną dużą literę, co najmniej jedną małą literę, co najmniej jedną cyfrę oraz co najmniej jeden znak specjalny.";
    //        return View(registerVM);
    //    }

    //    var newUser = new AppUser()
    //    {
    //        UserName = registerVM.Username,
    //        Email = registerVM.Email,
    //    };


    //    var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

    //    if (newUserResponse.Succeeded)
    //    {
    //        await _userManager.AddToRoleAsync(newUser, UserRoles.User);
    //    }
    //    else
    //    {
    //        var errors = string.Join(", ", newUserResponse.Errors.Select(e => e.Description));
    //        TempData["Error"] = errors;
    //        return View(registerVM);
    //    }

    //    return RedirectToAction("Index", "Home");
    //}

    //public async Task<IActionResult> Logout()
    //{
    //    await _signInManager.SignOutAsync();
    //    return RedirectToAction("Index", "Home");
    //}
}

