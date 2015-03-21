using System;
using System.Net;
using System.Threading.Tasks;

namespace ZeroKnowledge
{
	public class Connection
	{
		public Program Program { get; set; }
		public IPEndPoint Source { get; set; }
		public IPEndPoint Destination { get; set; }
		public string Type { get; set; }
		public DateTime ConnectionStart { get; set; }

		public long Activity { get; set; }

		public string HostName { get { return _hostname == null ? null : _hostname.Result; } }
		private Task<string> _hostname;

		public double ThreatLevel { get; set; }


		public override string ToString ()
		{
			return string.Format ("[Connection: Source={1}, Destination={2}, Type={3}, ConnectionStart={4}, Activity={5}, HostName={6}, Program={0}]", Program.Identifier, Source, Destination, Type, ConnectionStart, Activity, HostName);
		}

		public void Resolve()
		{
			_hostname = Dns.GetHostEntryAsync (Destination.Address).ContinueWith<string>((t) => t.Result.HostName);
		}
	}
}

