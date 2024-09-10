using UserManagement.Common.Events;
using UserManagement.QueryService.Domain.Entities;
using UserManagement.QueryService.Domain.Repositories;

namespace UserManagement.QueryService.Infrastructure.Handlers;

public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
{
    private readonly IUserRepository _userRepository;

    public UserCreatedEventHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(UserCreatedEvent @event)
    {
        UserEntity user = new()
        {
            UserId = @event.Id,
            UserName = @event.UserName,
            Email = @event.Email,
            Password = @event.Password
        };
        
        await _userRepository.CreateAsync(user);
    }
}