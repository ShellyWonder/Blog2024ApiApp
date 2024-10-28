using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog2024ApiApp.Models;
using Blog2024ApiApp.Services.Interfaces;

namespace Blog2024ApiApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        
        private readonly ITagService _tagService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPostService _postService;

    #region  CONSTRUCTOR
        public TagsController(ITagService tagsService,
                                IApplicationUserService applicationUserService,
                                 IPostService postService)
        {
            _tagService = tagsService;
            _applicationUserService = applicationUserService;
            _postService = postService;
        }
        #endregion

        #region GET TAGS/INDEX
        [HttpGet]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(tags);
        }
        #endregion

        #region GET DETAILS
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound(new { Message = $"Requested tag{id} was not found. Please select another tag." });
           
            var tag = await _tagService.GetTagByIdAsync(id.Value);

            if (tag == null) return NotFound(new { Message = $"Requested tag{id} was not found. Please select another tag." });
            
            return Ok(tag);
        }
        #endregion

        #region TAG CREATE
        [HttpPost]
        [Authorize(Roles = "Administrator, Author")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Tag tag)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _tagService.AddTagAsync(tag.Text!, tag.PostId, tag.AuthorId!);
                return CreatedAtAction(nameof(Details), new { id = tag.Id }, tag);
            }

            catch (Exception)
            {
                return NotFound(new { message = $"An error occurred while creating the tag." });

            }
        }
            #endregion

        #region POST EDIT/UPDATE
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator, Author")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateTag(int id, [FromBody] Tag tag)
        {
            if (id != tag.Id) return NotFound(new { message = $"Tag {tag.Id} not found." });
            
            if (!ModelState.IsValid) return BadRequest(ModelState);
                
            try
            {
                await _tagService.UpdateTagAsync(tag);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(tag.Id))
                {
                return NotFound(new { message = $"Tag {tag.Id} not found." });
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        #endregion

        #region TAG DELETE
            [HttpDelete("{id}")]
            [Authorize(Roles = "Author, Administrator")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteTag(int id)
            {
                await _tagService.DeleteTagAsync(id);
                return NoContent();
            }
            #endregion

        #region TAG EXISTS
            private bool TagExists(int id)
            {
                return _tagService.TagExists(id);
            }
            #endregion
    }
}
