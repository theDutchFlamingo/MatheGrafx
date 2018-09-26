using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math.Settings
{
	/// <summary>
	/// A class which contains the setting related to numbers, so the output formatting,
	/// whether fractions should be factored down or not, etc.
	/// </summary>
	public static class NumberSettings
	{
		public static bool FactorFractions { get; set; } = true;
	}
}
