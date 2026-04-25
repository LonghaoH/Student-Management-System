using SchoolApp.Api.DTOs;

namespace SchoolApp.Api.Services;

// Defines instructor-related operations.
public interface IInstructorService
{
    Task<IEnumerable<InstructorResponseDto>> GetAllInstructorsAsync();
    Task<InstructorResponseDto?> GetInstructorByIdAsync(int id);
    Task<InstructorResponseDto> CreateInstructorAsync(InstructorRequestDto dto);
    Task<InstructorResponseDto?> UpdateInstructorAsync(int id, InstructorRequestDto dto);
    Task<bool> DeleteInstructorAsync(int id);
}