using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.QueryService.Domain.Entities;

[Table("User")]
public class UserEntity
{
    [Key]
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    // public bool IsActive { get; set; }
    // public DateTime CreatedAt { get; set; }
    // public DateTime UpdatedAt { get; set; }
    // public DateTime? DeletedAt { get; set; }
}
