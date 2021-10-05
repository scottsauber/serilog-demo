using System;
using Microsoft.Extensions.Logging;

namespace SerilogDemo.Infrastructure
{
    public interface ILoggerService<T>
    {
        void LogInformation(string message, params object[] args);
        void LogError(Exception ex, string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogDebug(string message, params object[] args);
    }
    
    public class LoggerService<T> : ILoggerService<T>
    {
        private readonly ILogger<T> _logger;
 
        public LoggerService(ILogger<T> logger)
        {
            _logger = logger;
        }
 
        public void LogError(Exception ex, string message, params object[] args)
        {
            _logger.LogError(ex, message, args);
        }
 
        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void LogDebug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);
        }
    }
}