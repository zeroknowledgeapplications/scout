using System;
using System.Collections.Generic;

namespace ZeroKnowledge
{
	public class ThreatClassifier
	{
		public ThreatClassifier ()
		{

		}

		public static double[] Weights = new double[]{0.5,0.5,1.0, 0.0};

		public static void Classify(List<Connection> connections) {

			Perceptron perceptrion = new Perceptron (Weights);

			foreach (Connection connection in connections) {
				connection.ThreatLevel = perceptrion.Predict (
					new double[]{
						() => { 
							return 0.0;
						},
						() => { 
							return 0.0;
						},
						() => { 
							return 1.0;
						},
					}
				);
			}
		}
	}
}

