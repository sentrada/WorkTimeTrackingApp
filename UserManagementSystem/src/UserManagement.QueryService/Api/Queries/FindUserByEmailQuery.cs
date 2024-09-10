using CQRS.Core.Queries;
using MediatR;
using UserManagement.QueryService.Domain.Entities;

namespace UserManagement.QueryService.Api.Queries;

public class FindUserByEmailQuery : BaseQuery, IRequest<UserEntity?>
{
    public required string Email { get; set; }
}