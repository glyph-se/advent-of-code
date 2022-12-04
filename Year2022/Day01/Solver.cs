using AdventOfCode.Common;

namespace AdventOfCode.Year2022.Day01
{
    internal class Solver : ISolver
    {
        public async Task<string> PartOne(string input)
        {
            await Task.Yield();

            var elfs = StringParsing.AsLineBlocks(input);

            List<int> elfCalories = new List<int>();

            foreach (string elf in elfs)
            {
                var calories = StringParsing.AsInts(elf);
                elfCalories.Add(calories.Sum());
            }

            return elfCalories.Max().ToString();
        }

        public async Task<string> PartTwo(string input)
        {
            await Task.Yield();

            var elfs = StringParsing.AsLineBlocks(input);

            List<int> elfCalories = new List<int>();

            foreach (string elf in elfs)
            {
                var calories = StringParsing.AsInts(elf);
                elfCalories.Add(calories.Sum());
            }

            var sortedList = elfCalories.OrderDescending().ToArray();

            int result = sortedList[0] + sortedList[1] + sortedList[2];
            return result.ToString();
        }
    }
}
