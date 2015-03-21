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

		public double ThreatLevel { get; set; }

		public string CountryCode { get; set; }

		public List<Connection> Connection { get; set; }

	}
}

