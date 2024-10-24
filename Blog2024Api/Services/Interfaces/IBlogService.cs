using Blog2024ApiApp.Data;
using Blog2024ApiApp.Enums;
using Blog2024ApiApp.Models;
using Blog2024ApiApp.DTO;
using X.PagedList;

namespace Blog2024ApiApp.Services.Interfaces
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
