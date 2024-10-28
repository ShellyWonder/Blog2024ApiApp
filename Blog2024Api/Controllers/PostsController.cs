using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Blog2024ApiApp.Data;
using Blog2024ApiApp.Enums;
using Blog2024ApiApp.Models;
using Blog2024ApiApp.Services.Interfaces;

namespace Blog2024ApiApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IBlogService _blogService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IImageService _imageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISlugService _slugService;
        private readonly ITagService _tagService;
        private readonly ISearchService _searchService;

        #region CONSTRUCTOR
        public PostsController(IPostService postService, IBlogService blogService,
                                     IApplicationUserService applicationUserService,
                                     IImageService imageService,
                                     UserManager<ApplicationUser> userManager,
                                     ISlugService slugService, ITagService tagService,
                                     ISearchService searchService)
        {
            _postService = postService;
            _blogService = blogService;
            _applicationUserService = applicationUserService;
            _imageService = imageService;
            _userManager = userManager;
            _slugService = slugService;
            _tagService = tagService;
            _searchService = searchService;

        }
        #endregion

        #region SEARCH INDEX
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Post>>> SearchIndex(int? page, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return NotFound($"The search term '{searchTerm}' was not found. Please try again.");
            }

            var pageNumber = page ?? 1;
            var pageSize = 5;
            var posts = await _searchService.SearchPostsAsync(PostState.ProductionReady,pageNumber,pageSize, searchTerm);
            return Ok(posts);
        }
        #endregion

        #region GET POSTS/INDEX
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Post>>>Index()
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(posts);
        }
        #endregion

        #region GET ALL BLOG POSTS INDEX BY BLOG
        [HttpGet("blog/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Post>>> BlogPostIndex(int id, int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 5;
            var posts = await _postService.GetAllPostsByStateAsync(PostState.ProductionReady, pageNumber, pageSize, id);
            return Ok(posts);
        }
        #endregion

        #region GET DETAILS 
        [HttpGet("{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var post = await _postService.GetPostBySlugAsync(slug);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }
        #endregion

        #region POST CREATE
        [HttpPost]
        [Authorize(Roles = "Administrator, Author")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Post>> PostCreate([FromBody] Post post,List<string>tagValues)
        {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                try
                {
                    // Get the current user ID
                    var authorId = _userManager.GetUserId(User);
                    post.AuthorId = authorId;

                    #region SLUG CREATION AND VALIDATION 
                    //
                    var slug = _slugService.UrlFriendly(post.Title);

                    //check for blank or malformed slugs
                    if (string.IsNullOrEmpty(slug) || !_slugService.IsUnique(slug))
                    {
                        return BadRequest("Invalid or duplicate title. Please choose another.");
                    }
                     post.Slug = slug;
                    #endregion

                    #region IMAGE HANDLING
                    // Convert the uploaded image to a byte array and store it in database
                    post = await _imageService.SetImageAsync(post);
                    #endregion

                    #region TAG CREATION AND VALIDATION

                    foreach (var tagText in tagValues)
                    {
                        // Add each tag using the tag service/repository method
                        await _tagService.AddTagAsync(tagText, post.Id, authorId!);
                    }

                    #endregion 

                    #region SAVE
                    // Pass userId to the service/repository
                    await _postService.AddPostAsync(post, authorId!);
                        return CreatedAtAction(nameof(Details), new { slug = post.Slug }, post);
                }
                catch (Exception)
                {

                    return NotFound($"An error occurred while creating the post.");
                }
            #endregion
        }
        #endregion

        #region EDIT/UPDATE
       [HttpPut("{id}")]
        [Authorize(Roles = "Administrator, Author")]
        public async Task<IActionResult> Update(int id, [FromBody] Post post,
                                                                     IFormFile newImage,
                                                                     List<string>tagValues)
        {
            if (id != post.Id) return NotFound($"Post with ID {post.Id} was not found.");

            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            try
            {
                // Get the current user ID
                var userId = _userManager.GetUserId(User);
                // Get current blog ID FROM EXISTING POST
                var newPost = await _postService.GetPostByIdAsync(post.Id);

                if (newPost.Title != newPost.Title) newPost.Title = post.Title;
                if (newPost.Abstract != newPost.Abstract) newPost.Abstract = post.Abstract;
                if (newPost.Content != newPost.Content) newPost.Content = post.Content;
                if (newPost.BlogPostState != newPost.BlogPostState) newPost.BlogPostState = post.BlogPostState;

                #region SLUGS
                // Generate a new slug from the updated title and validate uniqueness
                var newSlug = _slugService.UrlFriendly(post.Title);

                if (newSlug != newPost.Slug &&  _slugService.IsUnique(newSlug))
                {
                    newPost.Slug = newSlug;
                }
                else
                {
                    return BadRequest($"This title, {"title"} is a duplicate of a previous post. Please choose another title");

                }
                #endregion

                #region IMAGE
                // If a new image is provided, process and update it
                if (newImage != null)
                {
                    // Assign the new image to the ImageFile property of the Blog object
                    newPost.ImageFile = newImage;
                    // Convert the uploaded image to a byte array and store it in the database
                    newPost = await _imageService.SetImageAsync(newPost);
                }
                else
                {
                    return BadRequest($"There was a problem processing this image. Please choose another image.");

                }
                #endregion

                #region TAGS
                //Remove all tags associated with the post
                await _tagService.RemoveAllTagsByPostIdAsync(post.Id);

                // Add the new tags to the post
                foreach (var tagText in tagValues)
                {
                    await _tagService.AddTagAsync(tagText, post.Id, userId!);
                }
               #endregion

                #region SAVE
                // Pass userId to the service/repository,updating the rest of the post
                await _postService.UpdatePostAsync(post, userId!);
                return NoContent();
                #endregion
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Post with ID {post.Id} was not found.");
            }
        }
        #endregion

        #region POST DELETE
        [HttpDelete("{id}")]
        [Authorize (Roles= "Administrator, Author")]
        public async Task<IActionResult> PostDelete(int id)
        {
            if (!await PostExists(id)) return NotFound();

            await _postService.DeletePostAsync(id);
            return NoContent();
        }

        private async Task<bool> PostExists(int id)
        {
            return await _postService.PostExistsAsync(id);
        }
        #endregion

       
        
    }
    
}
