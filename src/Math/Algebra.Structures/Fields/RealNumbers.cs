using System;
using Math.Algebra.Structures.Fields.Members;
using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Ordering;
using Math.Algebra.Structures.Rings.Members;
using Math.Rationals;

namespace Math.Algebra.Structures.Fields
{
	public class RealNumbers : OrderedField<Real>
	{
		public override Real Create(params object[] value)
		{
			if (value == null || value.Length == 0)
			{
				throw new ArgumentException("Given parameters were null or empty.");
			}

			if (value[0] is double d)
			{
				return new Real(d);
			}

			if (Double.TryParse(value[0] as string, out d))
			{
				return new Real(d);
			}

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

		public override bool Contains<T>(T element)
		{
			switch (element)
			{
				case Real _:
				case Fraction _:
				case Integer _:
				case Natural _:
					return true;
			}

			return false;
		}
	}
}
