using MediatR;
using UserManagement.QueryService.Api.Queries;
using UserManagement.QueryService.Domain.Entities;
using UserManagement.QueryService.Domain.Repositories;

namespace UserManagement.QueryService.Api.QueryHandlers;

public class FindUserByEmailQueryHandler : IRequestHandler<FindUserByEmailQuery, UserEntity?>
{
    private readonly IUserRepository _userRepository;

    public FindUserByEmailQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserEntity?> Handle(FindUserByEmailQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.FindUserByEmailAsync(request.Email);
    }
}