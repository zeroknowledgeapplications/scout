using System;
using System.Net;

namespace ZeroKnowledge
{
	public class Connection
	{
		public Program Program { get; set; }
		public IPEndPoint Source { get; set; }
		public IPEndPoint Destination { get; set; }
		public string Type { get; set; }
		public DateTime ConnectionStart { get; set; }

		public string HostName { get; set; }
	}
}

