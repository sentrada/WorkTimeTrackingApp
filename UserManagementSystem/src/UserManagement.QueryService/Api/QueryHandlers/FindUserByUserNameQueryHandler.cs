using MediatR;
using UserManagement.QueryService.Api.Queries;
using UserManagement.QueryService.Domain.Entities;
using UserManagement.QueryService.Domain.Repositories;

namespace UserManagement.QueryService.Api.QueryHandlers;

public class FindUserByUserNameQueryHandler : IRequestHandler<FindUserByUserNameQuery, UserEntity?>
{
    private readonly IUserRepository _userRepository;

    public FindUserByUserNameQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserEntity?> Handle(FindUserByUserNameQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.FindUserByUserNameAsync(request.UserName);
    }
}