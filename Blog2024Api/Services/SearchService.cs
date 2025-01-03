﻿using Blog2024Api.Models;
using Blog2024Api.Data.Repositories.Interfaces;
using Blog2024Api.Enums;
using Blog2024Api.Services.Interfaces;

namespace Blog2024Api.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISearchRepository _searchRepository;

        #region CONSTRUCTOR
        public SearchService(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }
        #endregion

        #region SEARCH POSTS
        public async Task<(List<Post> Posts, int TotalCount)> SearchPostsAsync(
            PostState postState, int pageNumber, int pageSize, string searchTerm)
        {
            // Fetch the paged data and the total count from the repository.
            var posts = await _searchRepository.SearchPostsByStateAsync(postState, pageNumber, pageSize, searchTerm);
            var totalCount = await _searchRepository.GetTotalCountAsync(postState, searchTerm);

            return (posts, totalCount);
        }

    } 
    #endregion
}

