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
		}

		public double[] Weights = new double[]{0.0, 1.0};

		public void Classify(List<Connection> connections) {

			Perceptron perceptron = new Perceptron (Weights);

			foreach (Connection connection in connections) {
				var inputs = _predictors.Select ((p) => p (connection)).ToArray();
				connection.ThreatLevel = perceptron.Predict (inputs);

				Debug.WriteLine(string.Format("Connection = {0} Threat = {1}",
					connection, connection.ThreatLevel
				));
			}
		}
	}
}

