using Microsoft.AspNetCore.Mvc;
using UserManagement.CommandService.Api.Commands;

namespace UserManagement.CommandService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreateUserController : Controller
{
    private readonly ILogger<CreateUserCommand> _logger;
    private readonly IMediator _mediator;

    public CreateUserController(ILogger<CreateUserCommand> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]  
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(CreateUserCommand command)
    {
        
        try
        {
            Guid id = Guid.NewGuid();
            command.Id = id;
            await _mediator.Send(command);

            return StatusCode(StatusCodes.Status201Created, new
            {
                Id = id,
                Message = "User creation request completed successfully!"
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Client made a bad request!");
            return BadRequest(new
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            const string safeErrorMessage = "Error while processing request to create a new user!";
            _logger.Log(LogLevel.Error, ex, safeErrorMessage);

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Message = safeErrorMessage
            });
        }
    }
}