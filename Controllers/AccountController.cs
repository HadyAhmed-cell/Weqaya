using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VirtualClinic.Dto;
using VirtualClinic.Identity;

namespace VirtualClinic.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromQuery] RegisterDto registerDto)
        {
            if ( CheckEmailExistsAsync(registerDto.Email).Result.Value )
            {
                return BadRequest("Email Already Exists");
            }

            var user = new AppUser
            {
                UserName = registerDto.Email.Split("@")[0],
                Email = registerDto.Email,
                Fname = registerDto.FirstName,
                Lname = registerDto.LastName,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if ( !result.Succeeded )
            {
                return BadRequest();
            }
            await _signInManager.SignInAsync(user, isPersistent: false);


            var userDto = new UserDto()
            {
                Email = registerDto.Email,
                DisplayName = $"{user.Fname}"
            };
            return Ok(userDto);


        }


        [HttpGet("Email Exists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
         => await _userManager.FindByEmailAsync(email) != null;


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromQuery] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if ( user is null )
            {
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if ( !result.Succeeded )
            {
                return Unauthorized();
            }
            var userDto = new UserDto()
            {
                Email = loginDto.Email,
                DisplayName = $"{user.Fname}"
            };
            return Ok(userDto);
        }

    }
}
