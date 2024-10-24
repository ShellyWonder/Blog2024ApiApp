using Blog2024ApiApp.DTO;

namespace Blog2024ApiApp.Data.Repositories.Interfaces
{
    public interface IApplicationUserRepository
    {
        Task<IEnumerable<UserDTO?>> GetAllUsersAsync();
        Task<UserDTO?> GetUserByIdAsync(string id);
        Task<IEnumerable<UserDTO?>> GetAllAuthorsAsync();
                                        
        Task<IEnumerable<UserDTO?>> GetAllBlogAuthorsAsync();
        Task<IEnumerable<UserDTO?>> GetAllPostAuthorsAsync();

        Task<IEnumerable<UserDTO?>> GetAllModeratorsAsync();
        Task<UserDTO?> GetModeratorByIdAsync(string id);
        Task<UserDTO?> GetAuthorByIdAsync(string id);
        Task<UserDTO?> GetBlogAuthorByIdAsync(string id);
        Task<UserDTO?> GetPostAuthorByIdAsync(string id);
        Task<IEnumerable<UserDTO?>> GetAllAdministratorsAsync();
        Task<UserDTO?> GetAdministratorByIdAsync(string id);


    }
}
