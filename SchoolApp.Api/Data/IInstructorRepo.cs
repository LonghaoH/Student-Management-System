using SchoolApp.Api.Models;

namespace SchoolApp.Api.Data;

// Defines raw data access for Instructors - works with model objects, not DTOs.
// The service layer is responsible for mapping between models and DTOs.
public interface IInstructorRepo
{
    Task<IEnumerable<Instructor>> GetAllInstructorsAsync();
    Task<Instructor?> GetInstructorByIdAsync(int id);
    Task<Instructor> AddInstructorAsync(Instructor instructor);
    Task<Instructor?> UpdateInstructorAsync(Instructor instructor);
    Task DeleteInstructorAsync(Instructor instructor);
}