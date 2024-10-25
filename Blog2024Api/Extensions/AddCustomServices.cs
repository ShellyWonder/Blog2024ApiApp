using Microsoft.AspNetCore.Http.Features;
using Blog2024ApiApp.Data.Repositories.Interfaces;
using Blog2024ApiApp.Data.Repositories;
using Blog2024ApiApp.Services.Interfaces;
using Blog2024ApiApp.Services;
using Blog2024ApiApp.DTO;
using Blog2024ApiApp.Data.SeedData;

namespace Blog2024ApiApp.Extensions
{
    public static class ServicesConfigurationExtensions
    {
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
    }

}
