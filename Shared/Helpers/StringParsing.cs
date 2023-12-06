using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Shared.Helpers
{
	public static class StringParsing
	{
		public static ImmutableList<int> AsInts(this string input)
		{
			return input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToImmutableList();
		}

		public static ImmutableList<long> AsLongs(this string input)
		{
			return input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => long.Parse(s)).ToImmutableList();
		}

		public static ImmutableList<string> AsLines(this string input)
		{
			return input.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToImmutableList();
		}

		public static ImmutableList<string> AsLineBlocks(this string input)
		{
			return input.Split("\n" + "\n", StringSplitOptions.RemoveEmptyEntries).ToImmutableList();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt(this string input)
		{
			return int.Parse(input);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToLong(this string input)
		{
			return long.Parse(input);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsDigit(this char input)
		{
			return char.IsDigit(input);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string[] TrimSplit(this string input, string separator)
		{
			return input.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string[] TrimSplit(this string input, string[] separator)
		{
			return input.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string ReplaceRemove(this string input, string oldValue)
		{
			return input.Replace(oldValue, string.Empty);
		}

		public static (string first, string second) Split2(this string input, string separator)
		{
			var parts =input.TrimSplit(separator);
			if(parts.Length != 2)
			{
				throw new Exception("Expected 2 parts after split");
			}
			return (parts[0], parts[1]);
		}

		public static (string first, string second, string third) Split3(this string input, string separator)
		{
			var parts = input.TrimSplit(separator);
			if (parts.Length != 3)
			{
				throw new Exception("Expected 3 parts after split");
			}
			return (parts[0], parts[1], parts[2]);
		}

		public static (string first, string second) Split2(this string input, string[] separator)
		{
			var parts = input.TrimSplit(separator);
			if (parts.Length != 2)
			{
				throw new Exception("Expected 2 parts after split");
			}
			return (parts[0], parts[1]);
		}

		public static (string first, string second, string third) Split3(this string input, string[] separator)
		{
			var parts = input.TrimSplit(separator);
			if (parts.Length != 3)
			{
				throw new Exception("Expected 3 parts after split");
			}
			return (parts[0], parts[1], parts[2]);
		}

		/*
		 * Use something like this?
		 * https://stackoverflow.com/a/49191033
		 * 
		 * 
		/*
		public static (T1, T2) AsSplit<T1, T2>(this string input, char separator = ' ')
        {
            string[] splitted = input.Split(separator);

            return splitted
        }
        */
	}
}
