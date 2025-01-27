using E_Learning.Models;

namespace E_Learning.Repository
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment> EnrollInCourse(ApplicationUser user, int courseId);
        Task<Enrollment> UnenrollFromCourse(ApplicationUser user, int courseId);
    }
}
