using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Api.DTOs;

// Used as the request body for POST and PUT endpoints.
public class InstructorRequestDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
