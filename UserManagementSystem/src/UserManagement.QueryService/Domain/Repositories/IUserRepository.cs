using UserManagement.QueryService.Domain.Entities;

namespace UserManagement.QueryService.Domain.Repositories;

public interface IUserRepository
{
    Task CreateAsync(UserEntity user);
    Task UpdateAsync(UserEntity user);
    Task DeleteAsync(Guid userId);
    Task<List<UserEntity>> ListAllAsync();
    Task<UserEntity?> GetByIdAsync(Guid userId);
    Task<UserEntity?> FindUserByEmailAsync(string email);
    Task<UserEntity?> FindUserByUserNameAsync(string userName);
}