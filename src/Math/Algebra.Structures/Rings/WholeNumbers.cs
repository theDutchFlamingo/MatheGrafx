using System;
using Math.Algebra.Structures.Rings.Members;

namespace Math.Algebra.Structures.Rings
{
	public class WholeNumbers : Ring<Integer>
	{
		public override Integer Create(params object[] value)
		{
			if (value == null || value.Length == 0)
				throw new ArgumentException("Given parameters were null or empty.");
			if (value[0] is int i)
				return new Integer(i);
			if (value[0] is Integer itgr)
			{
				return new Integer(itgr);
			}

			throw new ArgumentException("Value must be of type double");
		}

		public override Integer Unit()
		{
			return new Integer(1);
		}

		public override Integer Null()
		{
			return new Integer(0);
		}
	}
}
