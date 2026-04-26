using SchoolApp.Api.Data;
using SchoolApp.Api.DTOs;
using SchoolApp.Api.Models;

namespace SchoolApp.Api.Services;

// EnrollmentService sits between the controller and the repo.
// It receives DTOs from the controller, maps them to/from models,
// and delegates the actual database work to IEnrollmentRepo.
public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepo _repo;

    // IEnrollmentRepo is injected by ASP.NET's DI container - registered in Program.cs.
    public EnrollmentService(IEnrollmentRepo repo)
    {
        _repo = repo;
    }

    // Fetches all enrollments and converts each model to a response DTO.
    public async Task<IEnumerable<EnrollmentResponseDto>> GetAllEnrollmentsAsync()
    {
        var enrollments = await _repo.GetAllEnrollmentsAsync();
        return enrollments.Select(ToDto);
    }

    // Fetches a single enrollment by its composite key. Returns null if not found,
    // which the controller will translate to a 404.
    public async Task<EnrollmentResponseDto?> GetEnrollmentByIdAsync(int studentId, int courseId)
    {
        var enrollment = await _repo.GetEnrollmentByIdAsync(studentId, courseId);
        return enrollment is null ? null : ToDto(enrollment);
    }

    // Maps the incoming DTO to a new Enrollment model, then hands it to the repo to insert.
    // Both StudentId and CourseId are required - they form the composite key.
    public async Task<EnrollmentResponseDto> CreateEnrollmentAsync(EnrollmentRequestDto dto)
    {
        var enrollment = new Enrollment
        {
            StudentId = dto.StudentId,
            CourseId = dto.CourseId,
            Grade = dto.Grade
        };

        var created = await _repo.AddEnrollmentAsync(enrollment);
        return ToDto(created);
    }

    // Fetches the existing enrollment by composite key, then updates the grade.
    // Returns null if no enrollment with that key exists.
    public async Task<EnrollmentResponseDto?> UpdateEnrollmentAsync(int studentId, int courseId, EnrollmentRequestDto dto)
    {
        var enrollment = await _repo.GetEnrollmentByIdAsync(studentId, courseId);
        if (enrollment is null) return null;

        // Mutate the tracked entity directly - EF Core detects the changes automatically.
        // StudentId and CourseId are the composite key and cannot be changed - only Grade is updated.
        enrollment.Grade = dto.Grade;

        var updated = await _repo.UpdateEnrollmentAsync(enrollment);
        return updated is null ? null : ToDto(updated);
    }

    // Fetches the enrollment first so we can confirm it exists before attempting a delete.
    // Returns false if not found (controller maps this to a 404).
    public async Task<bool> DeleteEnrollmentAsync(int studentId, int courseId)
    {
        var enrollment = await _repo.GetEnrollmentByIdAsync(studentId, courseId);
        if (enrollment is null) return false;

        await _repo.DeleteEnrollmentAsync(enrollment);
        return true;
    }

    // Centralises the model-to-DTO mapping so each method doesn't repeat it.
    private static EnrollmentResponseDto ToDto(Enrollment e) => new()
    {
        StudentId = e.StudentId,
        CourseId = e.CourseId,
        Grade = e.Grade
    };
}
