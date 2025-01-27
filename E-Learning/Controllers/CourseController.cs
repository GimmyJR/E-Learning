using E_Learning.Models;
using E_Learning.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICourseRepository courseRepository;

        public CourseController(UserManager<ApplicationUser> userManager,ICourseRepository courseRepository)
        {
            this.userManager = userManager;
            this.courseRepository = courseRepository;
        }

        [HttpPost("CreateCourse")]
        public async Task<IActionResult> CreateCourse([FromBody] Course course)
        {
            if (ModelState.IsValid)
            {
                var instructor = await userManager.GetUserAsync(User);
                await courseRepository.CreateCourse(course, instructor!);
                return CreatedAtAction(nameof(CreateCourse), new { id = course.Id }, course);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("UpdateCourse/{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course updatedCourse)
        {

            if (ModelState.IsValid)
            {
                var course =await courseRepository.UpdateCourse(id,updatedCourse);
                if (course == null)
                {
                    return NotFound();
                }
                return Ok(course);  
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("DeleteCourse/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            courseRepository.DeleteCourse(id);
            return NoContent(); 
        }
    }
}
