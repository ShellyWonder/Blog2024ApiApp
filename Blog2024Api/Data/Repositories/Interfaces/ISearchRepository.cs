using Blog2024Api.Enums;
using Blog2024Api.Models;

namespace Blog2024Api.Data.Repositories.Interfaces
{

    public interface ISearchRepository
    {
        Task<List<Post>> SearchPostsByStateAsync(PostState postState, int pageNumber, int pageSize, string searchTerm);
        Task<int> GetTotalCountAsync(PostState postState, string searchTerm);

    }
}
