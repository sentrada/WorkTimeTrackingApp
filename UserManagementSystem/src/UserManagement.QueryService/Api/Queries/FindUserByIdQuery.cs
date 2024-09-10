using CQRS.Core.Queries;
using MediatR;
using UserManagement.QueryService.Domain.Entities;

namespace UserManagement.QueryService.Api.Queries;

public class FindUserByIdQuery : BaseQuery, IRequest<UserEntity?>
{
    public Guid Id { get; set; }
}