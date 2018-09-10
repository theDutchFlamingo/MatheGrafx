using System;
using Math.Algebra.Structures.Groups.Members;
using Math.Algebra.Structures.Monoids;

namespace Math.Algebra.Structures.Groups
{
	public class NaturalNumbers : Monoid<Natural>
	{
		public override Natural Create(params object[] value)
		{
			throw new NotImplementedException();
		}

		public override Natural Null()
		{
			throw new NotImplementedException();
		}

		public override bool Contains<T>(T element)
		{
			switch (element)
			{
				case Natural _:
					return true;
			}

			return false;
		}
	}
}
