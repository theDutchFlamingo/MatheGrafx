using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math.Algebra.Structures.Monoids.Members
{
	public abstract class MonoidMember
	{
		internal abstract T Add<T>(T other) where T : MonoidMember;

		public abstract T Null<T>() where T : MonoidMember;

		public abstract bool Equals<T>(T other);
	}
}
