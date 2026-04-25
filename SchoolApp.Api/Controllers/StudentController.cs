using Microsoft.AspNetCore.Mvc;
using SchoolApp.Api.DTOs;
using SchoolApp.Api.Services;

namespace SchoolApp.Api.Controllers;

// [ApiController] enables automatic model validation and binding conventions.
// [Route] sets the base URL for all endpoints in this controller to /api/students.
[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _service;

    // IStudentService is injected by ASP.NET's DI container.
    public StudentController(IStudentService service)
    {
        _service = service;
    }

    // GET /api/students
    // Returns all students. Always succeeds with an empty list if there are none.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentResponseDto>>> GetAll()
    {
        var students = await _service.GetAllStudentsAsync();
        return Ok(students);
    }

    // GET /api/students/{id}
    // Returns a single student, or 404 if not found.
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentResponseDto>> GetById(int id)
    {
        var student = await _service.GetStudentByIdAsync(id);
        if (student is null) return NotFound();
        return Ok(student);
    }

    // POST /api/students
    // Creates a new student from the request body.
    // Returns 201 Created with a Location header pointing to the new resource.
    [HttpPost]
    public async Task<ActionResult<StudentResponseDto>> Create(StudentRequestDto dto)
    {
        var created = await _service.CreateStudentAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.StudentId }, created);
    }

    // PUT /api/students/{id}
    // Replaces an existing student's data. Returns 404 if the student doesn't exist.
    [HttpPut("{id}")]
    public async Task<ActionResult<StudentResponseDto>> Update(int id, StudentRequestDto dto)
    {
        var updated = await _service.UpdateStudentAsync(id, dto);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    // DELETE /api/students/{id}
    // Deletes a student. Returns 204 No Content on success, 404 if not found.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteStudentAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
