using Shared;

namespace Year2022.Day01
{
	public class Solver : ISolver
	{
		public async Task<string> PartOne(string input)
		{
			await Task.Yield();

			var elfs = input.AsLineBlocks();

			List<int> elfCalories = new List<int>();

			foreach (string elf in elfs)
			{
				var calories = elf.AsInts();
				elfCalories.Add(calories.Sum());
			}

			return elfCalories.Max().ToString();
		}

		public async Task<string> PartTwo(string input)
		{
			await Task.Yield();

			var elfs = input.AsLineBlocks();

			List<int> elfCalories = new List<int>();

			foreach (string elf in elfs)
			{
				var calories = elf.AsInts();
				elfCalories.Add(calories.Sum());
			}

			var sortedList = elfCalories.OrderDescending().ToArray();

			int result = sortedList[0] + sortedList[1] + sortedList[2];
			return result.ToString();
		}
	}
}
