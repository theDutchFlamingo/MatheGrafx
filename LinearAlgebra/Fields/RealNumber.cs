using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra.Main;

namespace LinearAlgebra.Fields
{
	public class RealNumber : FieldMember<Real>
	{
		public RealNumber(double value) : base(new Real(value))
		{

		}

		public static FieldMember<Real> Create(double value)
		{
			return new RealNumber(value);
		}

		public override FieldMember<Real> Null()
		{
			return Create(0);
		}

		public override FieldMember<Real> Unit()
		{
			return Create(1);
		}

		public override FieldMember<Real> AdditiveInverse()
		{
			return new RealNumber(-Value.Value);
		}

		public override FieldMember<Real> MultiplicativeInverse()
		{
			if (IsNull)
				throw new ArgumentException("Null element doesn't have an inverse");

			return new RealNumber(1 / Value.Value);
		}

		protected override FieldMember<Real> Add(FieldMember<Real> fieldMember)
		{
			return new Real(Value.Value + fieldMember.Value.Value);
		}

		protected override FieldMember<Real> Multiply(FieldMember<Real> fieldMember)
		{
			return new Real(Value.Value * fieldMember.Value.Value);
		}

		public override bool Equals(FieldMember<Real> other)
		{
			return other?.Value.Value.CloseTo(Value.Value) ?? false;
		}

		public override double ToDouble()
		{
			return Value.Value;
		}
	}
}
