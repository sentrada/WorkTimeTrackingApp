// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace UserManagement.CommandService.Api.Commands;

public class CreateUserCommand : BaseCommand, IRequest<Guid>
{
    public required string Name { get; set; } 
    public required string Email { get; set; } 
    public required string Password { get; set; } 
}