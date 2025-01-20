using System;

using NLog;

namespace Summers.Wyvern.Common
{
	public class Loggable
	{
		private Logger _logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Writes the provided exception to the logger and throws it
		/// </summary>
		/// <typeparam name="TException">The type of the exception.</typeparam>
		/// <param name="message">The message.</param>
		protected void LogAndThrow<TException>(string message) where TException : Exception
		{
			_logger.Log(LogLevel.Error, message);
			throw (TException)Activator.CreateInstance(typeof(TException), message);
		}

		/// <summary>
		/// Writes the provided exception to the logger and throws it
		/// </summary>
		/// <typeparam name="TException">The type of the exception.</typeparam>
		/// <typeparam name="TInnerException">The type of the inner exception.</typeparam>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		protected void LogAndThrow<TException, TInnerException>(string message, TInnerException innerException) where TException : Exception where TInnerException : Exception
		{
			_logger.Log(LogLevel.Error, message);
			if (innerException != null)
				throw (TException)Activator.CreateInstance(typeof(TException), message, innerException);

			throw (TException)Activator.CreateInstance(typeof(TException), message);
		}

		/// <summary>
		/// Logs a message to the DEBUG logger
		/// </summary>
		/// <param name="message">The message.</param>
		protected void LogToDebug(string message)
		{
			_logger.Log(LogLevel.Debug, message);
		}

		/// <summary>
		/// Logs a message to the specified log level logger
		/// </summary>
		/// <param name="logLevel">The log level.</param>
		/// <param name="message">The message.</param>
		protected void Log(LogLevel logLevel, string message)
		{
			_logger.Log(logLevel, message);
		}
	}
}
