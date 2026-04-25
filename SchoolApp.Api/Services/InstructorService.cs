using SchoolApp.Api.Data;
using SchoolApp.Api.DTOs;
using SchoolApp.Api.Models;

namespace SchoolApp.Api.Services;

// InstructorService sits between the controller and the repo.
// It receives DTOs from the controller, maps them to/from models,
// and delegates the actual database work to IInstructorRepo.
public class InstructorService : IInstructorService
{
    private readonly IInstructorRepo _repo;

    // IInstructorRepo is injected by ASP.NET's DI container - registered in Program.cs
    public InstructorService(IInstructorRepo repo)
    {
        _repo = repo;
    }

    // Fetches all instructors and converts each model to a response DTO.
    public async Task<IEnumerable<InstructorResponseDto>> GetAllInstructorsAsync()
    {
        var instructors = await _repo.GetAllInstructorsAsync();
        return instructors.Select(ToDto);
    }

    // Fetches a single instructor by PK. Returns null if not found,
    // which the controller will translate to a 404.
    public async Task<InstructorResponseDto?> GetInstructorByIdAsync(int id)
    {
        var instructor = await _repo.GetInstructorByIdAsync(id);
        return instructor is null ? null : ToDto(instructor);
    }

    // Maps the incoming DTO to a new Instructor model, then hands it to the repo to insert.
    // The repo returns the saved entity with its DB-generated InstructorId populated.
    public async Task<InstructorResponseDto> CreateInstructorAsync(InstructorRequestDto dto)
    {
        var instructor = new Instructor
        {
            Name = dto.Name
        };

        var created = await _repo.AddInstructorAsync(instructor);
        return ToDto(created);
    }

    // Fetches the existing instructor, applies the new values from the DTO, then saves.
    // Returns null if no instructor with that ID exists.
    public async Task<InstructorResponseDto?> UpdateInstructorAsync(int id, InstructorRequestDto dto)
    {
        var instructor = await _repo.GetInstructorByIdAsync(id);
        if (instructor is null) return null;

        // Mutate the tracked entity directly - EF Core detects the changes automatically.
        instructor.Name = dto.Name;

        var updated = await _repo.UpdateInstructorAsync(instructor);
        return updated is null ? null : ToDto(updated);
    }

    // Fetches the instructor first so we can confirm it exists before attempting a delete.
    // Returns false if not found (controller maps this to a 404).
    public async Task<bool> DeleteInstructorAsync(int id)
    {
        var instructor = await _repo.GetInstructorByIdAsync(id);
        if (instructor is null) return false;

        await _repo.DeleteInstructorAsync(instructor);
        return true;
    }

    // Centralises the model-to-DTO mapping so each method doesn't repeat it.
    private static InstructorResponseDto ToDto(Instructor i) => new()
    {
        InstructorId = i.InstructorId,
        Name = i.Name
    };
}