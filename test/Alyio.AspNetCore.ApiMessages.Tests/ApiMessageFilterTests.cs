// MIT License

#if NET8_0_OR_GREATER

using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Alyio.AspNetCore.ApiMessages.Extensions;
using Xunit;

namespace Alyio.AspNetCore.ApiMessages.Tests;

public class ApiMessageFilterTests
{
    [Theory]
    [InlineData("/400", HttpStatusCode.BadRequest, StatusCodeTypes.Status400BadRequest)]
    [InlineData("/401", HttpStatusCode.Unauthorized, StatusCodeTypes.Status401Unauthorized)]
    [InlineData("/403", HttpStatusCode.Forbidden, StatusCodeTypes.Status403Forbidden)]
    [InlineData("/404", HttpStatusCode.NotFound, StatusCodeTypes.Status404NotFound)]
    [InlineData("/500", HttpStatusCode.InternalServerError, StatusCodeTypes.Status500InternalServerError)]
    public async Task Filter_WhenApiMessageExceptionIsThrown_ReturnsCorrectProblemDetails(string path, HttpStatusCode expectedStatusCode, string expectedType)
    {
        // Arrange
        var builder = CreateWebHostBuilder(endpoints =>
        {
            endpoints.MapGet(path, () => { throw GetExceptionForPath(path); }).AddApiMessage();
        });
        var testServer = new TestServer(builder);

        // Act
        var response = await testServer.CreateRequest(path).GetAsync();

        // Assert
        Assert.Equal(expectedStatusCode, response.StatusCode);
        var details = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(details);
        Assert.Equal((int)expectedStatusCode, details.Status);
        Assert.Equal(expectedType, details.Type);
    }

    private static IWebHostBuilder CreateWebHostBuilder(Action<IEndpointRouteBuilder> configureEndpoints)
    {
        return new WebHostBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddRouting();
                services.AddApiMessages();
            })
            .Configure(app =>
            {
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    configureEndpoints(endpoints);
                });
            });
    }

    private static Exception GetExceptionForPath(string path) => path switch
    {
        "/400" => new BadRequestException(XMessage.ValidationFailed),
        "/401" => new UnauthorizedException(),
        "/403" => new ForbiddenException(),
        "/404" => new NotFoundException(),
        "/500" => new InternalServerErrorException(),
        _ => new InvalidOperationException("Should not happen in this test.")
    };
}

#endif