using UserManagement.Common.Events;
using UserManagement.QueryService.Domain.Entities;
using UserManagement.QueryService.Domain.Repositories;

namespace UserManagement.QueryService.Infrastructure.Handlers;

public class UserUpdatedEventHandler : IEventHandler<UserUpdatedEvent>
{
    private readonly IUserRepository _userRepository;

    public UserUpdatedEventHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(UserUpdatedEvent @event)
    {
        UserEntity? user = await _userRepository.GetByIdAsync(@event.Id);

        if (user == null) return;

        user.UserName = @event.UserName;
        user.Email = @event.Email;
        await _userRepository.UpdateAsync(user);
    }
}