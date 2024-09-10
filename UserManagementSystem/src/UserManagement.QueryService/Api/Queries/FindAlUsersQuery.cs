using CQRS.Core.Queries;
using MediatR;
using UserManagement.QueryService.Domain.Entities;

namespace UserManagement.QueryService.Api.Queries;

public class FindAlUsersQuery : BaseQuery, IRequest<List<UserEntity>>
{
}