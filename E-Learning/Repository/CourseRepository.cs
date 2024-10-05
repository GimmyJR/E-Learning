using E_Learning.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository
{
    public class CourseRepository:ICourseRepository
    {
        private readonly AppDbContext context;

        public CourseRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateCourse(Course course,ApplicationUser user)
        {
            course.Instructor = user;
            course.CreatedDate = DateTime.Now;
            course.UpdatedDate = DateTime.Now;
            context.Courses.Add(course);
            await context.SaveChangesAsync();

        }

        public async Task<Course> UpdateCourse(int id, Course updatedCourse)
        {
            var course = context.Courses.Find(id);
            if(course == null)
            {
                return null;
            }
            course.Title = updatedCourse.Title;
            course.Description = updatedCourse.Description;
            course.Price = updatedCourse.Price;
            course.Category = updatedCourse.Category;
            course.Level = updatedCourse.Level;
            course.ImageUrl = updatedCourse.ImageUrl;
            course.UpdatedDate = DateTime.Now;

            context.Courses.Update(course);
            await context.SaveChangesAsync();
            return course;
        }

        public async Task DeleteCourse(int id)
        {
            var course = context.Courses.Find(id);
            if (course != null)
            {
            context.Courses.Remove(course);
            await context.SaveChangesAsync();
            }
            
        }
    }
}
