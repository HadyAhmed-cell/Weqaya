using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VirtualClinic.Dto;
using VirtualClinic.Identity;
using VirtualClinic.Interfaces;

namespace VirtualClinic.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        //private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService
            /*IMapper mapper*/)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            //_mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if ( CheckEmailExistsAsync(registerDto.Email).Result.Value )
            {
                return BadRequest("Email Already Exists");
            }

            var user = new AppUser
            {
                UserName = registerDto.Email.Split("@")[0],
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if ( registerDto.Email.Split("@")[1] == "admin.com" )
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            else if ( registerDto.Email.Split("@")[1] == "doctor.com" )
            {
                await _userManager.AddToRoleAsync(user, "Doctor");
            }
            else if ( registerDto.Email.Split("@")[1] == "lab.com" )
            {
                await _userManager.AddToRoleAsync(user, "Lab");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Patient");
            }
            if ( !result.Succeeded )
            {
                return BadRequest();
            }
            //await _signInManager.SignInAsync(user, isPersistent: false);

            var userDto = new UserDto()
            {
                Email = registerDto.Email,
                Token = await _tokenService.CreateToken(user, _userManager)
            };
            return Ok(userDto);
        }

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync(string email)
         => await _userManager.FindByEmailAsync(email) != null;

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
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
                Token = await _tokenService.CreateToken(user, _userManager)
            };
            return Ok(userDto);
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                Email = user.Email,
                Token = await _tokenService.CreateToken(user, _userManager)
            });
        }
    }
}