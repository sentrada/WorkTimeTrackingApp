// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace UserManagement.CommandService.Api.Commands;

public class UpdateUserCommand : BaseCommand, IRequest
{
    public required string Name { get; set; } 
    public required string Email { get; set; }
}