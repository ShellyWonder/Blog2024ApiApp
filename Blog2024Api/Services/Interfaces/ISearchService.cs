using Blog2024ApiApp.Enums;
using Blog2024ApiApp.Models;

namespace Blog2024ApiApp.Services.Interfaces
{
    public interface ISearchService
    {
        Task<(List<Post> Posts, int TotalCount)> SearchPostsAsync(PostState postState, int pageNumber, int pageSize, string searchTerm);
        
    }

}