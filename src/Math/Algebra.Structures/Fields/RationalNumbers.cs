using System;
using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Rings.Members;
using Math.Rationals;

namespace Math.Algebra.Structures.Fields
{
	public class RationalNumbers : Field<Fraction>
	{
		public override Fraction Create(params object[] value)
		{
			if (value == null || value.Length != 1 && value.Length != 2)
			{
				throw new ArgumentException("Exactly two parameters must be given to create a fraction.");
			}

			if (!(value[0] is int num))
			{
				throw new ArgumentException("The two parameters must be of integer type");
			}

			if (value[1] is int den)
			{
				return new Fraction(num, den);
			}

			return new Fraction(num);
		}
		
		public override Fraction Null()
		{
			return new Fraction();
		}

		public override Fraction Unit()
		{
			return Create(1, 0);
		}

		public override bool Contains<T>(T element)
		{
			switch (element)
			{
				case Fraction _:
				case Integer _:
				case Natural _:
					return true;
			}

			return false;
		}
	}
}
