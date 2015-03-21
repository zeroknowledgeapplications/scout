using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ZeroKnowledge
{
	public class ThreatClassifier
	{
		List<Func<Connection, double>> _predictors = new List<Func<Connection, double>>();

		public ThreatClassifier ()
		{
			_predictors.Add ((c) => 1);
			_predictors.Add ((c) => c.HostName != null ? (c.HostName.Contains ("spotify") ? 0 : 1) : 1);
			_predictors.Add ((c) => c.HostName == null ? 1 : 0);
		}

		public double[] Weights = new double[]{0.1, 0.8, 0.1, 1.0};

		public void Classify(List<Connection> connections) {

			Perceptron perceptron = new Perceptron (Weights);

			foreach (Connection connection in connections) {
				var inputs = _predictors.Select ((p) => p (connection)).ToArray();
				connection.ThreatLevel = perceptron.Predict (inputs);
			}
		}
	}
}

