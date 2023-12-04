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
		public static string[] AsSplit(this string input, string separator)
		{
			return input.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string[] AsSplit(this string input, string[] separator)
		{
			return input.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		}

		/// <remarks>From https://stackoverflow.com/a/49191033</remarks>
		public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest)
		{

			first = list.Count > 0 ? list[0] : default(T)!; // or throw
			rest = list.Skip(1).ToList();
		}

		/// <remarks>From https://stackoverflow.com/a/49191033</remarks>
		public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest)
		{
			first = list.Count > 0 ? list[0] : default(T)!; // or throw
			second = list.Count > 1 ? list[1] : default(T)!; // or throw
			rest = list.Skip(2).ToList();
		}

		/// <remarks>From https://stackoverflow.com/a/49191033</remarks>
		public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T third, out IList<T> rest)
		{
			first = list.Count > 0 ? list[0] : default(T)!; // or throw
			second = list.Count > 1 ? list[1] : default(T)!; // or throw
			third = list.Count > 2 ? list[2] : default(T)!; // or throw
			rest = list.Skip(3).ToList();
		}


		/*        public static (T1, T2) AsSplit<T1, T2>(this string input, char separator = ' ')
                {
                    string[] splitted = input.Split(separator);

                    return splitted
                }
        */
	}
}
