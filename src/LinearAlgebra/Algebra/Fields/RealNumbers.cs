using System;

namespace LinearAlgebra.Fields
{
	public class RealNumbers : Field<Real>
	{
		public override Real Create(object value)
		{
			if (value is double d)
				return new Real(d);
			throw new ArgumentException("Value must be of type double");
		}

		public override Real Null()
		{
			return Create(0);
		}

		public override Real Unit()
		{
			return Create(1);
		}
	}
}
