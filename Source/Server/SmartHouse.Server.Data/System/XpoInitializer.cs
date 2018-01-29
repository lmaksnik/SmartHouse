using System.Reflection;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;

namespace SmartHouse.Server.Data.System {
	/// <summary>
	/// Данный класс предназначен для иниициализация подключения к базе данных
	/// </summary>
	public class XpoInitializer {

		/// <summary>
		/// Строка подключения к базе данных
		/// </summary>
		public readonly string ConnectionString;

		/// <summary>
		/// Коллекция сборок с моделью данных
		/// </summary>
		public readonly Assembly[] Assemblies;

		public readonly AutoCreateOption AutoCreateOption;

		public bool Initialized { get; private set; }

		private readonly object _lockObject = new object();

		public XpoInitializer(string connectionString, AutoCreateOption autoCreateOption, params Assembly[] assemblies) {
			ConnectionString = connectionString;
			Assemblies = assemblies;
			AutoCreateOption = autoCreateOption;
		}

		/// <summary>
		/// Инициализация подключения
		/// </summary>
		public void Init() {
			lock (_lockObject) {
				if (Initialized) return;

				var dataStore = XpoDefault.GetConnectionProvider(ConnectionString, AutoCreateOption);
				var dictionary = new ReflectionDictionary();
				var dataLayer = new ThreadSafeDataLayer(dictionary, dataStore, Assemblies);

				XpoDefault.Dictionary = dataLayer.Dictionary;
				XpoDefault.DataLayer = dataLayer;
				XpoDefault.DataLayer.UpdateSchema(false);
				XpoDefault.Session = new UnitOfWork(dataLayer);

				Initialized = true;
			}
		}
	}
}
