using SchoolApp.Api.Data;
using SchoolApp.Api.DTOs;
using SchoolApp.Api.Models;

namespace SchoolApp.Api.Services;

// StudentService sits between the controller and the repo.
// It receives DTOs from the controller, maps them to/from models,
// and delegates the actual database work to IStudentRepo.
public class StudentService : IStudentService
{
    private readonly IStudentRepo _repo;

    // IStudentRepo is injected by ASP.NET's DI container - registered in Program.cs.
    public StudentService(IStudentRepo repo)
    {
        _repo = repo;
    }

    // Fetches all students and converts each model to a response DTO.
    public async Task<IEnumerable<StudentResponseDto>> GetAllStudentAsync()
    {
        var students = await _repo.GetAllStudentsAsync();
        return students.Select(ToDto);
    }

    // Fetches a single student by PK. Returns null if not found,
    // which the controller will translate to a 404.
    public async Task<StudentResponseDto?> GetStudentByIdAsync(int id)
    {
        var student = await _repo.GetStudentByIdAsync(id);
        return student is null ? null : ToDto(student);
    }

    // Maps the incoming DTO to a new Student model, then hands it to the repo to insert.
    // The repo returns the saved entity with its DB-generated StudentId populated.
    public async Task<StudentResponseDto> CreateStudentAsync(StudentRequestDto dto)
    {
        var student = new Student
        {
            Name = dto.Name,
            Email = dto.Email
        };

        var created = await _repo.AddStudentAsync(student);
        return ToDto(created);
    }

    // Fetches the existing student, applies the new values from the DTO, then saves.
    // Returns null if no student with that ID exists.
    public async Task<StudentResponseDto?> UpdateStudentAsync(int id, StudentRequestDto dto)
    {
        var student = await _repo.GetStudentByIdAsync(id);
        if (student is null) return null;

        // Mutate the tracked entity directly - EF Core detects the changes automatically.
        student.Name = dto.Name;
        student.Email = dto.Email;

        var updated = await _repo.UpdateStudentAsync(student);
        return updated is null ? null : ToDto(updated);
    }

    // Fetches the student first so we can confirm it exists before attempting a delete.
    // Returns false if not found (controller maps this to a 404).
    public async Task<bool> DeleteStudentAsync(int id)
    {
        var student = await _repo.GetStudentByIdAsync(id);
        if (student is null) return false;

        await _repo.DeleteStudentAsync(student);
        return true;
    }

    // Centralises the model-to-DTO mapping so each method doesn't repeat it.
    private static StudentResponseDto ToDto(Student s) => new()
    {
        StudentId = s.StudentId,
        Name = s.Name,
        Email = s.Email
    };
}
