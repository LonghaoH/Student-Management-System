using SchoolApp.Api.Data;
using SchoolApp.Api.DTOs;
using SchoolApp.Api.Models;

namespace SchoolApp.Api.Services;

// CourseService sits between the controller and the repo.
// It receives DTOs from the controller, maps them to/from models,
// and delegates the actual database work to ICourseRepo.
public class CourseService : ICourseService
{
    private readonly ICourseRepo _repo;

    // ICourseRepo is injected by ASP.NET's DI container - registered in Program.cs.
    public CourseService(ICourseRepo repo)
    {
        _repo = repo;
    }

    // Fetches all courses and converts each model to a response DTO.
    public async Task<IEnumerable<CourseResponseDto>> GetAllCoursesAsync()
    {
        var courses = await _repo.GetAllCoursesAsync();
        return courses.Select(ToDto);
    }

    // Fetches a single course by PK. Returns null if not found,
    // which the controller will translate to a 404.
    public async Task<CourseResponseDto?> GetCourseByIdAsync(int id)
    {
        var course = await _repo.GetCourseByIdAsync(id);
        return course is null ? null : ToDto(course);
    }

    // Maps the incoming DTO to a new Course model, then hands it to the repo to insert.
    // The repo returns the saved entity with its DB-generated CourseId populated.
    public async Task<CourseResponseDto> CreateCourseAsync(CourseRequestDto dto)
    {
        var course = new Course
        {
            Title = dto.Title,
            InstructorId = dto.InstructorId
        };

        var created = await _repo.AddCourseAsync(course);
        return ToDto(created);
    }

    // Fetches the existing course, applies the new values from the DTO, then saves.
    // Returns null if no course with that ID exists.
    public async Task<CourseResponseDto?> UpdateCourseAsync(int id, CourseRequestDto dto)
    {
        var course = await _repo.GetCourseByIdAsync(id);
        if (course is null) return null;

        // Mutate the tracked entity directly - EF Core detects the changes automatically.
        course.Title = dto.Title;
        course.InstructorId = dto.InstructorId;

        var updated = await _repo.UpdateCourseAsync(course);
        return updated is null ? null : ToDto(updated);
    }

    // Fetches the course first so we can confirm it exists before attempting a delete.
    // Returns false if not found (controller maps this to a 404).
    public async Task<bool> DeleteCourseAsync(int id)
    {
        var course = await _repo.GetCourseByIdAsync(id);
        if (course is null) return false;

        await _repo.DeleteCourseAsync(course);
        return true;
    }

    // Centralises the model-to-DTO mapping so each method doesn't repeat it.
    private static CourseResponseDto ToDto(Course c) => new()
    {
        CourseId = c.CourseId,
        Title = c.Title,
        InstructorId = c.InstructorId
    };
}
