using Blog2024Api.Data;
using Blog2024Api.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                              throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Identity configuration w/Roles
builder.Services.AddIdentityWithRoles(builder.Configuration);

// Add Google and GitHub Authentication
builder.Services.AddIdentityWithExternalProviders(builder.Configuration);

builder.Services.AddCustomServices(builder.Configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllers();

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
//NOTE:captures unhandled exceptions
//and sends a consistent error response format to the client. 
//The frontend can parse this JSON response
//and display an error using its own view model.
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
