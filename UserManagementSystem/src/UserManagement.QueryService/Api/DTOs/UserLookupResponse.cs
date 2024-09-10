using UserManagement.Common.DTOs;
using UserManagement.QueryService.Domain.Entities;

namespace UserManagement.QueryService.Api.DTOs;

public class UserLookupResponse : BaseResponse
{
    public required List<UserEntity>? Users { get; set; }
}