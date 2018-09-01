using System;
using Math.Rationals;

namespace Math.Algebra.Fields
{
	public class RationalNumbers : Field<Fraction>
	{
		public override Fraction Create(params object[] value)
		{
			if (value == null || value.Length != 2)
				throw new ArgumentException("Exactly two parameters must be given to create a fraction.");
			if (value[0] is int num)
			{
				if (value[1] is int den)
					return new Fraction(num, den);
				return new Fraction(num);
			}
			throw new ArgumentException("The two parameters must be of integer type");
		}
		
		public override Fraction Null()
		{
			return new Fraction();
		}

		public override Fraction Unit()
		{
			return Create(1, 0);
		}
	}
}
