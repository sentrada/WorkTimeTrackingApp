using Microsoft.EntityFrameworkCore;
using UserManagement.QueryService.Domain.Entities;
using UserManagement.QueryService.Domain.Repositories;
using UserManagement.QueryService.Infrastructure.DataAccess;

namespace UserManagement.QueryService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContextFactory _contextFactory;

    public UserRepository(DatabaseContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task CreateAsync(UserEntity user)
    {
        await using DatabaseContext context = _contextFactory.CreateDbContext();
        context.Users.Add(user);
        _ = await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserEntity user)
    {
        await using DatabaseContext context = _contextFactory.CreateDbContext();
        context.Users.Update(user);
        _ = await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid userId)
    {
        await using DatabaseContext context = _contextFactory.CreateDbContext();
        UserEntity? user = await GetByIdAsync(userId);
        if (user == null) return;

        context.Users.Remove(user);
        _ = await context.SaveChangesAsync();
    }

    public async Task<List<UserEntity>> ListAllAsync()
    {
        await using DatabaseContext context = _contextFactory.CreateDbContext();
        return await context.Users.AsNoTracking()
            .ToListAsync();
    }

    public async Task<UserEntity?> GetByIdAsync(Guid userId)
    {
        await using DatabaseContext context = _contextFactory.CreateDbContext();
        return await context.Users
            .FindAsync(userId);
    }

    public async Task<UserEntity?> FindUserByEmailAsync(string email)
    {
        await using DatabaseContext context = _contextFactory.CreateDbContext();
        return await context.Users
            .FirstOrDefaultAsync(e=>e.Email==email);
    }

    public async Task<UserEntity?> FindUserByUserNameAsync(string userName)
    {
        await using DatabaseContext context = _contextFactory.CreateDbContext();
        return await context.Users
            .FirstOrDefaultAsync(e=>e.UserName==userName);
    }
}
