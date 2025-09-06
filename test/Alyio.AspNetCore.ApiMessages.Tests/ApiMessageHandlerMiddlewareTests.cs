// MIT License

using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Alyio.AspNetCore.ApiMessages.Tests;

public class ApiMessageHandlerMiddlewareTests
{
    [Theory]
    [InlineData("/400", HttpStatusCode.BadRequest, StatusCodeTypes.Status400BadRequest)]
    [InlineData("/401", HttpStatusCode.Unauthorized, StatusCodeTypes.Status401Unauthorized)]
    [InlineData("/403", HttpStatusCode.Forbidden, StatusCodeTypes.Status403Forbidden)]
    [InlineData("/404", HttpStatusCode.NotFound, StatusCodeTypes.Status404NotFound)]
    [InlineData("/500", HttpStatusCode.InternalServerError, StatusCodeTypes.Status500InternalServerError)]
    public async Task Middleware_WhenApiMessageExceptionIsThrown_ReturnsCorrectProblemDetails(string path, HttpStatusCode expectedStatusCode, string expectedType)
    {
        // Arrange
        var builder = CreateWebHostBuilder(app =>
        {
            app.Map(path, x => x.Run(_ => throw GetExceptionForPath(path)));
        });
        var testServer = new TestServer(builder);

        // Act
        var response = await testServer.CreateRequest(path).GetAsync();

        // Assert
        Assert.Equal(expectedStatusCode, response.StatusCode);
        var details = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(details);
        Assert.Equal(expectedType, details.Type);
    }

    [Fact]
    public async Task Middleware_WhenUnhandledExceptionIsThrown_Returns500InternalServerError()
    {
        // Arrange
        var builder = CreateWebHostBuilder(app =>
        {
            app.Run(_ => throw new InvalidOperationException("This is an unhandled exception."));
        });
        var testServer = new TestServer(builder);

        // Act
        var response = await testServer.CreateRequest("/").GetAsync();

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var details = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(details);
        Assert.Equal(StatusCodeTypes.Status500InternalServerError, details.Type);
    }

    [Fact]
    public async Task Middleware_WhenNoExceptionIsThrown_DoesNotInterfere()
    {
        // Arrange
        var builder = CreateWebHostBuilder(app =>
        {
            app.Map("/201", x =>
            {
                x.Run(h =>
                {
                    h.Response.StatusCode = StatusCodes.Status201Created;
                    return h.Response.WriteAsJsonAsync(new CreatedMessage { Id = "9527" });
                });
            });
        });
        var testServer = new TestServer(builder);

        // Act
        var response = await testServer.CreateRequest("/201").GetAsync();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var message = await response.Content.ReadFromJsonAsync<CreatedMessage>();
        Assert.NotNull(message);
        Assert.Equal("9527", message.Id);
    }

    [Fact]
    public async Task Middleware_WhenResponseHasAlreadyStarted_RethrowsException()
    {
        // Arrange
        var builder = CreateWebHostBuilder(app =>
        {
            // This middleware starts the response, so the ApiMessageHandler should not be able to write to it.
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Response started...");
                await next();
            });
            app.Run(_ => throw new BadRequestException("This should be re-thrown."));
        });
        var testServer = new TestServer(builder);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => testServer.CreateRequest("/").GetAsync());
    }

    private static IWebHostBuilder CreateWebHostBuilder(Action<IApplicationBuilder> configureApp)
    {
        return new WebHostBuilder()
            .ConfigureServices((_, services) =>
            {
#if NET8_0_OR_GREATER
                services.AddExceptionHandler<InternalServerErrorExceptionHandler>();
#endif
            })
            .Configure(app =>
            {
#if NET8_0_OR_GREATER
                app.UseExceptionHandler("/Error");
#else
                app.UseExceptionHandler(new ExceptionHandlerOptions { ExceptionHandler = ExceptionHandler.WriteUnhandledMessageAsync });
#endif
                app.UseApiMessage();
                configureApp(app);
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
