using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SmartHouse.Server.Core.Logger {
	public class LoggerMemory : ILogger {

		protected internal readonly ICollection<Tuple<DateTime, string>> Logs = new List<Tuple<DateTime, string>>(); 
		protected readonly string Category;
		protected readonly IConfigurationSection Configuration;
		public LoggerMemory(IConfigurationSection configuration, string category) {
			Configuration = configuration;
			Category = category;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
			if(IsEnabled(logLevel))
				Logs.Add(new Tuple<DateTime, string>(DateTime.Now, $"{logLevel} [{eventId.Id}:{eventId.Name}] - {formatter.Invoke(state, exception)}"));
		}

		public bool IsEnabled(LogLevel logLevel) {
			var configLogLevel = GetConfiguredLogLevel(Configuration, Category);
			return (int)logLevel >= (int)configLogLevel;
		}

		public IDisposable BeginScope<TState>(TState state) {
			return null;
		}

		protected LogLevel GetConfiguredLogLevel(IConfigurationSection configuration, string categoryName) {
			var logLevelSection = configuration.GetSection("LogLevel");
			string logLevelString = null;
			if(!string.IsNullOrWhiteSpace(categoryName))
				logLevelString = logLevelSection[categoryName];
			if(string.IsNullOrWhiteSpace(logLevelString))
				logLevelString = logLevelSection["Default"];
			if(Enum.TryParse(logLevelString, out LogLevel result))
				return result;
			return default(LogLevel);
		}
	}
}
