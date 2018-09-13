using System.Collections.Generic;
using Math.Algebra.Structures.Fields.Members;

namespace Math.Algebra.Expressions.Definitions
{
	/// <summary>
	/// Contains variables (user-assigned or unknowns) and constant objects.
	/// </summary>
	public static class Quantities
	{
		public static readonly Dictionary<string, object> Constants = new Dictionary<string, object>
		{
			{ "e", new Real("2.718281828459045235360287471352662497757247093699959574966967627724076630353") },
			{ "pi", new Real("3.141592653589793238462643383279502884197169399375105820974944592307816406286") },
			{ "phi", new Real("1.618033988749894848204586834365638117720309179805762862135448622705260462818") }
		};

		public static Dictionary<string, object> Variables = new Dictionary<string, object>();

		/// <summary>
		/// Add the given object to the variables dictionary with name given by the given string.
		/// </summary>
		/// <param name="variable"></param>
		/// <param name="value"></param>
		/// <returns>The object that was previously assigned to the variable name.</returns>
		public static object SetValue(this string variable, object value)
		{
			if (Constants.ContainsKey(variable) || Functions.DefaultRealFunctions.ContainsKey(variable) ||
			    Functions.NamedFunctions.ContainsKey(variable))
			{

			}

			if (Variables.ContainsKey(variable))
			{
				object prev = Variables[variable];
				Variables[variable] = value;
				return prev;
			}

			Variables.Add(variable, value);
			return null;
		}
	}
}
