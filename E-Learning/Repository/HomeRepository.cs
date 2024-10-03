using E_Learning.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository
{
    public class HomeRepository:IHomeRepository
    {
        private readonly AppDbContext context;

        public HomeRepository(AppDbContext context)
        {
            this.context = context;
        }
        public List<Course> GetCourses()
        {
            List<Course> courses = context.Courses.Include(c => c.Instructor).ToList();
            return courses;
        }

        public Course GetCourseDetails(int id)
        {
            var course = context.Courses
            .Include(c => c.Instructor)
            .Include(c => c.Reviews).ThenInclude(r => r.User)
            .FirstOrDefault(c => c.Id == id);

            return course;
        }

        public List<Course> SearchCourses(string query)
        {
            var courses = context.Courses.Where(c => c.Title.Contains(query) || c.Category.Contains(query)).ToList();
            return courses;
        }

        public List<Course> FilterCourses(string category, string level)
        {
            var courses = context.Courses
            .Where(c => c.Category == category && c.Level.ToString() == level)
            .ToList();
            return courses;
        }
    }
}
