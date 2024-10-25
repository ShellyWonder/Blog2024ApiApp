using Blog2024ApiApp.Models;
using Blog2024ApiApp.Enums;

namespace Blog2024ApiApp.Data.Repositories.Interfaces
{

    public interface ISearchRepository
    {
        Task<List<Post>> SearchPostsByStateAsync(PostState postState, int pageNumber, int pageSize, string searchTerm);
        Task<int> GetTotalCountAsync(PostState postState, string searchTerm);

    }
}
