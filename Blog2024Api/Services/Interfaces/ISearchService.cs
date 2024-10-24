using Blog2024ApiApp.Enums;
using Blog2024ApiApp.Models;
using X.PagedList;

namespace Blog2024ApiApp.Services.Interfaces
{
    public interface ISearchService
    {
        Task<IPagedList<Post>> SearchPosts(PostState postState, int pageNumber, int pageSize, string searchTerm);
    }
}