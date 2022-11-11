using System.Collections.Immutable;

namespace AdventOfCode.Common
{
    internal class Helpers
    {
        public static ImmutableList<int> AsInts(string input)
        {
            return input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToImmutableList();
        }

        public static ImmutableList<string> AsLines(string input)
        {
            return input.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToImmutableList();
        }
    }
}
