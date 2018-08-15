using System;
using LinearAlgebra.Main;

namespace LinearAlgebra.Fields
{
	/// <summary>
	/// A class that represents real numbers
	/// </summary>
	public class Real : RealNumber
	{
		public new double Value { get; set; }

		public Real() : base(0)
		{

		}

		public Real(double value) : base(value)
		{
			Value = value;
		}
	}
}
