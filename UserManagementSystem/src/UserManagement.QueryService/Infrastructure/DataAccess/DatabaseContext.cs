using Microsoft.EntityFrameworkCore;
using UserManagement.QueryService.Domain.Entities;

namespace UserManagement.QueryService.Infrastructure.DataAccess;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; }
}