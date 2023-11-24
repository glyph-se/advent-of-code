using System.Collections.Immutable;

namespace Year2021.Common;

internal static class StringParsing
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



    /*        public static (T1, T2) AsSplit<T1, T2>(this string input, char separator = ' ')
            {
                string[] splitted = input.Split(separator);

                return splitted
            }
    */
}
