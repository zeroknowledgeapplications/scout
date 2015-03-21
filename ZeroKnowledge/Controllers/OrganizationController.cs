using System;
using System.Collections.Generic;

namespace ZeroKnowledge
{
	public class OrganizationController
	{

		public List<Connection> Connections { get; set; }
		public string Name { get; set; }

		public OrganizationController (string name, List<Connection> connections)
		{
			Name = name;
			Connections = connections;
		}

		public static List<Organization> CreateFromConnections(List<Connection> connections) {
			// TODO: group here!
			return new List<Organization>();

		}
	}
}

