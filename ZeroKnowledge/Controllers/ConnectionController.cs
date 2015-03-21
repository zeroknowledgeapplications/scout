using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Net;

namespace ZeroKnowledge
{
	public class ConnectionController
	{

		private static IPAddress[] _skipSourceIps = new IPAddress[] {
			IPAddress.Parse("0.0.0.0"),
			IPAddress.Parse("127.0.0.1")
		};

		public static List<Connection> GetConnections()
		{
			Dictionary<string, Program> programs = new Dictionary<string, Program> ();

			var plist = Process.GetProcesses ();
			var result = new List<Connection> ();

			foreach (var p in plist) {
				var uid = GetUser (p.Id);

				if (uid == null)
					continue;

				if (!programs.ContainsKey (uid))
					programs.Add (uid, new Program (){ Identifier = uid, ProcessId = p.Id });
			}

			foreach (var p in programs.Values) {
				foreach (var con in ParseNetFile (String.Format("/proc/{0}/net/tcp", p.ProcessId))) {
					con.Type = "tcp";
					con.Program = p;
					result.Add (con);
				}
				/*
				foreach (var con in ParseNetFile (String.Format("/proc/{0}/net/tcp6", p.ProcessId))) {
					con.Type = "tcp";
					con.Program = p;
					result.Add (con);
				}
				foreach (var con in ParseNetFile(String.Format("/proc/{0}/net/udp", p.ProcessId))){
					con.Type = "udp";
					con.Program = p;
					result.Add (con);
				}
				foreach (var con in ParseNetFile(String.Format("/proc/{0}/net/udp6", p.ProcessId))){
					con.Type = "udp";
					con.Program = p;
					result.Add (con);
				}*/
			}

			return result;
		}

		private static IEnumerable<Connection> ParseNetFile(string filename)
		{
			FileInfo f = new FileInfo (filename);
			if (!f.Exists)
				yield break;

			var reader = new StreamReader (f.OpenRead ());

			while (!reader.EndOfStream) {
				var line = reader.ReadLine ();
				if (String.IsNullOrWhiteSpace (line))
					continue;
				var parts = Regex.Split (line, @"\s+");

				if (parts [1] == "sl")
					continue;

				var source = ParseHexEndpoint (parts [2]);
				var dest = ParseHexEndpoint (parts [3]);
				var packets = long.Parse (parts [5].Split(':')[1], System.Globalization.NumberStyles.HexNumber);

				if (_skipSourceIps.Contains(dest.Address)) {
					continue;
				}

				//Debug.WriteLine ("D:"+ dest);
				//Debug.WriteLine ("S:"+ source);

				var c = new Connection ();
				c.Source = source;
				c.Destination = dest;
				c.Activity = packets;
				yield return c;
			}
		}

		static IPEndPoint ParseHexEndpoint (string description)
		{
			var c = new Connection ();
			var ippart = description.Split (':') [0];

			if ((BitConverter.IsLittleEndian && ippart.Length != 8) ||
				(!BitConverter.IsLittleEndian && ippart.Length == 8))
			{
				ippart = SwapHexEndianness (ippart);
			}
				
			IPAddress ip;
			if (ippart.Length == 8)
				ip = new IPAddress (uint.Parse (ippart, System.Globalization.NumberStyles.HexNumber));
			else {
				string representation = "";
				for (int i = 0; i < 8; i++)
					representation += ippart.Substring (i * 4, 4) + ":";
				ip = IPAddress.Parse(representation.Substring(0, representation.Length - 1));
			}

			var sourceport = int.Parse (description.Split (':') [1], System.Globalization.NumberStyles.HexNumber);

			return new IPEndPoint (ip, sourceport);
		}

		public static string SwapHexEndianness(string input)
		{
			if (input.Length % 2 != 0)
				throw new ArgumentException ("input is not a hex string", "input");

			string result = "";
			for(int i = 2; i < input.Length + 1; i += 2)
			{
				result += input.Substring (input.Length - i, 2);
			}

			return result;
		}

		public static string GetUser(int pid)
		{
			FileInfo f = new FileInfo (String.Format ("/proc/{0}/cmdline", pid));
			if (!f.Exists)
				return null;

			var reader = new StreamReader (f.OpenRead ());
			var uid = reader.ReadLine ();

			// Filter system processes
			if (uid != null && uid.Contains ("/"))
				uid = null;

			return uid;
		}
	}
}

