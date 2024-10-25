﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Blog2024ApiApp.Data;
using Blog2024ApiApp.Models;
using Blog2024ApiApp.Services.Interfaces;

namespace Blog2024ApiApp.Controllers
{

    [Route("api/[Controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageService _imageService;

        public BlogsController(IBlogService blogService, UserManager<ApplicationUser> userManager, IImageService imageService)
        {
            _blogService = blogService;
            _userManager = userManager;
            _imageService = imageService;
        }
      
        #region GET All BLOGS INDEX
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Blog>>> GetAllBlogs()
        {
            var blogs = await _blogService.GetAllBlogsAsync();
            return Ok(blogs);
        }
        #endregion

        #region GET SINGLE BLOG DETAILS
        // GET: api/Blogs/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Blog>>Details(int? id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id!.Value);
            
            if (blog == null)
            {
                return NotFound(new { message = $"Blog with ID {id} was not found." });
            }

            return Ok(blog);
        }
        #endregion

        #region POST BLOG CREATE
        [HttpPost]
        [Authorize(Roles = "Administrator,Author")]
        public async Task<ActionResult<Blog>>BlogCreate([FromForm]Blog blog)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
           
            var userId = _userManager.GetUserId(User);
            
            if (userId is null) return NotFound(new { message = $"User Id {userId} is not found." });

            blog = await ImageImplementationAsync(blog);

            await _blogService.CreateBlogAsync(blog, userId);
            return CreatedAtAction(nameof(Details), new {id = blog.Id}, blog);
        }
        #endregion

        #region PUT EDIT
        // PUT: api/Blogs/{id}
        [HttpPut ("{id}")]
        [Authorize (Roles="Administrator, Author")]
        public async Task<IActionResult> BlogUpdate(int id, [FromForm] Blog blog, IFormFile newImage)
        {
            if (id != blog.Id) return NotFound(new { message = $"Mismatched Blog ID." });
      
            if (!ModelState.IsValid) return NotFound(ModelState);
            
            var existingBlog = await _blogService.GetBlogByIdAsync(id);
            
            if (existingBlog == null) return NotFound(new { message = $"Blog with ID {id} was not found." });

            existingBlog.Name = blog.Name;
            existingBlog.Description = blog.Description;

            if (newImage != null)
            {
                existingBlog.ImageFile = newImage;
                existingBlog = await ImageImplementationAsync(existingBlog);
            }

            var userId = _userManager.GetUserId(User);
            
            if (userId is null) return NotFound(new { message = $"User Id {userId} is not found." });
            
            await _blogService.UpdateBlogAsync(existingBlog, userId);
            return NoContent();
        }
        #endregion

        #region DELETE
        // DELETE: api/Blogs/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,Author")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blogExists = await _blogService.BlogExistsAsync(id);
             
            if (!blogExists) return NotFound(new { message = $"Blog with ID {id} was not found." });

            await _blogService.DeleteBlogAsync(id);
            return NoContent();
        }
        #endregion

        #region BLOG EXISTS
        private async Task<bool> BlogExists(int id)
        {
            return await _blogService.BlogExistsAsync(id);
        }
        #endregion

        #region IMG IMPLEMENTATION
        private async Task<Blog> ImageImplementationAsync(Blog blog)
        {
            if (blog.ImageFile != null)
            {    //Convert incoming file into a byte array
                blog.Image = await _imageService.ConvertFileToByteArrayAsync(blog.ImageFile);
                blog.ImageType = blog.ImageFile.ContentType;
            }
            else
            {
                // Assign default image if no image is uploaded
                var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "default_icon.png");
                blog.Image = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                blog.ImageType = "image/png";
            }
            return blog;
        }
        #endregion
    }
}