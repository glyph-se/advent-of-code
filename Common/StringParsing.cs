using System.Collections.Immutable;

namespace AdventOfCode.Common
{
    internal static class StringParsing
    {
        public static ImmutableList<int> AsInts(string input)
        {
            return input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToImmutableList();
        }

        public static ImmutableList<long> AsLongs(string input)
        {
            return input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => long.Parse(s)).ToImmutableList();
        }

        public static ImmutableList<string> AsLines(string input)
        {
            return input.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToImmutableList();
        }

        public static ImmutableList<string> AsLineBlocks(string input)
        {
            return input.Split("\n" + "\n", StringSplitOptions.RemoveEmptyEntries).ToImmutableList();
        }
    }
}
