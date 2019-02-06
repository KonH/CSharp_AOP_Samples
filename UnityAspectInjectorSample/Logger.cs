using System;

namespace UnityAspectInjectorSample {
	public static class Logger {
		static Action<string> _callback;

		public static void Initialize(Action<string> callback) {
			_callback = callback;
		}

		public static void Log(string message) {
			_callback?.Invoke(message);
		}
	}
}
