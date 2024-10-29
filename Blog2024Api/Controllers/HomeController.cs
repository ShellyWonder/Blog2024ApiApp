using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog2024ApiApp.Enums;
using Blog2024ApiApp.Models;
using Blog2024ApiApp.Services.Interfaces;
using Blog2024ApiApp.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Blog2024ApiApp.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogEmailSender _emailSender;
        private readonly IBlogService _blogService;

    #region  CONSTRUCTOR
        public HomeController(ILogger<HomeController> logger, 
                                    IBlogEmailSender emailSender,
                                    IBlogService blogService)
        {
            _logger = logger;
            _emailSender = emailSender;
            _blogService = blogService;
        }
        #endregion

        #region GET INDEX/productionReady posts by blog
        // GET: api/Home/Index/productionReady posts by blog
        [HttpGet("Index")]
        public async Task<ActionResult<IEnumerable<Blog>>> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 5;
            //Grabs all status: productionReady posts by blog in descending order by date
            var blogs = await _blogService.GetBlogsByStateAsync(PostState.ProductionReady, pageNumber, pageSize);
            return Ok(blogs);
        }
        #endregion

        #region GET ABOUT
        // GET: api/Home/About
        [HttpGet("About")]
        public IActionResult About()
        {
            return Ok();
        }
        #endregion

        #region GET CONTACT
        [HttpGet("Contact")]
        public IActionResult Contact()
        {
            return Ok();
        }
        #endregion

        #region POST CONTACT
        [HttpPost("Contact")]
        public async Task<IActionResult> Contact([FromBody] ContactUsDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                //build email
                model.Message = $"{model.Message}<hr>" +
            //using the Ternary (Conditional) operator (??) to check whether model.Phone is null 
            $"Phone: {(string.IsNullOrWhiteSpace(model.Phone) ? "Phone: Not Provided" : model.Phone)}";
                await _emailSender.SendContactEmailAsync(model.Email, model.Name, model.Subject, model.Message);

                return Ok(new {message = "Your message has been sent successfully." });
            }
            catch (Exception)
            {
                return NotFound(new { message = "Your message could not be sent. Please try again." });
            }
        }
        #endregion
 
    }
}
