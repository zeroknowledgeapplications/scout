using System;
using System.Collections.Generic;
using System.Linq;

namespace ZeroKnowledge
{
	public class ThreatClassifier
	{
		List<Func<Connection, double>> _predictors = new List<Func<Connection, double>>();

		public bool MatchHostNameBundleName(string host, Program program) {
			if (host == null || program == null) {
				return false;
			}
			var parts1 = host.Split ('.');
			var parts2 = program.Identifier.Split ('.');

			if (parts1.Length < 2 || parts2.Length < 2) {
				return false;
			}

			return parts2 [1] == parts1 [parts1.Length - 2];
		}

		public ThreatClassifier ()
		{
			_predictors.Add (c => 1);
			_predictors.Add (c => MatchHostNameBundleName (c.HostName, c.Program) ? 0 : 1);
			_predictors.Add (c => Math.Min (c.Activity, 20) / 20.0);
			_predictors.Add (c => c.HostName == null ? 1 : 0);
		}

		public double[] Weights = {0.01, 0.8, 0.5, 0.1, 1.0};

		public void Classify(List<Connection> connections) {

			Perceptron perceptron = new Perceptron (Weights);

			foreach (Connection connection in connections) {
				var inputs = _predictors.Select ((p) => p (connection)).ToArray();
				connection.ThreatLevel = perceptron.Predict (inputs);
			}
		}
	}
}

