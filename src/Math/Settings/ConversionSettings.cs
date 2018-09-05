using System;
using System.Collections.Generic;
using static System.String;

namespace Math.Settings
{
	/// <summary>
	/// A class that contains all the setting related to conversion,
	/// especially conversions from vector to matrices.
	/// </summary>
	public static class ConversionSettings
	{
		/// <summary>
		/// When no explicit VectorType is given, this default will be selected
		/// </summary>
		public static VectorType DefaultVectorType { get; set; } = VectorType.Column;

		/// <summary>
		/// The delimiter that is used for converting matrices and vectors to latex strings.
		/// </summary>
		public static char DefaultLatexDelimiter = '(';

		/// <summary>
		/// The delimiter that is used for converting matrices and vectors to strings.
		/// </summary>
		public static char DefaultStringDelimiter = '{';

		public static readonly List<char> OpenDelimiters = new List<char>
		{
			'(', '[', '{', '|'
		};

		public static readonly List<char> CloseDelimiters = new List<char>
		{
			')', ']', '}', '|'
		};

		/// <summary>
		/// When converting from and to string, delimiters must be used (or parsed).
		/// This method helps out by quickly finding the matching delimiter.
		/// </summary>
		/// <param name="delimiter"></param>
		/// <returns></returns>
		public static char MatchingDelimiter(this char delimiter)
		{
			switch (delimiter)
			{
				case '(':
					return ')';
				case '[':
					return ']';
				case '|':
					return '|';
				case '{':
					return '}';
			}

			throw new ArgumentException("Delimiter for a matrix must be one of: '" +
			                            Join("', '", OpenDelimiters) + "'.");
		}
	}
}
