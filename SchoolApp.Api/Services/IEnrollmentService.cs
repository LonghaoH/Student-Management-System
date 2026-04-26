using SchoolApp.Api.DTOs;

namespace SchoolApp.Api.Services;

// Defines the enrollment-related operations.
public interface IEnrollmentService
{
    Task<IEnumerable<EnrollmentResponseDto>> GetAllEnrollmentsAsync();
    // Enrollment has no single PK - StudentId and CourseId together identify a record.
    Task<EnrollmentResponseDto?> GetEnrollmentByIdAsync(int studentId, int courseId);
    Task<EnrollmentResponseDto> CreateEnrollmentAsync(EnrollmentRequestDto dto);
    Task<EnrollmentResponseDto?> UpdateEnrollmentAsync(int studentId, int courseId, EnrollmentRequestDto dto);
    Task<bool> DeleteEnrollmentAsync(int studentId, int courseId);
}
