using E_Learning.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_Learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeRepository homeRepository;

        public HomeController(IHomeRepository homeRepository)
        {
            this.homeRepository = homeRepository;
        }
        [HttpGet]
        public IActionResult GetCourses()
        {
            var courses = homeRepository.GetCourses();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public IActionResult GetCourseDetails(int id)
        {
            var course = homeRepository.GetCourseDetails(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }
        [HttpGet("search")]
        public IActionResult SearchCourses([FromQuery] string query)
        {
            var courses = homeRepository.SearchCourses(query);
            return Ok(courses);
        }
        [HttpGet("filter")]    
        public IActionResult FilterCourses([FromQuery] string category, [FromQuery] string level)
        {
            var courses = homeRepository.FilterCourses(category, level);
            return Ok(courses);
        }
    }
}
