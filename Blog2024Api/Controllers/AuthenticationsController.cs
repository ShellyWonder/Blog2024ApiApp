using Blog2024Api.DTO;
using Blog2024Api.Services;
using Blog2024ApiApp.Data;
using Blog2024ApiApp.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Blog2024Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly JwtTokenService _jwtTokenService;

     #region CONSTRUCTOR
        public AuthenticationsController(UserManager<ApplicationUser> userManager,
                                       IConfiguration configuration,
                                       JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwtTokenService = jwtTokenService;
        }
        #endregion

     #region REGISTER
        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName!,
                LastName = registerDto.LastName!
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password!);

            if (!result.Succeeded) return BadRequest(result.Errors);

            // Assign the "Commentator" role
            await _userManager.AddToRoleAsync(user, "Commentator");
            // Generate JWT token
            var token = _jwtTokenService.GenerateJwtToken(user);
            return Ok(new { token });
        } 
        #endregion

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _jwtTokenService.AuthenticateUserAsync(loginDto.UserName!, loginDto.Password!);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            var token = _jwtTokenService.GenerateJwtToken(user);
            return Ok(new { token });
        }


        // GET: api/auth/userinfo
        //User Profile
        [HttpGet("userinfo")]
        public async Task<ActionResult<UserDTO>> GetUserInfo()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userDto = new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                GitHubUrl = user.GitHubUrl,
                LinkedInUrl = user.LinkdInUrl,
                ImageData = user.ImageData,
                ImageType = user.ImageType
            };

            return Ok(userDto);
        }
    }
}
   