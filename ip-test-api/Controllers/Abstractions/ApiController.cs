using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ip_test_api.Controllers.Abstractions;

[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult GetHandledResult<TResult>(
        Result<TResult> result)
    {
        return result switch
        {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result.Value),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    protected IActionResult GetHandledResult(
        Result result)
    {
        return result switch
        {
            { IsFailed: true } => Problem(result.Errors),
            { IsSuccess: true } => Ok(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    private IActionResult Problem(List<IError> errors)
    {
        if (errors.Count is 0)
        {
            return Problem();
        }

        return Problem(statusCode: (int)HttpStatusCode.BadRequest, title: errors[0].Message);
    }
}