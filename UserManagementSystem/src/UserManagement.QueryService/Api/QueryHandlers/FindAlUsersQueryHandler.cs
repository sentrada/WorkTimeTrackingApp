using MediatR;
using UserManagement.QueryService.Api.Queries;
using UserManagement.QueryService.Domain.Entities;
using UserManagement.QueryService.Domain.Repositories;

namespace UserManagement.QueryService.Api.QueryHandlers;

public class FindAlUsersQueryHandler : IRequestHandler<FindAlUsersQuery,List<UserEntity>>
{
    private readonly IUserRepository _userRepository;

    public FindAlUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserEntity>> Handle(FindAlUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.ListAllAsync();
    }
}