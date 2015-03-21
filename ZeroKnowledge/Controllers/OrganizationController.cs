using System;
using System.Linq;
using System.Collections.Generic;

namespace ZeroKnowledge
{
	public class OrganizationController
	{
	
		public static List<Organization> CreateFromConnections(List<Connection> connections) {

			return connections.GroupBy ((c) => c.Program).Select ((g) => new Organization() { 
				Connections = g.ToList(), 
				ThreatLevel = g.ToList().Select((c2) => c2.ThreatLevel).Sum(),
				Name = g.Key.Identifier }).ToList();
		}
	}
}

