using CQRS.Core.Handlers;
using UserManagement.CommandService.Api.Commands;
using UserManagement.CommandService.Domain.Aggregates;

namespace UserManagement.CommandService.Api.CommandHandlers;

public class CreateUserCommandHandler(IEventSourcingHandler<UserAggregate> eventSourcingHandler)
    : IRequestHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        UserAggregate aggregate = new(request.Id, request.Name, request.Email,request.Password);
        await eventSourcingHandler.SaveAsync(aggregate);
        return aggregate.Id;
    }
}