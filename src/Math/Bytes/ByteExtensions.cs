using System;
using System.Linq;
using Math.Algebra.Structures.Groups.Members;
using static System.Math;

namespace Math.Bytes
{
	public static class ByteExtensions
	{
		public static Natural Add(this Natural left, Natural right)
		{
			return new Natural(left.ToString().Add(right.ToString()));
		}

		public static Natural Multiply(this Natural left, Natural right)
		{
			return new Natural(left.ToString().Multiply(right.ToString()));
		}

		/// <summary>
		/// My own adder for byte arrays
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static byte[] Add(this byte[] left, byte[] right)
		{
			if (left == null && right == null)
			{
				return new byte[] { 0 };
			}

			if (left == null || right == null)
			{
				return left ?? right;
			}

			if (left.Length == 0 || left.Length == 0)
			{
				return left.Length == 0 ? right.Length == 0 ? new byte[] { 0 } : right : left;
			}

			int length = Max(left.Minimize().Length, right.Minimize().Length);

			left = left.Minimize().Extend(length);
			right = right.Minimize().Extend(length);
			byte[] result = new byte[length];

			for (int i = result.Length - 1; i >= 0; i--)
			{
				byte l = left[i];
				byte r = right[i];

				if (l + r + result[i] > Byte.MaxValue)
				{
					result[i] = (byte)(l + r + result[i]);

					if (i == 0)
					{
						result = result.Extend(length + 1);
						result[0] = 1;
					}
					else
					{
						result[i - 1] = 1;
					}
				}
				else
				{
					result[i] += (byte)(l + r);
				}
			}

			return result;
		}

		/// <summary>
		/// My own multiplier for byte arrays
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static byte[] Multiply(this byte[] left, byte[] right)
		{
			if (left == null || right == null || left.Length == 0 || right.Length == 0)
			{
				return new byte[] { 0 };
			}

			left = left.Minimize();
			right = right.Minimize();
			int length = left.Length + right.Length;
			int scooch = 1;

			if (left.First() * right.First() <= Byte.MaxValue)
			{
				length--;
				scooch = 0;
			}

			byte[] result = new byte[length];
			string strResult = result.ToHexString();

			for (int i = left.Length - 1; i >= 0; i--)
			{
				byte[] temp = new byte[length];

				for (int j = right.Length - 1; j >= 0; j--)
				{
					byte l = left[i];
					byte r = right[j];

					if (l * r + temp[i + j] > Byte.MaxValue)
					{
						byte remainder = (byte) (l * r + temp[i + j + scooch]);
						temp[i + j - 1 + scooch] = (byte)((l * r + temp[i + j + scooch] - remainder) / 256);
						temp[i + j + scooch] = remainder;
					}
					else
					{
						temp[i + j + scooch] += (byte)(l * r);
					}
				}

				result = result.Add(temp);
				strResult = result.ToHexString();
			}

			return result;
		}

		/// <summary>
		/// My own adder for byte arrays
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static byte[] AddLittleEndian(this byte[] left, byte[] right)
		{
			if (left == null && right == null)
			{
				return new byte[] { 0 };
			}

			if (left == null || right == null)
			{
				return left ?? right;
			}

			if (left.Length == 0 || left.Length == 0)
			{
				return left.Length == 0 ? right.Length == 0 ? new byte[] { 0 } : right : left;
			}

			int length = Max(left.Minimize().Length, right.Minimize().Length);

			// Convert to little endian
			left = left.Minimize().Extend(length).Reverse().ToArray();
			right = right.Minimize().Extend(length).Reverse().ToArray();
			byte[] result = new byte[length];

			for (int i = result.Length - 1; i >= 0; i--)
			{
				byte l = left[i];
				byte r = right[i];

				if (l + r + result[i] > Byte.MaxValue)
				{
					result[i] = (byte)(l + r + result[i]);

					if (i == 0)
					{
						result = result.Extend(length + 1);
						result[0] = 1;
					}
					else
					{
						result[i - 1] = 1;
					}
				}
				else
				{
					result[i] += (byte)(l + r);
				}
			}

			return result;
		}

