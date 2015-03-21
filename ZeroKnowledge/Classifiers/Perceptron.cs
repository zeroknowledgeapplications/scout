using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroKnowledge
{
	class Perceptron
	{
		public int Dimensions { get; protected set; }
		public double[] Weights { get; set; }

		public Perceptron(int dimensions)
		{
			Dimensions = dimensions;
			Weights = new double[dimensions + 1];
		}

		public override string ToString()
		{
			return string.Join(",", Weights.Select((d) => d.ToString()));
		}

		public double Predict(double[] input)
		{
			if (input.Length != Dimensions)
				throw new InvalidOperationException("Wrong dimensions");

			double sum = 0;
			for (int i = 0; i < Dimensions; i++)
				sum += input[i] * Weights[i];
			sum += Weights[Dimensions];

			return sum;
		}

		public double Train(double[] input, double result, double rate)
		{
			var error = result - Predict(input);

			for(int i = 0; i < Dimensions; i++)
				Weights[i] += rate * error * input[i];
			Weights[Dimensions] += rate * error;

			return error;
		}
	}
}
