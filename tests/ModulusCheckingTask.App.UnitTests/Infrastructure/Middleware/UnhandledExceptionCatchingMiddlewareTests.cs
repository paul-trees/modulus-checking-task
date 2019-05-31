using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ModulusCheckingTask.App.Infrastructure.Middleware;
using NSubstitute;
using Xunit;

namespace ModulusCheckingTask.App.UnitTests.Infrastructure.Middleware
{
    public class UnhandledExceptionCatchingMiddlewareTests
    {
        #region Fields

        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<UnhandledExceptionCatchingMiddleware> _logger;
        private readonly UnhandledExceptionCatchingMiddleware _sut;

        #endregion

        #region Constructor

        public UnhandledExceptionCatchingMiddlewareTests()
        {
            _requestDelegate = Substitute.For<RequestDelegate>();
            _logger = Substitute.For<ILogger<UnhandledExceptionCatchingMiddleware>>();

            _sut = new UnhandledExceptionCatchingMiddleware(_requestDelegate, _logger);
        }

        #endregion

        #region Tests

        [Fact]
        public async Task InvokeAsync_ExecutesRequestDelegate()
        {
            // Arrange
            var context = new DefaultHttpContext();

            // Act
            await _sut.InvokeAsync(context);

            // Assert
            await _requestDelegate.Received(1).Invoke(context);
        }

        [Fact]
        public async Task InvokeAsync_CatchesExceptionAfterExecutingRequestDelegate()
        {
            // Arrange
            const string exceptionMessage = "InvokeAsync_CatchesExceptionAfterExecutingRequestDelegate";
            var context = new DefaultHttpContext();
            _requestDelegate.When(a => a.Invoke(context)).Throw(new NotImplementedException(exceptionMessage));

            // Act
            await _sut.InvokeAsync(context);

            // Assert
            _logger.Received(1).Log(LogLevel.Error, Arg.Any<EventId>(), 
                Arg.Is<object>(o => o.ToString() == $"An unhandled exception occurred. Trace Identifier: {context.TraceIdentifier}"),
                Arg.Is<NotImplementedException>(e => e.Message == exceptionMessage), Arg.Any<Func<object, Exception, string>>());
            context.Response.StatusCode.Should().Be(500);
            context.Response.ContentType.Should().Be("application/json");
            context.Response.Body.Should().NotBeNull();
        }

        #endregion
    }
}
