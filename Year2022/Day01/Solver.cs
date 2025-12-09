namespace Year2022.Day01;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		var elfs = input.ParseLineBlocks();

		List<int> elfCalories = new List<int>();

		foreach (string elf in elfs)
		{
			var calories = elf.ParseInts();
			elfCalories.Add(calories.Sum());
		}

		return elfCalories.Max().ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		var elfs = input.ParseLineBlocks();

		List<int> elfCalories = new List<int>();

		foreach (string elf in elfs)
		{
			var calories = elf.ParseInts();
			elfCalories.Add(calories.Sum());
		}

		var sortedList = elfCalories.OrderDescending().ToArray();

		int result = sortedList[0] + sortedList[1] + sortedList[2];
		return result.ToString();
	}
}
