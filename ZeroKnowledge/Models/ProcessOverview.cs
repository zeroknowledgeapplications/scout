using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Android.Content.PM;

namespace ZeroKnowledge
{
	public class ProcessOverview
	{
		Dictionary<int, string> _processUserMapping = new Dictionary<int, string>();
		Dictionary<string, Program> _programIdentifierMapping = new Dictionary<string, Program>();
		PackageManager _manager;

		public ProcessOverview(PackageManager manager)
		{
			_manager = manager;
		}

		public static string GetIdentifier(int pid)
		{
			FileInfo f = new FileInfo (String.Format ("/proc/{0}/cmdline", pid));
			if (!f.Exists)
				return null;

			var reader = new StreamReader (f.OpenRead ());
			var uid = reader.ReadLine ();

			// Filter system processes
			if (uid != null && uid.Contains ("/"))
				uid = null;

			Regex r = new Regex (@"[^a-zA-Z0-9.-]");

			if(uid != null)
				uid = r.Replace	(uid, "");

			return uid;
		}

		public void Update()
		{
			ProcessStartInfo s = new ProcessStartInfo ("/system/bin/ps");
			s.UseShellExecute = false;
			s.RedirectStandardOutput = true;

			var ps = Process.Start (s);
			ps.WaitForExit ();
			_processUserMapping.Clear ();

			ps.StandardOutput.ReadLine (); // Skip header
			while (!ps.StandardOutput.EndOfStream) {
				var line = ps.StandardOutput.ReadLine ();
				var parts = Regex.Split (line, @"\s+");

				_processUserMapping.Add (int.Parse (parts [1]), parts [0]);
			}

			var plist = Process.GetProcesses ();

			foreach (var p in plist) {
				var uid = GetIdentifier (p.Id);

				if (uid == null)
					continue;

				if (!_programIdentifierMapping.ContainsKey (uid)) {
					var program = new Program (){ Identifier = uid, ProcessId = p.Id, Name = NameFromIdentifier(uid) };
					program.UserId = GetUserId (_processUserMapping [p.Id]);
					_programIdentifierMapping.Add (uid, program);
				}
			}
		}

		public string NameFromIdentifier(string identifier)
		{
			try {
				var info = _manager.GetApplicationInfo (identifier, PackageInfoFlags.MetaData);
				return _manager.GetApplicationLabel(info);
			} catch (Exception) {
				return identifier;
			}
		}

		public Program FromId(int id)
		{
			var matches = _programIdentifierMapping.Values.Where ((p) => p.UserId == id);
			if(!matches.Any())
				return null;
			return matches.First();
		}

		public int GetUserId(string user)
		{
			if (!user.Contains ("u0_a"))
				return 1; //map unknown to root

			var id = int.Parse (user.Substring (4)) + 10000;
			return id;
		}

		// id binary is badly implemented on cyanogenmod, can't use this
		/*public int GetUserId(string user)
		{
			throw new Exception ();
			ProcessStartInfo s = new ProcessStartInfo ("/system/bin/id", String.Format("-u {0}", user));
			s.UseShellExecute = false;
			s.RedirectStandardOutput = true;

			var id = Process.Start (s);
			id.WaitForExit ();
			var output = id.StandardOutput.ReadLine ();
			output = output.Split ('=') [1];
			output = output.Split ('(') [0];

			return int.Parse (output);
		}*/
	}
}

