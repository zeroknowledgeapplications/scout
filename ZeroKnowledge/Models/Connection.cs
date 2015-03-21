using System;
using System.Net;

namespace ZeroKnowledge
{
	public class Connection
	{
		Program Program { get; set; }
		IPEndPoint Source { get; set; }
		IPEndPoint Destination { get; set; }
		string Type { get; set; }
		DateTime ConnectionStart { get; set; }

		string HostName { get; set; }


	}
}

