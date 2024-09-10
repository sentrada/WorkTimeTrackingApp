using MediatR;
using UserManagement.QueryService.Api.Queries;
using UserManagement.QueryService.Domain.Entities;
using UserManagement.QueryService.Domain.Repositories;

namespace UserManagement.QueryService.Api.QueryHandlers;

public class FindUserByIdQueryHandler : IRequestHandler<FindUserByIdQuery, UserEntity?>
{
    private readonly IUserRepository _userRepository;

    public FindUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserEntity?> Handle(FindUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetByIdAsync(request.Id);
    }
}