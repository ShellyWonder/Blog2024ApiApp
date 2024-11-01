using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog2024Api.Services.Interfaces;
using Blog2024Api.Models;
using Blog2024Api.Identity;

namespace Blog2024Api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPostService _postService;

        #region CONSTRUCTOR
        public CommentsController(ICommentService commentService,
                                  IApplicationUserService applicationUserService,
                                  UserManager<ApplicationUser> userManager,
                                  IPostService postService)

        {
            _commentService = commentService;
            _applicationUserService = applicationUserService;
            _userManager = userManager;
            _postService = postService;
        }
        #endregion

        #region GET COMMENTS
        [HttpGet]
        [Authorize(Roles = "Moderator, Administrator")]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return Ok(comments);
        }
        #endregion

        #region GET DETAILS
        // GET: api/Blogs/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Moderator, Commentator, Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(new { message = $"Comment with ID {id} was not found." });
            }

            var comment = await _commentService.GetCommentByIdAsync(id.Value);
            if (comment == null)
            {
                return NotFound(new { message = $"Comment with ID {id} was not found." });
            }

            return Ok(comment);
        }
        #endregion

        #region POST COMMENT CREATE
        //NOTE: comment author = Commentator
        [HttpPost]
        [Authorize(Roles = "Commentator")]
        public async Task<ActionResult<Comment>> Create([FromForm] Comment comment)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Capture the user's ID and assign it to the CommentatorId
            comment.CommentatorId = _userManager.GetUserId(User);
            comment.Created = DateTime.Now;
            await _commentService.CreateCommentAsync(comment);
            return CreatedAtAction(nameof(Details), new { id = comment.Id }, comment);
        }
        #endregion

        #region COMMENT EDIT/UPDATE
        // PUT: api/Comments/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Commentator")]
        public async Task<IActionResult> Update(int id, [FromBody] Comment comment)
        {
            if (id != comment.Id) return NotFound(new { message = $"Comment with ID {id} was not found." });

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingComment = await _commentService.GetExistingCommentAsync(id);

            if (existingComment == null) return NotFound(new { message = $"Comment with ID {id} was not found." });

            try
            {
                existingComment!.Body = comment.Body;
                existingComment.Updated = DateTime.Now;
                await _commentService.UpdateCommentAsync(comment);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _commentService.CommentExistsAsync(comment.Id))
                {
                    return NotFound(new { message = $"Comment with ID {id} was not found." });
                }
                else
                {
                    throw;
                }
            }
            return NoContent();

        }
        #endregion

        #region COMMENT MODERATE/UPDATE
        [HttpPut("{id}/moderate")]
        [Authorize(Roles = "Moderator,Administrator")]
        public async Task<IActionResult> Moderate(int id, [FromBody] Comment comment)
        {
            if (id != comment.Id) return NotFound(new { message = $"Comment with ID {id} was not found." });

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingComment = await _commentService.GetExistingCommentAsync(id);

            if (existingComment == null) return NotFound(new { message = "Comment does not exist." });

            try
            {
                existingComment!.ModeratedBody = comment.ModeratedBody;
                existingComment!.ModerationReason = comment.ModerationReason;

                existingComment.Moderated = DateTime.Now;
                existingComment!.ModeratorId = _userManager.GetUserId(User);
                await _commentService.UpdateCommentAsync(comment);
            }
            catch (DbUpdateConcurrencyException)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the comment." });
            }
            return Ok(new { message = "Comment successfully moderated." });
        }
        #endregion

        #region COMMENT DELETE        
        // DELETE: api/Comments/{id}/{slug}
        [HttpDelete("{id}/{slug}")]
        [Authorize(Roles = "Moderator, Commentator, Administrator")]
        public async Task<IActionResult> Delete(int id, string slug)
        {
            await _commentService.DeleteCommentAsync(id);
            return NoContent();
        }
        #endregion

    }
}
