using System.Collections.Generic;
using Math.Algebra.Structures.Fields.Members;

namespace Math.Algebra.Expressions.Definitions
{
	/// <summary>
	/// Contains variables (user-assigned or unknowns) and constant objects.
	/// </summary>
	public static class NamedObjects
	{
		public static readonly Dictionary<string, object> Constants = new Dictionary<string, object>
		{
			{ "e", new Real(2.718281828459045235360287471352662497757247093699959574966967627724076630353) },
			{ "pi", new Real(3.141592653589793238462643383279502884197169399375105820974944592307816406286) },
		};
	}
}
