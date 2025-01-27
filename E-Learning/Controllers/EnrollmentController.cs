using E_Learning.Models;
using E_Learning.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEnrollmentRepository enrollmentRepository;

        public EnrollmentController(UserManager<ApplicationUser> userManager, IEnrollmentRepository enrollmentRepository)
        {
            this.userManager = userManager;
            this.enrollmentRepository = enrollmentRepository;
        }
        [HttpPost("EnrollInCourse")]
        public async Task<IActionResult> EnrollInCourse([FromBody] int courseId)
        {
            var user = await userManager.GetUserAsync(User);
            var res = await enrollmentRepository.EnrollInCourse(user, courseId);
            if (res != null)
            {
                return Ok(res);
            }
            return NotFound();

        }

        [HttpDelete("UnenrollFromCourse/{courseId}")]
        public async Task<IActionResult> UnenrollFromCourse(int courseId)
        {
            var user = await userManager.GetUserAsync(User);
            var res = await enrollmentRepository.UnenrollFromCourse(user, courseId);
            if(res != null)
            {
                return NoContent();
            }
            return NotFound();

        }
    }
}
