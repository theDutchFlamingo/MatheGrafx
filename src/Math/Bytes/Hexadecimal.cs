using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Numerics;
using static System.Math;
using static Math.Bytes.ByteExtensions;

namespace Math.Bytes
{
	public static class Hexadecimal
	{
		public static string ToHexString(this byte[] value)
		{
			try
			{
				return BitConverter.ToString(value).Replace("-", "").Minimize();
			}
			catch (ArgumentOutOfRangeException)
			{
				return "";
			}
		}

		public static byte[] FromHexadecimal(string value)
		{
			if (!Regex.IsMatch(value, @"^[0-9a-fA-F]+$"))
			{
				throw new ArgumentException("Hexadecimal string must contain only numbers, " +
											"and letters A to F");
			}

			List<char> digits = value.Reverse().ToList();
			List<byte> result = new List<byte>();

			for (int i = 0; i < digits.Count; i += 2)
			{
				if (digits.Count <= i + 1)
				{
					result.Add(ToByte(digits[i]));
				}
				else
				{
					byte b = SingleByte(digits[i + 1], digits[i]);
					result.Add(b);
				}
			}

			result.Reverse();

			return result.ToArray();
		}

		public static byte[] FromDecimal(string value)
		{
			return BigInteger.Parse(value).ToByteArray().Reverse().ToArray();
		}

		/// <summary>
		/// Change a hex string into a decimal string
		/// </summary>
		/// <param name="hex"></param>
		/// <returns></returns>
		public static string ToDecimal(this string hex)
		{
			byte[] newBytes = new byte[]{0}.Concat(FromHexadecimal(hex)).Reverse().ToArray();
			
			return new BigInteger(newBytes).ToString("D");
		}

		/// <summary>
		/// Change a decimal string into a hex string
		/// </summary>
		/// <param name="dec"></param>
		/// <returns></returns>
		public static string ToHexadecimal(this string dec)
		{
			BigInteger b = BigInteger.Parse(dec);

			return b.ToString("X");
		}

		public static string AddEfficiently(this string left, string right)
		{
			// TODO add an adder which partially uses the default addition of longs and partially the split addition
			const int maxLength = 17; // The max where 2 times a string of digits is guaranteed to be parsable to long

			if (left.Length < maxLength && right.Length < maxLength)
			{
				long.TryParse(left, out long l);
				long.TryParse(right, out long r);
				return (l + r).ToString();
			}

			List<char> charL = left.Minimize().Reverse().ToList();
			List<char> charR = right.Minimize().Reverse().ToList();

			var splitL = new List<string>();
			var splitR = new List<string>();
			var result = new List<string>();

			for (int i = 0; i < charL.Count; i += maxLength)
			{
				int count = charL.Count - i < maxLength ? charL.Count - i : maxLength;
				splitL.Add(string.Join("", charL.GetRange(i, count)));
			}

			for (int i = 0; i < charR.Count; i += maxLength)
			{
				int count = charR.Count - i - 1 < maxLength ? charR.Count - i : maxLength;
				splitR.Add(string.Join("", charR.GetRange(i, count)));
			}

			long carry = 0;

			for (int i = 0; i < Max(splitL.Count, splitR.Count); i++)
			{
				long l = splitL.Count > i ? long.Parse(new string(splitL[i].Reverse().ToArray())) : 0;
				long r = splitR.Count > i ? long.Parse(new string(splitR[i].Reverse().ToArray())) : 0;

				string res = (l + r + carry).ToString();

				if (res.Length > maxLength)
				{
					carry = 1;
					res = res.Substring(1);
				}
				else
				{
					carry = 0;
				}

				result.Add(res);
			}

			result.Reverse();

			return string.Join("", result);
		}

		/// <summary>
		/// Add two decimal strings together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static string AddDecimal(this string left, string right)
		{
			int length = Max(left.Minimize().Length, right.Minimize().Length);

			left = left.Minimize().Extend(length);
			right = right.Minimize().Extend(length);
			StringBuilder result = new StringBuilder(new String('0', length));

			for (int i = result.Length - 1; i >= 0; i--)
			{
				int l = left[i] - 48;
				int r = right[i] - 48;

				if (l + r + result[i] - 48 > 9)
				{
					result[i] = $"{l + r + result[i] - 48}"[1];
					// At index 1 because the sum is double digit.

					if (i == 0)
					{
						result = result.Extend(length + 1);
						result[0] = '1';
					}
					else
					{
						result[i - 1] = '1';
					}
				}
				else
				{
					result[i] = $"{l + r + result[i] - 48}"[0];
				}
			}

			return result.ToString();
		}

		/// <summary>
		/// Multiply two decimal strings together
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static string MultiplyDecimal(this string left, string right)
		{
			left = left.Minimize();
			right = right.Minimize();
			int length = left.Length + right.Length;
			int scooch = 1;

			if (MultiplyChar(left.First(), right.First()).StartsWith("0"))
			{
				length--;
				scooch = 0;
			}

			string result = new String('0', length);

			for (int i = left.Length - 1; i >= 0; i--)
			{
				int[] temp = new int[length];

				for (int j = right.Length - 1; j >= 0; j--)
				{
					char l = left[i];
					char r = right[j];

					if ((l - 48) * (r - 48) + result[i+ j] - 48 > 9)
					{
						// This should hopefully replace the two lines below
						result = result.AddDecimal(MultiplyChar(l, r).PadRight(length - i - j, '0'));
						result = result.Extend(length);

						//temp[i + j] = MultiplyChar(l, r).Item1;
						//temp[i + j + 1] += MultiplyChar(l, r).Item2;
					}
					else
					{
						result = result.AddDecimal(MultiplyChar(l, r).PadRight(length - i - j - scooch, '0'));
						result = result.Extend(length);

						//temp[i + j + scooch] = MultiplyChar(l, r).Item2 + temp[i + j + scooch];
					}
				}

				//result = result.AddDecimal(new string(temp.Select(t => $"{t}"[0]).ToArray()));
			}

			return result;
		}

		private static StringBuilder Minimize(this StringBuilder str)
		{
			return new StringBuilder(str.ToString().TrimStart('0'));
		}

		private static StringBuilder Extend(this StringBuilder str, int length)
		{
			return new StringBuilder(str.ToString().PadLeft(length, '0'));
		}

		private static string Minimize(this string str)
		{
			return str.TrimStart('0');
		}

		private static string Extend(this string str, int length)
		{
			return str.PadLeft(length, '0');
		}

		public static string MultiplyChar(char left, char right)
		{
			int l = left - 48, r = right - 48;
			int remainder = l * r % 10;
			return $"{(l * r - remainder)/10}{remainder}".Minimize();
		}
	}
}
