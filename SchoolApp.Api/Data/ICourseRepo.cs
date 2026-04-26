using SchoolApp.Api.Models;

namespace SchoolApp.Api.Data;

// Defines raw data access for Courses - works with model objects, not DTOs.
// The service layer is responsible for mapping between models and DTOs.
public interface ICourseRepo
{
    Task<IEnumerable<Course>> GetAllCoursesAsync();
    Task<Course?> GetCourseByIdAsync(int id);
    Task<Course> AddCourseAsync(Course course);
    Task<Course?> UpdateCourseAsync(Course course);
    Task DeleteCourseAsync(Course course);
}
