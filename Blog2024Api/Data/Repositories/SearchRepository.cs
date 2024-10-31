using Blog2024Api.Models;
using Blog2024Api.Data.Repositories.Interfaces;
using Blog2024Api.Enums;
using Microsoft.EntityFrameworkCore;

namespace Blog2024Api.Data.Repositories
{

    public class SearchRepository : ISearchRepository
    {
        private readonly ApplicationDbContext _context;

        public SearchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> SearchPostsByStateAsync(PostState postState, int pageNumber, int pageSize, string searchTerm)
        {
            var posts = _context.Posts.Where(p => p.BlogPostState == PostState.ProductionReady);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                posts = posts.Where(p =>
                       p.Title.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                       p.Abstract!.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                       p.Content!.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                       p.Comments.Any(c =>
                           c.Body!.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                           c.ModeratedBody!.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                           c.Commentator!.FirstName.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                           c.Commentator.LastName.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                           c.Commentator.Email!.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                       ));
            }

            return await posts.OrderByDescending(p => p.Created)
                                                .Skip((pageNumber - 1) * pageSize)
                                                .Take(pageSize)
                                                .ToListAsync();
        }
        public async Task<int> GetTotalCountAsync(PostState postState, string searchTerm)
        {
            return await _context.Posts
                .Where(post => post.BlogPostState == PostState.ProductionReady &&
                                   (string.IsNullOrEmpty(searchTerm)
                                   || post.Title.Contains(searchTerm)
                                   || post.Content!.Contains(searchTerm)))
                .CountAsync();
        }
    }
}
