using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math.Algebra.Groups.Members;

namespace Math.Algebra.Groups
{
	public abstract class Group<T> where T : GroupMember
	{
		public abstract T Create(params object[] value);

		public abstract T Null();
	}
}
