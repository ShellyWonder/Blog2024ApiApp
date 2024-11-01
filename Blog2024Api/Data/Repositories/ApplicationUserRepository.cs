using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Blog2024Api.Data.Repositories.Interfaces;
using Blog2024Api.DTO;
using Blog2024Api.Identity;

namespace Blog2024Api.Data.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

    #region CONSTRUCTOR
        public ApplicationUserRepository(ApplicationDbContext context,
                                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        #endregion

        #region GET ALL USERS
        public async Task<IEnumerable<UserDTO?>> GetAllUsersAsync()
        {

            var users = _context.Users
                                    .Select(u => new UserDTO
                                    {
                                        Id = u.Id.ToString(),              // Access the Id from ApplicationUser
                                        FullName = u.FullName    // Access the FullName from ApplicationUser
                                    });

            return await users.ToListAsync();
        }
        #endregion

        #region GET USER BY ID
        /// <summary>
        /// ApplicationUser inherits id from IdentityUser whose Id is a guid.
        /// Therefore, GetUserByIdAsync has a string id parameter)
        /// </summary>

        public async Task<UserDTO?> GetUserByIdAsync(string id)
        {
            // Await the result and store it in a variable
            var user = await _context.Users
                                     .Where(x => x.Id.ToString() == id)
                                     .Select(x => new UserDTO
                                     {
                                         Id = x.Id.ToString(),
                                         FullName = x.FullName
                                     })
                                     .SingleOrDefaultAsync();

            // Check if the user is null and throw an exception if needed
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} was not found.");
            }

            return user;
        }
        #endregion

        #region GET ALL MODERATORS
        public async Task<IEnumerable<UserDTO?>> GetAllModeratorsAsync()
        {
            var moderators = _context.Comments
                .Where(c => c.Moderator != null)
                .Select(c => new UserDTO
                {
                    Id = c.Moderator!.Id.ToString(),
                    FullName = c.Moderator!.FullName
                })
                .Distinct();

            return await moderators.ToListAsync();
        }
        #endregion

        #region GET MODERATOR BY ID/FULL NAME
        public async Task<UserDTO?> GetModeratorByIdAsync(string id)
        {
            // Attempt to parse the string id to a Guid
            if (!Guid.TryParse(id, out Guid moderatorId))
            {
                throw new ArgumentException("Invalid ID format.", nameof(id));
            }
            var moderator = await _context.Users
                                      //joining Comments 
                                      .Where(u => _context.Comments.Any(c => c.ModeratorId == u.Id.ToString() && u.Id == moderatorId))
                                     .Select(u => new UserDTO
                                     {
                                         Id = u.Id.ToString(),
                                         FullName = u.FullName
                                     })
                                     .SingleOrDefaultAsync();


            // Check if the user is null and throw an exception if needed
            if (moderator == null)
            {
                throw new KeyNotFoundException($"Moderator with ID {id} was not found.");
            }

            return moderator;
        }
        #endregion

        #region GET ALL AUTHORS
        //pulls both BLOG & POST Authors
        public async Task<IEnumerable<UserDTO?>> GetAllAuthorsAsync()
        {
            var blogAuthors = await GetAllBlogAuthorsAsync();

            var postAuthors = await GetAllPostAuthorsAsync();

            return blogAuthors
            .Union(postAuthors)
            .Distinct();
        }
        #endregion

        #region GET ALL BLOG AUTHORS
        public async Task<IEnumerable<UserDTO?>> GetAllBlogAuthorsAsync()
        {
            var blogAuthors = _context.Blogs
                .Where(b => b.Author != null)
                .Select(p => new UserDTO
                {
                    Id = p.Author!.Id.ToString(),
                    FullName = p.Author!.FullName
                })
                .Distinct();

            return await blogAuthors.ToListAsync();

        }
        #endregion

        #region GET ALL POST AUTHORS
        public async Task<IEnumerable<UserDTO?>> GetAllPostAuthorsAsync()
        {
            var postAuthors = _context.Posts
                .Where(p => p.Author != null)
                .Select(p => new UserDTO
                {
                    Id = p.Author!.Id.ToString(),
                    FullName = p.Author!.FullName
                })
                .Distinct();

            return await postAuthors.ToListAsync();
        }
        #endregion

        #region GET AUTHOR BY ID
        public async Task<UserDTO?> GetAuthorByIdAsync(string id)
        {
            // Search for the author in Blogs
            var blogAuthor = await GetBlogAuthorByIdAsync(id);
            if (blogAuthor != null)
            {
                return blogAuthor;
            }

            // If not found in Blogs, search for the author in Posts
            return await GetPostAuthorByIdAsync(id);
        }
        #endregion

        #region GET BLOG AUTHOR BY ID
        public async Task<UserDTO?> GetBlogAuthorByIdAsync(string id)
        {
            // Search for the author in Blogs
            var blogAuthor = await _context.Blogs
                .Where(b => b.Author != null && b.Author.Id.ToString() == id)
                .Select(b => new UserDTO
                {
                    Id = b.Author!.Id.ToString(),
                    FullName = b.Author!.FullName
                })
                .FirstOrDefaultAsync();

            return blogAuthor;
        }
        #endregion

        #region GET POST AUTHOR BY ID
        public async Task<UserDTO?> GetPostAuthorByIdAsync(string id)
        {
            //search for the author in Posts
            var postAuthor = await _context.Posts
                .Where(p => p.Author != null && p.Author.Id.ToString() == id)
                .Select(p => new UserDTO
                {
                    Id = p.Author!.Id.ToString(),
                    FullName = p.Author!.FullName
                })
                .FirstOrDefaultAsync();
            return postAuthor;
        }
        #endregion

        #region GET ALL ADMINISTRATORS
        public async Task<IEnumerable<UserDTO?>> GetAllAdministratorsAsync()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Administrator");

            var administrators = usersInRole
                .Select(u => new UserDTO
                {
                    Id = u.Id.ToString(),
                    FullName = u.FullName
                })
                .Distinct();

            return administrators;
        }
        #endregion

        #region GET ADMINISTRATOR BY ID/FULL NAME
        public async Task<UserDTO?> GetAdministratorByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null && await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                return new UserDTO
                {
                    Id = user.Id.ToString(),
                    FullName = user.FullName
                };
            }

            throw new KeyNotFoundException($"Administrator with ID {id} was not found.");
        }
        #endregion

    }

}

