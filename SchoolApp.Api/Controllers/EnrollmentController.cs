using Microsoft.AspNetCore.Mvc;
using SchoolApp.Api.DTOs;
using SchoolApp.Api.Services;

namespace SchoolApp.Api.Controllers;

// [ApiController] enables automatic model validation and binding conventions.
// [Route] sets the base URL for all endpoints in this controller to /api/enrollments.
[ApiController]
[Route("api/[controller]")]
public class EnrollmentController : ControllerBase
{
    private readonly IEnrollmentService _service;

    // IEnrollmentService is injected by ASP.NET's DI container.
    public EnrollmentController(IEnrollmentService service)
    {
        _service = service;
    }

    // GET /api/enrollments
    // Returns all enrollments. Always succeeds with an empty list if there are none.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnrollmentResponseDto>>> GetAll()
    {
        var enrollments = await _service.GetAllEnrollmentsAsync();
        return Ok(enrollments);
    }

    // GET /api/enrollments/{studentId}/{courseId}
    // Returns a single enrollment, or 404 if not found.
    // Both IDs are required since enrollment has a composite key.
    [HttpGet("{studentId}/{courseId}")]
    public async Task<ActionResult<EnrollmentResponseDto>> GetById(int studentId, int courseId)
    {
        var enrollment = await _service.GetEnrollmentByIdAsync(studentId, courseId);
        if (enrollment is null) return NotFound();
        return Ok(enrollment);
    }

    // POST /api/enrollments
    // Creates a new enrollment from the request body.
    // Returns 201 Created with a Location header pointing to the new resource.
    [HttpPost]
    public async Task<ActionResult<EnrollmentResponseDto>> Create(EnrollmentRequestDto dto)
    {
        var created = await _service.CreateEnrollmentAsync(dto);
        return CreatedAtAction(nameof(GetById), new { studentId = created.StudentId, courseId = created.CourseId }, created);
    }

    // PUT /api/enrollments/{studentId}/{courseId}
    // Updates the grade for an existing enrollment. Returns 404 if the enrollment doesn't exist.
    [HttpPut("{studentId}/{courseId}")]
    public async Task<ActionResult<EnrollmentResponseDto>> Update(int studentId, int courseId, EnrollmentRequestDto dto)
    {
        var updated = await _service.UpdateEnrollmentAsync(studentId, courseId, dto);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    // DELETE /api/enrollments/{studentId}/{courseId}
    // Deletes an enrollment. Returns 204 No Content on success, 404 if not found.
    [HttpDelete("{studentId}/{courseId}")]
    public async Task<IActionResult> Delete(int studentId, int courseId)
    {
        var deleted = await _service.DeleteEnrollmentAsync(studentId, courseId);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
