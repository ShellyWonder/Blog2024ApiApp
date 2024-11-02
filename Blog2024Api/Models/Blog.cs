using System.ComponentModel.DataAnnotations;
using Blog2024Api.Services.Interfaces;
using Blog2024Api.UserIdentity;

namespace Blog2024Api.Models
{
    public partial class Blog : IImageEntity
    {
        public int Id { get; set; }
        public string? AuthorId { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        [Display(Name = "Blog Name")]
        public required string Name { get; set; }
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        [Display(Name = "Blog Description")]
        public required string Description { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        public DateTime? Updated { get; set; }

        //Navigation properties
        //Parent Author to Child Blog
        public virtual ApplicationUser? Author { get; set; }

        //Parent Blog to Child Posts
        public virtual ICollection<Post> Posts { get; set; } = [];
    }
}
