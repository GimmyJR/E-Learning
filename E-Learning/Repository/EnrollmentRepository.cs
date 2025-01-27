using E_Learning.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository
{
    public class EnrollmentRepository:IEnrollmentRepository
    {
        private readonly AppDbContext context;

        public EnrollmentRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<Enrollment> EnrollInCourse(ApplicationUser user,int courseId)
        {
            var course = context.Courses.Find(courseId);
            if (course == null || user == null)
            {
                return null;
            }

            var enrollment = new Enrollment
            {
                CourseId = courseId,
                UserId = user.Id,
                EnrollmentDate = DateTime.Now,
            };

            context.Enrollments.Add(enrollment);
            await context.SaveChangesAsync();
            return enrollment;
        }

        public async Task<Enrollment> UnenrollFromCourse(ApplicationUser user, int courseId)
        {
            var enrollment = context.Enrollments
            .FirstOrDefault(e => e.CourseId == courseId && e.UserId == user.Id);

            if (enrollment == null)
            {
                return null;
            }

            context.Enrollments.Remove(enrollment);
            await context.SaveChangesAsync();
            return enrollment;
        }
    }
}
