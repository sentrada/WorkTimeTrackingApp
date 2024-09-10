using CQRS.Core.Handlers;
using UserManagement.CommandService.Api.Commands;
using UserManagement.CommandService.Domain.Aggregates;

namespace UserManagement.CommandService.Api.CommandHandlers;

public class DeleteUserCommandHandler(IEventSourcingHandler<UserAggregate> eventSourcingHandler) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        UserAggregate aggregate = await eventSourcingHandler.GetByIdAsync(request.Id);
        aggregate.DeleteUser();
        await eventSourcingHandler.SaveAsync(aggregate);
    }
}