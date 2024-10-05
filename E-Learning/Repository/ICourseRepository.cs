using E_Learning.Models;

namespace E_Learning.Repository
{
    public interface ICourseRepository
    {
        Task CreateCourse(Course course, ApplicationUser user);
        Task<Course> UpdateCourse(int id, Course updatedCourse);

        Task DeleteCourse(int id);
    }
}
