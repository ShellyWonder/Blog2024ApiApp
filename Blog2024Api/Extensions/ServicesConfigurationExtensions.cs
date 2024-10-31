using Microsoft.AspNetCore.Http.Features;
using Blog2024Api.Data.Repositories.Interfaces;
using Blog2024Api.Data.Repositories;
using Blog2024Api.Services.Interfaces;
using Blog2024Api.Services;
using Blog2024Api.DTO;
using Blog2024Api.Data.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Google;
using Blog2024Api.Data;

namespace Blog2024Api.Extensions
{
    public static class ServicesConfigurationExtensions
    {
        #region CUSTOM SERVICES
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register application services
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            services.AddScoped<IBlogEmailSender, EmailSender>();
            services.AddScoped<IImageService, ImageService>();


            services.AddScoped<ISlugRepository, SlugRepository>();
            services.AddScoped<ISlugService, SlugService>();
            services.AddScoped<ISearchRepository, SearchRepository>();
            services.AddScoped<ISearchService, SearchService>();
            //data seed services
            services.AddTransient<RolesDataService>();
            services.AddTransient<BlogsDataService>();
            services.AddTransient<PostsDataService>();
            //configure MailSettings
            services.Configure<MailSettingsDTO>(configuration.GetSection("MailSettings"));

            // Configure form options
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 1024 * 1024 * 10; // 10MB
            });

            return services;
        }
        #endregion

        #region IDENTITY WITH ROLES
        public static IServiceCollection AddIdentityWithRoles(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                //Use for email confirmation, password reset, etc.
                .AddDefaultTokenProviders();
            return services;
        }
        #endregion

        #region IDENTITY WITH EXTERNAL PROVIDERS
        public static IServiceCollection AddIdentityWithExternalProviders(this IServiceCollection services, IConfiguration configuration)
        {

            // Add Google and GitHub Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                // Ensure the default challenge scheme is set
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
                .AddGoogle(GoogleDefaults.AuthenticationScheme, googleOptions =>
                {
                    var ClientId = configuration["Authentication:Google:ClientId"];
                    var ClientSecret = configuration["Authentication:Google:ClientSecret"];

                    if (string.IsNullOrEmpty(ClientId) || string.IsNullOrEmpty(ClientSecret))
                    {
                        throw new Exception("Google ClientId or ClientSecret is not set in the configuration.");
                    }

                    googleOptions.ClientId = ClientId;
                    googleOptions.ClientSecret = ClientSecret;
                })
                .AddGitHub(githubOptions =>
                {
                    var ClientId = configuration["Authentication:GitHub:ClientId"];
                    var ClientSecret = configuration["Authentication:GitHub:ClientSecret"];

                    if (string.IsNullOrEmpty(ClientId) || string.IsNullOrEmpty(ClientSecret))
                    {
                        throw new Exception("GitHub ClientId or ClientSecret is not set in the configuration.");
                    }

                    githubOptions.ClientId = ClientId;
                    githubOptions.ClientSecret = ClientSecret;
                });
            return services;
        } 
        #endregion
    }

}
