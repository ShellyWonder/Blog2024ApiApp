using Blog2024Api.Data.Repositories.Interfaces;
using Blog2024Api.Services.Interfaces;
using Blog2024Api.DTO;

namespace Blog2024Api.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        #region CONSTRUCTOR
        public ApplicationUserService(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }
        #endregion

        #region GET ALL USERS 
        public async Task<IEnumerable<UserDTO?>> GetAllUsersAsync()
        {
            return await _applicationUserRepository.GetAllUsersAsync();
        }
        #endregion

        #region GET USER BY ID
        public async Task<UserDTO?> GetUserByIdAsync(string id)
        {
            return await _applicationUserRepository.GetUserByIdAsync(id);
        }
        #endregion

        #region GET ALL AUTHORS
        //pulls authors from both POSTS & BLOGS tables
        public async Task<IEnumerable<UserDTO?>> GetAllAuthorsAsync()
        {
            return await _applicationUserRepository.GetAllAuthorsAsync();
        }
        #endregion

        #region GET AUTHOR BY ID
        public async Task<UserDTO?> GetAuthorByIdAsync(string id)
        {
            return await _applicationUserRepository.GetAuthorByIdAsync(id);
        }
        #endregion

        #region GET BLOG AUTHOR BY ID
        public async Task<UserDTO?> GetBlogAuthorByIdAsync(string id)
        {
            return await _applicationUserRepository.GetBlogAuthorByIdAsync(id);
        }

        #endregion

        #region GET POST AUTHOR BY ID 
        public async Task<UserDTO?> GetPostAuthorByIdAsync(string id)
        {
            return await _applicationUserRepository.GetPostAuthorByIdAsync(id);
        }

        #endregion

        #region GET ALL ADMINISTRATORS
        public async Task<IEnumerable<UserDTO?>> GetAllAdministratorsAsync()
        {
            return await _applicationUserRepository.GetAllAdministratorsAsync();
        }
        #endregion

        #region GET ADMINISTRATOR BY ID
        public async Task<UserDTO?> GetAdministratorByIdAsync(string id)
        {
            return await _applicationUserRepository.GetAdministratorByIdAsync(id);
        }
        #endregion

        #region GET ALL BLOG AUTHORS
        //pulls authors from BLOGS tables
        public async Task<IEnumerable<UserDTO?>> GetAllBlogAuthorsAsync()
        {
            return await _applicationUserRepository.GetAllBlogAuthorsAsync();
        }
        #endregion

        #region GET ALL POST AUTHORS
        //pulls authors from both POSTS 
        public async Task<IEnumerable<UserDTO?>> GetAllPostAuthorsAsync()
        {
            return await _applicationUserRepository.GetAllPostAuthorsAsync();
        }
        #endregion

        #region GET ALL MODERATORS
        public async Task<IEnumerable<UserDTO?>> GetAllModeratorsAsync()
        {
            return await _applicationUserRepository.GetAllModeratorsAsync();
        }
        #endregion

        #region GET MODERATOR BY ID
        public async Task<UserDTO?> GetModeratorByIdAsync(string id)
        {
            return await _applicationUserRepository.GetModeratorByIdAsync(id);
        }

        #endregion

    }
}
