using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math.Parsing
{
	public class Parser<T>
	{
		private readonly Func<string, T> _parser;

		public Parser(Func<string, T> parsingFunction)
		{
			_parser = parsingFunction;
		}

		public T Parse(string value)
		{
			return _parser(value);
		}
	}
}
