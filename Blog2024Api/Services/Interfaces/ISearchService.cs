using Blog2024Api.Models;
using Blog2024Api.Enums;

namespace Blog2024Api.Services.Interfaces
{
    public interface ISearchService
    {
        Task<(List<Post> Posts, int TotalCount)> SearchPostsAsync(PostState postState, int pageNumber, int pageSize, string searchTerm);
        
    }

}