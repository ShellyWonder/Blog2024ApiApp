﻿using Blog2024ApiApp.Data.Repositories.Interfaces;
using Blog2024ApiApp.Enums;
using Blog2024ApiApp.Models;
using X.PagedList;
using X.PagedList.EF;

namespace Blog2024ApiApp.Data.Repositories
{

    public class SearchRepository(ApplicationDbContext context) : ISearchRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IPagedList<Post>> SearchPostsByStateAsync(PostState postState, int pageNumber, int pageSize, string searchTerm)
        {
            var posts = _context.Posts.Where(p => p.BlogPostState == postState);

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
                              .ToPagedListAsync(pageNumber, pageSize);
        }

    }
}
