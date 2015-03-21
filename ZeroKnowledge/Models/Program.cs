using System;

namespace ZeroKnowledge
{
	public class Program
	{
		public string Identifier { get; set; }
		public string Name { get; set; }
		public int ProcessId { get; set; }
		public int UserId { get; set; }

		public int ThreatLevel { get; set; }

		public override string ToString ()
		{
			return Identifier;
		}
	}
}

