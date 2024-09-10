using CQRS.Core.Events;

namespace UserManagement.Common.Events;

public class UserDeletedEvent() : BaseEvent(nameof(UserDeletedEvent))
{
}
