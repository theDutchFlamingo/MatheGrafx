using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math.Algebra.Monoids.Members
{
	public abstract class MonoidMember : IEquatable<MonoidMember>
	{
		internal abstract T Add<T>(T other) where T : MonoidMember;

		public abstract bool Equals(MonoidMember other);
	}
}
