using SchoolApp.Api.Models;

namespace SchoolApp.Api.Data;

// Defines raw data access for Students - works with model objects, not DTOs.
// The service layer is responsible for mapping between models and DTOs.
public interface IStudentRepo
{
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    Task<Student?> GetStudentByIdAsync(int id);
    Task<Student> AddStudentAsync(Student student);
    Task<Student?> UpdateStudentAsync(Student student);
    Task DeleteStudentAsync(Student student);
}
