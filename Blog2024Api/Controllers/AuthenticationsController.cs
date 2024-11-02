using Blog2024Api.DTO;
using Blog2024Api.UserIdentity;
using Blog2024Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace Blog2024Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly JwtTokenService _jwtTokenService;
        private readonly ImageService _imageService;
        private readonly EmailSender _emailSender;

        #region CONSTRUCTOR
        public AuthenticationsController(UserManager<ApplicationUser> userManager,
                                       IConfiguration configuration,
                                       JwtTokenService jwtTokenService,
                                       ImageService imageService,
                                       EmailSender emailSender)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwtTokenService = jwtTokenService;
            _imageService = imageService;
            _emailSender = emailSender;
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
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                GitHubUrl = registerDto.GitHubUrl,
                LinkdInUrl = registerDto.LinkedInUrl
            };

            // Process Image File
            if (registerDto.ImageFile != null)
            {
                user.ImageData = await _imageService.ConvertFileToByteArrayAsync(registerDto.ImageFile);
                user.ImageType = _imageService.GetFileType(registerDto.ImageFile);
            }
            else
            {
                // Default avatar image
                user.ImageData = await _imageService.ConvertFileToByteArrayAsync("/img/avatar_icon.png");
                user.ImageType = "png";
            }

            // Register the user
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            // Assign a default role (e.g., "Commentator")
            await _userManager.AddToRoleAsync(user, "Commentator");

            // Generate email confirmation token
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Authentications",
                new { userId = user.Id, code },
                protocol: HttpContext.Request.Scheme
            );

            // Send confirmation email
            await _emailSender.SendEmailAsync(registerDto.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            var token = _jwtTokenService.GenerateJwtToken(user);

            return Ok(new { token });
        }
        #endregion

        #region LOGIN
        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _jwtTokenService.AuthenticateUserAsync(loginDto.UserName!, loginDto.Password!);

            if (user == null) return Unauthorized("Invalid username or password");

            var token = _jwtTokenService.GenerateJwtToken(user);
            return Ok(new { token });
        }
        #endregion

        #region CONFIRM EMAIL
        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null) return BadRequest("Invalid email confirmation request.");
            
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user == null) return NotFound("User not found.");
            
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            return result.Succeeded ? Ok("Email confirmed successfully.") : BadRequest("Email confirmation failed.");
        }
        #endregion

        #region USER INFO
        // GET: api/auth/userinfo
        //User Profile
        [HttpGet("userinfo")]
        public async Task<ActionResult<UserDTO>> GetUserInfo()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return NotFound("User not found.");

            var userDto = new UserDTO
            {
                Id = user.Id.ToString(),
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
        #endregion
    }
}
   