using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebra.Numeric
{
	public class Polynomial
	{
		private Vector _coefficients;

		public int Degree { get; private set; }

		public Vector Coefficients
		{
			get => _coefficients;
			set
			{
				_coefficients = value;
				Degree = value.Dimension;
			}
		}

		/// <summary>
		/// Create a polynomial with as coefficients
		/// </summary>
		/// <param name="coefficients"></param>
		public Polynomial(Vector coefficients)
		{
			Coefficients = coefficients;
		}
	}
}
