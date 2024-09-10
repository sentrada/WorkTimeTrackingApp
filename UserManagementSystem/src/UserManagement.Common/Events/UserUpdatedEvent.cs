using CQRS.Core.Events;

namespace UserManagement.Common.Events;

public class UserUpdatedEvent() : BaseEvent(nameof(UserUpdatedEvent))
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
}