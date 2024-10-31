using System.ComponentModel.DataAnnotations;

namespace Blog2024Api.DTO
{
    public class RegisterDTO
    {
        public string? UserName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }
        
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public required string LastName { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        [Display(Name = "GitHub Url")]
        public string? GitHubUrl { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        [Display(Name = "LinkedIn Url")]
        public string? LinkedInUrl { get; set; }

        [Display(Name = "User Profile Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
