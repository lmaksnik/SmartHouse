using System;
using System.Collections.Generic;
using System.Reflection;

namespace SmartHouse.Server.Common {
	public static class Global {

		private static Assembly[] _allAssembly = null;
		public static Assembly[] AllAssemblies {
			get {
				if (_allAssembly == null) {
					var assemblies = new List<Assembly>();
					foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
						try {
							var types = assembly.GetTypes();
							if (types.Length > 0)
								assemblies.Add(assembly);
						} catch(Exception){ }
					}
					_allAssembly = assemblies.ToArray();
				}
				return _allAssembly;
			}
		}

	}
}
