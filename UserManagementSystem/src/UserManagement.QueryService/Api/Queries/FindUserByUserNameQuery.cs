using CQRS.Core.Queries;
using MediatR;
using UserManagement.QueryService.Domain.Entities;

namespace UserManagement.QueryService.Api.Queries;

public class FindUserByUserNameQuery : BaseQuery, IRequest<UserEntity?>
{
    public required string UserName { get; set; }
}