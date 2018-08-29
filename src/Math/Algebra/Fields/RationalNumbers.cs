using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math.Rationals;

namespace Math.Algebra.Fields
{
	public class RationalNumbers : Field<Fraction>
	{
		public override Fraction Unit()
		{
			return Create(new { Num = 0, Den = 0 });
		}

		public override Fraction Null()
		{
			throw new NotImplementedException();
		}

		public override Fraction Create(params object[] value)
		{
			if (value == null || value.Length != 2)
				throw new ArgumentException("Exactly two parameters must be given to create a fraction.");
			if (value[0] is int num && value[1] is int den)
				return new Fraction(num, den);
			throw new ArgumentException("The two parameters must be of integer type");
		}
	}
}
