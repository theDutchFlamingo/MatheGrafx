using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math.Algebra.Expressions.Functions;
using Math.Algebra.Structures.Fields.Members;
using Math.Parsing;

namespace Math.Algebra.Expressions.Definitions
{
	public static class Functions
	{
		public static Dictionary<string, Function<Real>> DefaultRealFunctions = new Dictionary<string, Function<Real>>
		{
			{ "sin", new Function<Real>("sin(x)") }
		};

		public static Dictionary<string, Function<object>> NamedFunctions = new Dictionary<string, Function<object>>();
	}
}
