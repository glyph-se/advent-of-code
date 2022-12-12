using System.Collections.Immutable;

namespace AdventOfCode.Common
{
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

        public static TReturn[,] AsGrid<TReturn>(this string input, Func<char, int, int, TReturn> constructor)
        {
            var lines = input.AsLines();

            TReturn[,] grid = new TReturn[lines.Count, lines[0].Length];

            for (int row = 0; row < lines.Count; row++)
            {
                string line = lines[row];
                for (int col = 0; col < line.Length; col++)
                {
                    char c = line[col];
                    grid[row, col] = constructor(c, row, col);
                }
            }

            return grid;
        }



        /*        public static (T1, T2) AsSplit<T1, T2>(this string input, char separator = ' ')
                {
                    string[] splitted = input.Split(separator);

                    return splitted
                }
        */
    }
}
