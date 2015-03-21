using System;
using System.Linq;
using System.Collections.Generic;

namespace ZeroKnowledge
{
	public class OrganizationController
	{
	
		public static List<Organization> CreateFromConnections(List<Connection> connections) {

			var unknownProgram = new Program (){ Identifier = "Unknown" };

			connections.ForEach (c => {
				if(c.Program == null) c.Program = unknownProgram;
			});

			return connections.GroupBy ((c) => c.Program).Select ((g) => new Organization() { 
				Connections = g.ToList(), 
				Name = g.Key.Identifier }).ToList();
		}
	}
}

