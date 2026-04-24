namespace SchoolApp.Api.DTOs;

// Used as the request body for POST and PUT endpoints
// Only contains fields the client is allowed to set - no ID, no enrollments.
public class StudentRequestDto
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}