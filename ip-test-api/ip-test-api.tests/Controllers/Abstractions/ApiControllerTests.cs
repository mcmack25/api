using FluentResults;
using ip_test_api.Controllers.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ip_test_api.tests.Controllers.Abstractions;

[TestFixture]
public class ApiControllerTests: ApiController
{
    [Test]
    public void GetHandledResult_ReturnsOk_WhenResultIsSuccess()
    {
        // Arrange
        var expectedResult = Result.Ok("Test");

        // Act
        var result = GetHandledResult(expectedResult);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(((OkObjectResult)result).Value, Is.EqualTo(expectedResult.Value));
        });
    }

    [Test]
    public void GetHandledResult_ReturnsProblem_WhenResultIsFailed()
    {
        // Arrange
        const string message = "Error message";
        var expectedResult = Result.Fail(message);

        // Act
        var result = GetHandledResult(expectedResult);

        // Assert
        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = (ObjectResult)result;
        Assert.Multiple(() =>
        {
            Assert.That(objectResult.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(((ProblemDetails)objectResult.Value!).Title, Is.EqualTo(message));
        });
    }

    [Test]
    public void GetHandledResult_ReturnsInternalServerError_WhenResultIsNeitherSuccessNorFailed()
    {
        // Arrange
        // Act
        var result = GetHandledResult(null!);

        // Assert
        Assert.That(result, Is.InstanceOf<StatusCodeResult>());
        var statusCodeResult = (StatusCodeResult)result;
        Assert.That(statusCodeResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }
}
