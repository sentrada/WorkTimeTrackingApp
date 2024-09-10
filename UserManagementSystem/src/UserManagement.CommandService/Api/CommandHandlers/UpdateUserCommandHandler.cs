using CQRS.Core.Handlers;
using UserManagement.CommandService.Api.Commands;
using UserManagement.CommandService.Domain.Aggregates;

namespace UserManagement.CommandService.Api.CommandHandlers;

public class UpdateUserCommandHandler(IEventSourcingHandler<UserAggregate> eventSourcingHandler) : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        UserAggregate aggregate = await eventSourcingHandler.GetByIdAsync(request.Id);
        aggregate.UpdateUser(request.Name, request.Email);
        await eventSourcingHandler.SaveAsync(aggregate);
    }
}