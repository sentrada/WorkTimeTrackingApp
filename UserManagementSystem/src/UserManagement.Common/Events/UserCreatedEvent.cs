using CQRS.Core.Events;

namespace UserManagement.Common.Events;

public class UserCreatedEvent : BaseEvent
{
    public UserCreatedEvent() : base(nameof(UserCreatedEvent))
    {
    }
    
    public required string UserName { get; set; } 
    public required string Email { get; set; }
    public required string Password { get; set; }
}