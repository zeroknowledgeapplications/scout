using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;

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
					programs.Add (uid, new Program (){ Identifier = uid, });
			}

			foreach (var p in programs.Values)
				Debug.WriteLine (p.Identifier);

			return null;
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

