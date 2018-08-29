using System;
using Math.Algebra.Monoids.Members;
using Math.Algebra.Rings.Members;
using Math.Exceptions;

namespace Math.Algebra.Groups.Members
{
	public class Natural : MonoidMember
	{
		public int Value { get; set; }

		public Natural()
		{

		}

		public Natural(int value)
		{
			Value = System.Math.Abs(value);
		}

		public Natural(uint value)
		{
			if (value > Int32.MaxValue)
				throw new ArgumentException("Value exceeds the maximum value for integers");
		}

		internal override T Add<T>(T other)
		{
			if (other is Natural n)
			{
				return (T) (MonoidMember) new Natural(n.Value + Value);
			}

			if (other is Integer i && i >= 0)
			{
				return (T) (MonoidMember) new Natural(i.Value + Value);
			}

			throw new IncorrectSetException(GetType(), "added", other.GetType());
		}

		public override bool Equals(MonoidMember other)
		{
			if (other is Natural n)
			{
				return n.Value == Value;
			}

			return false;
		}
	}
}
