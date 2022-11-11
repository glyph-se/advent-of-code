using System.Collections.Immutable;

namespace AdventOfCode.Year2021.Day01
{
    internal class Solution : ISolution
    {
        public async Task<string> PartOne(string input)
        {
            await Task.Yield();

            var numbers = ToInts(input);

            int prev = -1;
            int result = 0;
            foreach (int current in numbers)
            {
                if (prev == -1)
                {
                    // nothing, is first
                }
                else if (current > prev)
                {
                    result++;
                }

                prev = current;
            }

            return result.ToString();
        }

        public async Task<string> PartTwo(string input)
        {
            await Task.Yield();

            var numbers = ToInts(input);

            int lastWindow = -1;
            int p0 = -1, p1 = -1, p2 = -1;

            int result = 0;
            foreach (int current in numbers)
            {
                p0 = p1;
                p1 = p2;
                p2 = current;

                if (p0 == -1)
                {
                    continue;
                }

                int currentWindow = p0 + p1 + p2;

                if (lastWindow == -1)
                {
                    // nothing, is first
                }
                else if (currentWindow > lastWindow)
                {
                    result++;
                }

                lastWindow = currentWindow;
            }

            return result.ToString();
        }

        public ImmutableList<int> ToInts(string input)
        {
            return input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToImmutableList();
        }
    }
}
