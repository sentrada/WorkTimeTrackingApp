using UserManagement.Common.Events;
using UserManagement.QueryService.Domain.Repositories;

namespace UserManagement.QueryService.Infrastructure.Handlers;

public class UserDeletedEventHandler : IEventHandler<UserDeletedEvent>
{
    private readonly IUserRepository _userRepository;

    public UserDeletedEventHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task HandleAsync(UserDeletedEvent @event)
    {
        return _userRepository.DeleteAsync(@event.Id);
    }
}