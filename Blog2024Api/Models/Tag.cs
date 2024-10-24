using Blog2024ApiApp.Data;


namespace Blog2024ApiApp.Models
{
        public class Tag
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public string? AuthorId { get; set; }

        public string? Text { get; set; }


        //Navigation properties
        public virtual Post? Post { get; set; }
        public virtual ApplicationUser? Author { get; set; }
    }
}
