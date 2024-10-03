using E_Learning.Models;

namespace E_Learning.Repository
{
    public interface IHomeRepository
    {
        List<Course> GetCourses();
        Course GetCourseDetails(int id);
        List<Course> SearchCourses(string query);
        List<Course> FilterCourses(string category, string level);
    }
}
