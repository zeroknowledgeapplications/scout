using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace ZeroKnowledge
{
	public class ConnectionController
	{
		public static List<Connection> GetConnections()
		{
			Dictionary<string, Program> programs = new Dictionary<string, Program> ();

			var plist = Process.GetProcesses ();

			foreach (var p in plist) {
				var uid = GetUser (p.Id);

				if (uid == null)
					continue;

				if (!programs.ContainsKey (uid))
					programs.Add (uid, new Program (){ Identifier = uid, ProcessId = p.Id });
			}

			foreach(var p in plist)
				foreach (var parts in GetConnectionData(p.Id)) {
					Debug.WriteLine (String.Join(" ", parts));
				}

			return null;
		}

		private static IEnumerable<string[]> GetConnectionData()
		{
			/*
			ProcessStartInfo pinfo = new ProcessStartInfo("/system/bin/netstat");
			pinfo.RedirectStandardOutput = true;
			pinfo.UseShellExecute = false;
			var proc = Process.Start(pinfo);

			proc.WaitForExit();
			var lines = proc.StandardOutput;
			while (!lines.EndOfStream) {
				var line = lines.ReadLine ();
				var parts = Regex.Split (line, @"\s+");
				yield return parts;
			}*/
		}

		public static string GetUser(int pid)
		{
			FileInfo f = new FileInfo (String.Format ("/proc/{0}/cmdline", pid));
			if (!f.Exists)
				return null;

			var reader = new StreamReader (f.OpenRead ());
			var uid = reader.ReadLine ();//.Split ('/') [1];

			// Filter system processes
			if (uid != null && uid.Contains ("/"))
				uid = null;

			return uid;
		}
	}
}

