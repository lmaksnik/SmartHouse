using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SmartHouse.Server.Core.Logger {
	public class LoggerProviderFile : ILoggerProvider {

		protected readonly IConfigurationSection Configuration;
		private readonly Timer _timer;
		protected readonly string Directory;
		protected ConcurrentDictionary<string, LoggerMemory> Loggers = new ConcurrentDictionary<string, LoggerMemory>();

		public LoggerProviderFile(IConfigurationSection configuration, string directory, int delay = 3000) {
			Configuration = configuration;
			Directory = directory;
			_timer = new Timer(delay);
			_timer.Elapsed += TimerOnElapsed;
			_timer.Start();
		}

		public ILogger CreateLogger(string categoryName) {
			var logger = new LoggerMemory(Configuration, categoryName);
			lock (Loggers) {
				Loggers.AddOrUpdate(categoryName, logger, (s, memory) => memory);
			}
			return logger;
		}

		private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs) {
			_timer.Enabled = false;
			try {
				List<Tuple<DateTime, string, LoggerMemory>> logs = new List<Tuple<DateTime, string, LoggerMemory>>();
				lock (Loggers) {
					foreach (var logger in Loggers) {
						lock (logger.Value.Logs) {
							foreach (var tuple in logger.Value.Logs) {
								logs.Add(new Tuple<DateTime, string, LoggerMemory>(tuple.Item1, tuple.Item2, logger.Value));
							}
							logger.Value.Logs.Clear();
						}
					}
				}
				if (!logs.Any())
					return;
				logs = logs.OrderBy(a => a.Item1).ToList();
				var dates = logs.Select(a => a.Item1.Date).ToList();
				foreach (var date in dates) {
					var logsByDate = logs.Where(a => a.Item1.Date == date).ToList();

					using (var writer = new StreamWriter(Path.Combine(Directory, $"{date.ToShortDateString()}.log"), true)) {
						foreach (var log in logsByDate) {
							writer.WriteLine($"{log.Item1:HH:mm:ss} {log.Item2}");
						}
					}
				}
			} finally {
				_timer.Enabled = true;
			}
		}

		public void Dispose() {
			Loggers.Clear();
			_timer.Dispose();
		}
	}
}
