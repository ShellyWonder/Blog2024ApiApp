using Blog2024Api.Data.Repositories.Interfaces;

namespace Blog2024Api.Data.Repositories
{
    public class SlugRepository(ApplicationDbContext context) : ISlugRepository
    {
        private readonly ApplicationDbContext _context = context;

        public bool IsSlugUnique(string slug)
        {
            return !_context.Posts.Any(p => p.Slug == slug);
        }
    }
}
