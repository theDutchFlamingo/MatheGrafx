using System;
using Math.Algebra.Structures.Monoids.Members;
using Math.Algebra.Structures.Rings.Members;
using Math.Exceptions;

namespace Math.Algebra.Structures.Groups.Members
{
	public class Natural : MonoidMember
	{
		private int Value { get; }

		public Natural()
		{
			Value = 0;
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

		#region Overrides

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

		public override T Null<T>()
		{
			if (typeof(T) == GetType())
				return (T)(MonoidMember) new Natural(0);
			throw new IncorrectSetException(this, "null", typeof(T));
		}

		public override bool Equals<T>(T other)
		{
			if (other is Natural n)
			{
				return n.Value == Value;
			}

			return false;
		}

		#endregion
	}
}
