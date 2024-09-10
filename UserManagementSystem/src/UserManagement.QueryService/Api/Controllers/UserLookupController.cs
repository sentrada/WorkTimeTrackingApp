using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Common.DTOs;
using UserManagement.QueryService.Api.DTOs;
using UserManagement.QueryService.Api.Queries;
using UserManagement.QueryService.Domain.Entities;

namespace UserManagement.QueryService.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserLookupController : ControllerBase
{
    private readonly ILogger<UserLookupController> _logger;
    private readonly IMediator _mediator;

    public UserLookupController(ILogger<UserLookupController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllPostsAsync()
    {
        try
        {
            List<UserEntity> users = await _mediator.Send(new FindAlUsersQuery());
            return NormalResponse(users);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve all users!";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        }
    }

    [HttpGet("byId/{userId}")]
    public async Task<ActionResult> GetByUserIdAsync(Guid userId)
    {
        try
        {
            UserEntity? user = await _mediator.Send(new FindUserByIdQuery { Id = userId });

            if (user == null)
                return NoContent();

            return Ok(new UserLookupResponse
            {
                Users = new List<UserEntity>{user},
                Message = "Successfully returned user!"
            });
        }
        catch (Exception ex)
        {
            const string safeErrorMessage = "Error while processing request to find user by ID!";
            return ErrorResponse(ex, safeErrorMessage);
        }
    }
    
    [HttpGet("byUserName/{userName}")]
    public async Task<ActionResult> GetByUserNameAsync(string userName)
    {
        try
        {
            UserEntity? user = await _mediator.Send(new FindUserByUserNameQuery() { UserName = userName});

            if (user == null)
                return NoContent();

            return Ok(new UserLookupResponse
            {
                Users = new List<UserEntity>{user},
                Message = "Successfully returned user!"
            });
        }
        catch (Exception ex)
        {
            const string safeErrorMessage = "Error while processing request to find user by UserName!";
            return ErrorResponse(ex, safeErrorMessage);
        }
    }
    
    [HttpGet("byEmail/{email}")]
    public async Task<ActionResult> GetByEmailAsync(string email)
    {
        try
        {
            UserEntity? user = await _mediator.Send(new FindUserByEmailQuery() { Email = email});

            if (user == null)
                return NoContent();

            return Ok(new UserLookupResponse
            {
                Users = new List<UserEntity>{user},
                Message = "Successfully returned user!"
            });
        }
        catch (Exception ex)
        {
            const string safeErrorMessage = "Error while processing request to find user by Email!";
            return ErrorResponse(ex, safeErrorMessage);
        }
    }
    
    private ActionResult NormalResponse(List<UserEntity>? users)
    {
        if (users == null || !users.Any())
            return NoContent();

        int count = users.Count;
        return Ok(new UserLookupResponse
        {
            Users = users,
            Message = $"Successfully returned {count} user{(count > 1 ? "s" : string.Empty)}!"
        });
    }
    
    private ActionResult ErrorResponse(Exception ex, string safeErrorMessage)
    {
        _logger.LogError(ex, safeErrorMessage);

        return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
        {
            Message = safeErrorMessage
        });
    }
}