using Microsoft.AspNetCore.Mvc;
using UserManagement.CommandService.Api.Commands;

namespace UserManagement.CommandService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeleteUserController : Controller
{
    private readonly ILogger<DeleteUserCommand> _logger;
    private readonly IMediator _mediator;

    public DeleteUserController(ILogger<DeleteUserCommand> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id, DeleteUserCommand command)
    {
        try
        {
            command.Id = id;
            await _mediator.Send(command);

            return StatusCode(StatusCodes.Status200OK, new
            {
                Message = "User deletion request completed successfully!"
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
            const string safeErrorMessage = "Error while processing request to delete a user!";
            _logger.Log(LogLevel.Error, ex, safeErrorMessage);

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Message = safeErrorMessage
            });
        }
    }
}