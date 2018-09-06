using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Math.Bytes
{
	public static class ByteExtensions
	{
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

			int length = left.Length + right.Length;

			if (left.First() * right.First() <= Byte.MaxValue)
			{
				length--;
			}

			left = left.Minimize();
			right = right.Minimize();
			byte[] result = new byte[length];

			for (int i = left.Length - 1; i >= 0; i--)
			{
				byte[] temp = {0};

				for (int j = right.Length - 1; j >= 0; j--)
				{
					byte l = left[i];
					byte r = right[j];

					if (l * r + result[i + j] > Byte.MaxValue)
					{
						result[i + j] = (byte)(l * r + result[i + j]);
						result[i + j - 1] = 1;
					}
					else
					{
						result[i] += (byte)(l + r);
					}
				}

				result = result.Add(temp);
			}

			return result;
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
	}
}
