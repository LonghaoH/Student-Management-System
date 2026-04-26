using Microsoft.AspNetCore.Mvc;
using SchoolApp.Api.DTOs;
using SchoolApp.Api.Services;

namespace SchoolApp.Api.Controllers;

// [ApiController] enables automatic model validation and binding conventions.
// [Route] sets the base URL for all endpoints in this controller to /api/courses.
[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly ICourseService _service;

    // ICourseService is injected by ASP.NET's DI container.
    public CourseController(ICourseService service)
    {
        _service = service;
    }

    // GET /api/courses
    // Returns all courses. Always succeeds with an empty list if there are none.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseResponseDto>>> GetAll()
    {
        var courses = await _service.GetAllCoursesAsync();
        return Ok(courses);
    }

    // GET /api/courses/{id}
    // Returns a single course, or 404 if not found.
    [HttpGet("{id}")]
    public async Task<ActionResult<CourseResponseDto>> GetById(int id)
    {
        var course = await _service.GetCourseByIdAsync(id);
        if (course is null) return NotFound();
        return Ok(course);
    }

    // POST /api/courses
    // Creates a new course from the request body.
    // Returns 201 Created with a Location header pointing to the new resource.
    [HttpPost]
    public async Task<ActionResult<CourseResponseDto>> Create(CourseRequestDto dto)
    {
        var created = await _service.CreateCourseAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.CourseId }, created);
    }

    // PUT /api/courses/{id}
    // Replaces an existing course's data. Returns 404 if the course doesn't exist.
    [HttpPut("{id}")]
    public async Task<ActionResult<CourseResponseDto>> Update(int id, CourseRequestDto dto)
    {
        var updated = await _service.UpdateCourseAsync(id, dto);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    // DELETE /api/courses/{id}
    // Deletes a course. Returns 204 No Content on success, 404 if not found.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteCourseAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
