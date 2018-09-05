using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math.Algebra.Expressions
{
	public enum InvalidExpressionType
	{
		/// <summary>
		/// When the expression contains one of the symbols from the list
		/// of invalid symbols.
		/// </summary>
		InvalidSymbol,
		/// <summary>
		/// When there are more closing delimiters than opening delimiters.
		/// </summary>
		CloseDelimiters,
		/// <summary>
		/// For infix expressions, when there are two numbers in a row
		/// without a binary operator in between.
		/// </summary>
		NumberRepetition,
		/// <summary>
		/// For when a number contains more than one decimal point / comma
		/// </summary>
		MultipleDecimalSeparators,
		/// <summary>
		/// For pre/postfix, when too many arguments are given on an operator
		/// with explicit amount of arguments.
		/// </summary>
		ArgumentOverflow,
		/// <summary>
		/// For pre/postfix, when too few arguments are given on an operator
		/// with explicit amount of arguments.
		/// </summary>
		ArgumentUnderflow
	}
}
