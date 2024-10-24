using Blog2024ApiApp.Data.Repositories.Interfaces;
using Blog2024ApiApp.Enums;
using Blog2024ApiApp.Models;
using Blog2024ApiApp.Services.Interfaces;
using PagedList;
using X.PagedList;

namespace Blog2024ApiApp.Services
{
#region PRIMARY CONSTRUCTOR
    public class SearchService(ISearchRepository searchRepository) : ISearchService
    {
        private readonly ISearchRepository _searchRepository = searchRepository; 
        #endregion

        #region SEARCH POSTS
        public async Task<PagedList.IPagedList<Post>> SearchPosts(PostState postState, int pageNumber, int pageSize, string searchTerm)
        {
            return (PagedList.IPagedList<Post>)await _searchRepository.SearchPostsByStateAsync(postState, pageNumber, pageSize, searchTerm);
        } 
        #endregion
    }
}
