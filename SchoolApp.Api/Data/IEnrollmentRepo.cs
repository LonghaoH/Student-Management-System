using SchoolApp.Api.Models;

namespace SchoolApp.Api.Data;

// Defines raw data access for Enrollments - works with model objects, not DTOs.
// The service layer is responsible for mapping between models and DTOs.
public interface IEnrollmentRepo
{
    Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync();
    // Enrollment has no single PK - StudentId and CourseId together form the composite key.
    Task<Enrollment?> GetEnrollmentByIdAsync(int studentId, int courseId);
    Task<Enrollment> AddEnrollmentAsync(Enrollment enrollment);
    Task<Enrollment?> UpdateEnrollmentAsync(Enrollment enrollment);
    Task DeleteEnrollmentAsync(Enrollment enrollment);
}
