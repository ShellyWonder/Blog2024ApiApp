using Blog2024ApiApp.Models;
using Blog2024ApiApp.Enums;
using X.PagedList;

namespace Blog2024ApiApp.Data.Repositories.Interfaces
{

    public interface ISearchRepository
    {
        Task<IPagedList<Post>> SearchPostsByStateAsync(PostState postState, int pageNumber, int pageSize, string searchTerm);

    }
}
