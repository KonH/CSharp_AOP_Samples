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
		var outPath = $"{curDir}/Assets/Plugins/UnityAspectInjectorSample/netstandard2.0";

		if ( Run($"clean \"{path}\" -o \"{outPath}\"") && Run($"build \"{path}\" -c {name} -o \"{outPath}\"") ) {
			AssetDatabase.Refresh();
		}
	}

	static bool Run(string command) {
		// Hack, because %PATH% inside Unity may be different on MacOS
		var oldPath = Environment.GetEnvironmentVariable("PATH");
		Environment.SetEnvironmentVariable("PATH", oldPath + ":/usr/local/share/dotnet/");
		
		var startInfo = new ProcessStartInfo {
			FileName               = "dotnet",
			Arguments              = command,
			RedirectStandardError  = true,
			RedirectStandardOutput = true,
			UseShellExecute        = false,
			CreateNoWindow         = true
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
