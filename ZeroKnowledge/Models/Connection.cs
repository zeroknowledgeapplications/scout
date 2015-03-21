﻿using System;
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

		public long Activity { get; set; }

		public string HostName { get; set; }

		public double ThreatLevel { get; set; }


		public override string ToString ()
		{
			return string.Format ("[Connection: Program={0}, Source={1}, Destination={2}, Type={3}, ConnectionStart={4}, Activity={5}, HostName={6}]", Program, Source, Destination, Type, ConnectionStart, Activity, HostName);
		}
	}
}

