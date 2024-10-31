using Blog2024Api.Models;
using Blog2024Api.Data.Repositories.Interfaces;
using Blog2024Api.Enums;
using Blog2024Api.Services.Interfaces;

namespace Blog2024Api.Services
{
    #region PRIMARY CONSTRUCTOR
    public class PostService(IPostRepository postRepository) : IPostService
    {
        private readonly IPostRepository _postRepository = postRepository; 
        #endregion

        #region ADD POST(CREATE)
        public async Task AddPostAsync(Post post, string userId)
        {
            await _postRepository.AddPostAsync(post, userId);
        } 
        #endregion

        #region UPDATE POST
        public async Task UpdatePostAsync(Post post, string userId)
        {
            await _postRepository.UpdatePostAsync(post, userId);
        } 
        #endregion

        #region DELETE POST
        public async Task DeletePostAsync(int id)
        {
            await _postRepository.DeletePostAsync(id);
        } 
        #endregion

        #region GET ALL POSTS
        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _postRepository.GetAllPostsAsync();
        }
        public async Task<IEnumerable<Post>> GetAllPostsByStateAsync(PostState postState, int pageNumber, int pageSize, int id)
        {
            return await _postRepository.GetAllPostsByStateAsync(postState, pageNumber, pageSize, id);
        }

        #endregion

        #region GET ALL POSTS BY BLOG ID
        public async Task<IEnumerable<Post>> GetAllPostsByBlogIdAsync(int id)
        {
            return await _postRepository.GetPostsByBlogIdAsync(id);
        } 
        #endregion

        #region GET POST BY ID
        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _postRepository.GetPostByIdAsync(id);
        } 
        #endregion

        #region GET POST BY SLUG
        public async Task<Post> GetPostBySlugAsync(string slug)
        {
            return await _postRepository.GetPostBySlugAsync(slug);
        } 
        #endregion

        #region POST EXISTS
        public async Task<bool> PostExistsAsync(int id)
        {
            return await _postRepository.PostExistsAsync(id);
        } 
        #endregion

    }
}
