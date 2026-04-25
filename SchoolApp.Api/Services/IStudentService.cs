using SchoolApp.Api.DTOs;

namespace SchoolApp.Api.Services;

// Defines the student-related operations.
public interface IStudentService
{
    Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync();
    Task<StudentResponseDto?> GetStudentByIdAsync(int id);
    Task<StudentResponseDto> CreateStudentAsync(StudentRequestDto dto);
    Task<StudentResponseDto?> UpdateStudentAsync(int id, StudentRequestDto dto);
    Task<bool> DeleteStudentAsync(int id);
}
