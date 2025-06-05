using Microsoft.Extensions.Logging;
using Moq;

namespace OrderProcessingApp.Tests.Helpers
{
    public static class LoggerExtensions
    {
        public static void VerifyLogging<T>(this Mock<T> loggerMock, LogLevel level, Times times)
            where T : class, ILogger
        {
            loggerMock.Verify(
                logger => logger.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<ArgumentException>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                times);
        }
    }
}
