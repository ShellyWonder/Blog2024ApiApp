using Blog2024ApiApp.Data;
using Blog2024ApiApp.Extensions;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                              throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Identity configuration w/Roles
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    //Use for email confirmation, password reset, etc.
    .AddDefaultTokenProviders();

// Add Google and GitHub Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    // Ensure the default challenge scheme is set
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddGoogle(GoogleDefaults.AuthenticationScheme, googleOptions =>
    {
        var ClientId = builder.Configuration["Authentication:Google:ClientId"];
        var ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

        if (string.IsNullOrEmpty(ClientId) || string.IsNullOrEmpty(ClientSecret))
        {
            throw new Exception("Google ClientId or ClientSecret is not set in the configuration.");
        }

        googleOptions.ClientId = ClientId;
        googleOptions.ClientSecret = ClientSecret;
    })
    .AddGitHub(githubOptions =>
    {
        var ClientId = builder.Configuration["Authentication:GitHub:ClientId"];
        var ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"];

        if (string.IsNullOrEmpty(ClientId) || string.IsNullOrEmpty(ClientSecret))
        {
            throw new Exception("GitHub ClientId or ClientSecret is not set in the configuration.");
        }

        githubOptions.ClientId = ClientId;
        githubOptions.ClientSecret = ClientSecret;
    });

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllers();
builder.Services.AddCustomServices(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Call to seed the database(DatabaseSeederExtensions.cs)
await app.SeedDatabaseAsync();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    builder.Configuration.AddUserSecrets<Program>();
    app.UseMigrationsEndPoint();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var errorResponse = new { message = "An unexpected error occurred." };
        await context.Response.WriteAsJsonAsync(errorResponse);
    });
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

////custom route
//app.MapControllerRoute(
//    name: "SlugRoute",
//    pattern: "BlogPosts/UrlFriendly/{slug}",
//    defaults: new { controller = "Posts", action = "Details" });

app.Run();
