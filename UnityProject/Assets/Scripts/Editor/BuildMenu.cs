using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;

using Debug = UnityEngine.Debug;

public static class BuildMenu {
	[MenuItem("Build/Configuration/Debug")]
	public static void Configuration_Debug() {
		ChangeConfiguration("Debug");
	}
	
	[MenuItem("Build/Configuration/Release")]
	public static void Configuration_Release() {
		ChangeConfiguration("Release");
	}
	
	public static void ChangeConfiguration(string name) {
		var curDir = Directory.GetCurrentDirectory();
		var parent = Directory.GetParent(curDir);
		var path = Path.Combine(parent.FullName, "UnityAspectInjectorSample");

		if ( Run($"clean \"{path}\"") && Run($"build \"{path}\" -c {name}") ) {
			AssetDatabase.Refresh();
		}
	}

	static bool Run(string command) {
		// Hack, because %PATH% inside Unity is different
		var oldPath = System.Environment.GetEnvironmentVariable("PATH");
		System.Environment.SetEnvironmentVariable("PATH", oldPath + ":/usr/local/share/dotnet/");
		
		var startInfo = new ProcessStartInfo {
			FileName               = "dotnet",
			Arguments              = command,
			RedirectStandardError  = true,
			RedirectStandardOutput = true,
			UseShellExecute        = false
		};
		
		try {
			using ( var process = Process.Start(startInfo) ) {
				process.WaitForExit();
				var output = process.StandardOutput.ReadToEnd() + "\n" + process.StandardError.ReadToEnd();
				if ( process.ExitCode == 0 ) {
					Debug.Log($"Success: '{command}'\n{output}");
					return true;
				} else {
					Debug.LogError($"Failed: '{command}'\n{output}");
				}
			}
		} catch ( Exception e ) {
			Debug.LogError($"Failed: '{command}': {e}");
		}
		return false;
	}
}
