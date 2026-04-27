using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Api.DTOs;

// Used as the request body for POST and PUT endpoints
// Only contains fields the client is allowed to set - no ID, no enrollments.
public class StudentRequestDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
