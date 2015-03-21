using System;
using System.Collections.Generic;

namespace ZeroKnowledge
{
	public class Organization
	{
		public Organization ()
		{
		}

		public string Name { get; set; }

		public List<Connection> Connections { get; set; }

		public double ThreatLevel { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Organization: Name={0}, Connections={1}, ThreatLevel={2}]", Name, Connections.Count, ThreatLevel);
		}

	}
}

