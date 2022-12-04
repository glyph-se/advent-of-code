using System.Collections.Immutable;

namespace AdventOfCode.Common
{
    internal static class StringParsing
    {
        public static ImmutableList<int> AsInts(string input)
        {
            return input.Trim().Split('\n').Select(s => int.Parse(s)).ToImmutableList();
        }

        public static ImmutableList<long> AsLongs(string input)
        {
            return input.Trim().Split('\n').Select(s => long.Parse(s)).ToImmutableList();
        }

        public static ImmutableList<string> AsLines(string input)
        {
            return input.Trim().Split('\n').ToImmutableList();
        }

        public static ImmutableList<string> AsBlocks(string input)
        {
            return input.Trim().Split("\n" + "\n").ToImmutableList();
        }
    }
}
