﻿using Blog2024Api.Models;
using Blog2024Api.Enums;


namespace Blog2024Api.Data.Repositories.Interfaces
{
    public interface IBlogRepository
    {
        Task<IEnumerable<Blog>> GetAllBlogsAsync();
        Task<IEnumerable<Blog>> GetBlogsByStateAsync(PostState postState, int pageNumber, int pageSize);

        Task<Blog?> GetBlogByIdAsync(int id);
        Task CreateBlogAsync(Blog blog, string userId);
        Task UpdateBlogAsync(Blog blog, string userId);
        Task DeleteBlogAsync(int id);
        Task<bool> BlogExistsAsync(int id);

        
    }
}