		/// <summary>
		/// My own multiplier for byte arrays
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static byte[] MultiplyLittleEndian(this byte[] left, byte[] right)
		{
			if (left == null || right == null || left.Length == 0 || right.Length == 0)
			{
				return new byte[] { 0 };
			}

			left = left.Minimize();
			right = right.Minimize();
			int length = left.Length + right.Length;
			int scooch = 1;

			if (left.First() * right.First() <= Byte.MaxValue)
			{
				length--;
				scooch = 0;
			}

			byte[] result = new byte[length];

			for (int i = left.Length - 1; i >= 0; i--)
			{
				byte[] temp = new byte[length];

				for (int j = right.Length - 1; j >= 0; j--)
				{
					byte l = left[i];
					byte r = right[j];

					if (l * r + temp[i + j] > Byte.MaxValue)
					{
						byte remainder = (byte)(l * r + temp[i + j]);
						temp[i + j - 1] = (byte)((l * r + temp[i + j] - remainder) / 256);
						temp[i + j] = remainder;
					}
					else
					{
						temp[i + j + scooch] += (byte)(l * r);
					}
				}

				result = result.Add(temp);
			}

			return result;
		}

		/// <summary>
		/// Subtract the right byte array from the left, the result should be positive
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static byte[] Subtract(this byte[] left, byte[] right)
		{
			if (right == null)
			{
				return left;
			}

			if (left.Equals(right))
			{
				return new byte[]{0};
			}

			if (right.GreaterThan(left))
			{
				throw new ArgumentException("Result must be positive in order to compute difference.");
			}

			int length = Max(left.Length, right.Length);

			left = left.Extend(length);
			right = right.Extend(length);
			byte[] result = new byte[length];

			for (int i = result.Length - 1; i >= 0; i--)
			{
				byte l = left[i];
				byte r = right[i];

				if (l < r)
				{
					int remainder = 0;
					int j = i - 1;
					for (; j >= 0; j--)
					{
						if (left[j] == 0)
						{
							continue;
						}

						left[j]--;
						byte digit = (byte) (Pow(0x100, i - j) + l - r);
						remainder = (int) (Pow(0x100, i - j) + l - r - digit) / 0x100;

						result[i] = digit;
						break;
					}

					for (j = i - 1; j >= 0 && remainder > 0; j--)
					{
						left[j] = (byte) (remainder % 0x100);
						remainder = (remainder - left[j]) / 0x100;
					}

					continue;
				}

				result[i] = (byte) (l - r);
			}

			return result.Minimize();
		}

		/// <summary>
		/// Whether the left byte array is greater than the right one
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool GreaterThan(this byte[] left, byte[] right)
		{
			left = left.Minimize();
			right = right.Minimize();

			if (left.Length > right.Length)
			{
				return true;
			}

			if (left.Length < right.Length)
			{
				return false;
			}

			return left.First() > right.First();
		}

		/// <summary>
		/// Creates a new byte array of the given length, with the current byte array at the end.
		/// </summary>
		/// <param name="bytes"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static byte[] Extend(this byte[] bytes, int size)
		{
			if (size < bytes.Length)
			{
				throw new ArgumentException("Size must be at least the given array length.");
			}

			byte[] result = new byte[size];
			int offset = size - bytes.Length;

			for (int i = 0; i < bytes.Length; i++)
			{
				result[i + offset] = bytes[i];
			}

			return result;
		}

		/// <summary>
		/// Make the array as small as possible, starting from the first non-zero byte
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static byte[] Minimize(this byte[] bytes)
		{
			byte[] result = { 0 };
			int offset = -1;

			for (int i = 0; i < bytes.Length; i++)
			{
				if (offset != -1)
				{
					result[i - offset] = bytes[i];
					continue;
				}

				if (bytes[i] == 0)
				{
					continue;
				}

				offset = i;
				result = new byte[bytes.Length - offset];
				result[i - offset] = bytes[i];
			}

			return result;
		}

		/// <summary>
		/// Turn a set of two characters into a single byte
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static byte SingleByte(char c1, char c2)
		{
			// First convert c1
			return (byte) (ToByte(c1) * 16 + ToByte(c2));
		}

		internal static byte ToByte(char c)
		{
			if (Char.IsDigit(c))
			{
				return (byte)(c - 48);
			}

			return (byte) (Char.ToUpper(c) - 65 + 10);
			// 65 Is the character 'A', and it has value 10
		}
	}
}
