using Blog2024Api.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog2024Api.Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public required string LastName { get; set; }

        [Display(Name = "User Profile Image")]
        public byte[]? ImageData { get; set; }

        public string? ImageType { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        [Display(Name = "GitHub Url")]
        public string? GitHubUrl { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        [Display(Name = "LinkedIn Url")]
        public string? LinkdInUrl { get; set; }

        [NotMapped]
        public string? FullName
        {

            get
            {
                return $"{FirstName} {LastName}";
            }
            set
            {

            }
        }
        // This will be used to upload the file from the form.
        [NotMapped]
        public IFormFile? ImageFile { get; set; }


        //Navigation Properties
        public virtual ICollection<Blog> Blogs { get; set; } = [];
        public virtual ICollection<Post> Posts { get; set; } = [];

        public virtual ICollection<UserRole>? UserRoles { get; set; } = new List<UserRole>();
    }

}
