using System;
using Math.Algebra.Fields.Members;

namespace Math.Algebra.Fields
{
	public class RealNumbers : OrderedField<Real>
	{
		public override Real Create(params object[] value)
		{
			if (value == null || value.Length == 0)
				throw new ArgumentException("Given parameters were null or empty.");
			if (value[0] is double d)
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

		public override bool Equal(Real left, Real right)
		{
			return left.Equals(right);
		}

		public override bool LessThan(Real left, Real right)
		{
			return left.LessThan(right);
		}
	}
}
