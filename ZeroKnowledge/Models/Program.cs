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
			return string.Format ("[Program: Identifier={0}, Name={1}, ProcessId={2}, UserId={3}, ThreatLevel={4}]", Identifier, Name, ProcessId, UserId, ThreatLevel);
		}
	}
}

