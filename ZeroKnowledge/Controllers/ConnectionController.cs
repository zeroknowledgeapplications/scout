using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Net;

using Android.Content.PM;

namespace ZeroKnowledge
{
	public class ConnectionController
	{

		private static IPAddress[] _skipSourceIps = new IPAddress[] {
			IPAddress.Parse("0.0.0.0"),
			IPAddress.Parse("127.0.0.1"),
			IPAddress.Parse("::")
		};

		public static List<Connection> GetConnections(PackageManager manager)
		{

			var result = new List<Connection> ();

			var links = new Dictionary<string, Program> ();
			ProcessOverview p = new ProcessOverview(manager);
			p.Update();

			result.AddRange (ParseNetFile ("/proc/net/tcp", "tcp", p));
			result.AddRange (ParseNetFile ("/proc/net/tcp6", "tcp", p));
			result.AddRange (ParseNetFile ("/proc/net/udp", "udp", p));
			result.AddRange (ParseNetFile ("/proc/net/udp6", "udp", p));

			result.ForEach ((c) => c.Resolve ());

			return result;
		}

		private static IEnumerable<Connection> ParseNetFile(string filename, string type, ProcessOverview overview)
		{
			FileInfo f = new FileInfo (filename);
			if (!f.Exists)
				yield break;

			var reader = new StreamReader (f.OpenRead ());
			var header = reader.ReadLine ();

			while (!reader.EndOfStream) {
				var line = reader.ReadLine ();
				if (String.IsNullOrWhiteSpace (line))
					continue;
				var parts = Regex.Split (line, @"\s+");

				var uid = int.Parse (parts [8]);
				var source = ParseHexEndpoint (parts [2]);
				var dest = ParseHexEndpoint (parts [3]);
				var packets = long.Parse (parts [5].Split(':')[1], System.Globalization.NumberStyles.HexNumber);

				if (_skipSourceIps.Contains(dest.Address)) {
					continue;
				}

				var c = new Connection ();
				c.Source = source;
				c.Destination = dest;
				c.Activity = packets;
				c.Program = overview.FromId (uid);
				c.Type = type;
				yield return c;
			}
		}

		static IPEndPoint ParseHexEndpoint (string description)
		{
			var c = new Connection ();
			var ippart = description.Split (':') [0];

			if (!BitConverter.IsLittleEndian)
			{
				ippart = SwapHexEndianness (ippart);
			}
				
			IPAddress ip;
			if (ippart.Length == 8)
				ip = new IPAddress (uint.Parse (ippart, System.Globalization.NumberStyles.HexNumber));
			else {
				// convert IPv6 that is just an extended IPv4 to correct type. 
				if (ippart.Substring (0, 24) == "0000000000000000FFFF0000") {
					ip = new IPAddress (uint.Parse (ippart.Substring (24), System.Globalization.NumberStyles.HexNumber));
				} else {

					string representation = "";
					for (int i = 0; i < 4; i++) {
						var chunk = ippart.Substring (i * 8, 8);
						int u = Convert.ToInt32 (chunk, 16);
						u = IPAddress.HostToNetworkOrder (u);
						chunk = String.Format ("{0:X8}", u);
						representation += chunk.Substring (0, 4) + ":" + chunk.Substring (4, 4) + ":";
					}
					ip = IPAddress.Parse (representation.Substring (0, representation.Length - 1));
				}
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


	}
}

