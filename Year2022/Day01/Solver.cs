using AdventOfCode.Common;

namespace AdventOfCode.Year2022.Day01
{
    internal class Solver : ISolver
    {
        public async Task<string> PartOne(string input)
        {
            await Task.Yield();

            var calories = Helpers.AsLines(input);

            List<int> elfCalories = new List<int>();

            int currentElf = 0;
            foreach (string current in calories)
            {
                if (current == "")
                {
                    elfCalories.Add(currentElf);
                    currentElf = 0;
                }
                else
                {
                    currentElf += int.Parse(current);
                }
            }
            if (currentElf > 0)
            {
                elfCalories.Add(currentElf);
            }

            return elfCalories.Max().ToString();
        }

        public async Task<string> PartTwo(string input)
        {
            await Task.Yield();

            var calories = Helpers.AsLines(input);

            List<int> elfCalories = new List<int>();

            int currentElf = 0;
            foreach (string current in calories)
            {
                if (current == "")
                {
                    elfCalories.Add(currentElf);
                    currentElf = 0;
                }
                else
                {
                    currentElf += int.Parse(current);
                }
            }
            if (currentElf > 0)
            {
                elfCalories.Add(currentElf);
            }

            var sortedList = elfCalories.OrderDescending().ToArray();

            int result = sortedList[0] + sortedList[1] + sortedList[2];
            return result.ToString();
        }
    }
}
