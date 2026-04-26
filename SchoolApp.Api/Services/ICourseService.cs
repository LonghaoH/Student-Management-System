using SchoolApp.Api.DTOs;

namespace SchoolApp.Api.Services;

// Defines the course-related operations.
public interface ICourseService
{
    Task<IEnumerable<CourseResponseDto>> GetAllCoursesAsync();
    Task<CourseResponseDto?> GetCourseByIdAsync(int id);
    Task<CourseResponseDto> CreateCourseAsync(CourseRequestDto dto);
    Task<CourseResponseDto?> UpdateCourseAsync(int id, CourseRequestDto dto);
    Task<bool> DeleteCourseAsync(int id);
}
