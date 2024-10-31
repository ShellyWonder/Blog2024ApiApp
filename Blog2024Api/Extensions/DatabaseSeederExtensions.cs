using Blog2024Api.Data.SeedData;
using Blog2024Api.Data.SeedData;

namespace Blog2024Api.Extensions
{
    public  static class DatabaseSeederExtensions
    {
        public static async Task SeedDatabaseAsync(this IHost app)
        {
            using (var scope = app.Services.CreateScope())
            {
                // Call the SetupDB method to run migrations and seed roles/users
                var rolesDataService = scope.ServiceProvider.GetRequiredService<RolesDataService>();
                await rolesDataService.SetupDBAsync();

                // Call the PostsDataService to seed posts and comments
                var postsDataService = scope.ServiceProvider.GetRequiredService<PostsDataService>();
                await postsDataService.SeedPostsAndCommentsAsync();

                // Call the BlogsDataService to seed blogs
                var blogsDataService = scope.ServiceProvider.GetRequiredService<BlogsDataService>();
                await blogsDataService.InitializeAsync();

            }
        }
    }
}
