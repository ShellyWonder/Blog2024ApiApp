using Blog2024Api.Data;
using Blog2024Api.Enums;
using Blog2024Api.DTO;
using X.PagedList;
using Blog2024Api.Models;

namespace Blog2024Api.Services.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetAllBlogsAsync();
        Task<IPagedList<Blog>> GetBlogsByStateAsync(PostState postState, int pageNumber, int pageSize);
        Task<Blog?> GetBlogByIdAsync(int id);
        Task CreateBlogAsync(Blog blog, string userId);
        Task UpdateBlogAsync(Blog blog, string userId);
        Task DeleteBlogAsync(int id);
        Task<bool> BlogExistsAsync(int id);
        Task<IEnumerable<UserDTO?>> GetAllBlogAuthorsAsync();
        Task<IEnumerable<UserDTO?>> GetAllAuthorsAsync();



    }
}
