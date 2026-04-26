using Microsoft.AspNetCore.Mvc;
using SchoolApp.Api.DTOs;
using SchoolApp.Api.Services;

namespace SchoolApp.Api.Controllers;

// [ApiController] enables automatic model validation and binding conventions.
// [Route] sets the base URL for all endpoints in this controller to /api/instructors.
[ApiController]
[Route("api/[controller]")]
public class InstructorController : ControllerBase
{
    private readonly IInstructorService _service;

    // IInstructorService is injected by ASP.NET's DI container.
    public InstructorController(IInstructorService service)
    {
        _service = service;
    }

    // GET /api/instructors
    // Returns all instructors. Always succeeds with an empty list if there are none.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InstructorResponseDto>>> GetAll()
    {
        var instructors = await _service.GetAllInstructorsAsync();
        return Ok(instructors);
    }

    // GET /api/instructors/{id}
    // Returns a single instructor, or 404 if not found.
    [HttpGet("{id}")]
    public async Task<ActionResult<InstructorResponseDto>> GetById(int id)
    {
        var instructor = await _service.GetInstructorByIdAsync(id);
        if (instructor is null) return NotFound();
        return Ok(instructor);
    }

    // POST /api/instructors
    // Creates a new instructor from the request body.
    // Returns 201 Created with a Location header pointing to the new resource.
    [HttpPost]
    public async Task<ActionResult<InstructorResponseDto>> Create(InstructorRequestDto dto)
    {
        var created = await _service.CreateInstructorAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.InstructorId }, created);
    }

    // PUT /api/instructors/{id}
    // Replaces an existing instructor's data. Returns 404 if the instructor doesn't exist.
    [HttpPut("{id}")]
    public async Task<ActionResult<InstructorResponseDto>> Update(int id, InstructorRequestDto dto)
    {
        var updated = await _service.UpdateInstructorAsync(id, dto);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    // DELETE /api/instructors/{id}
    // Deletes an instructor. Returns 204 No Content on success, 404 if not found.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteInstructorAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}