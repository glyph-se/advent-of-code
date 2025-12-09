namespace Year2022.Day03;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long score = 0;

		foreach (string rucksack in input.ParseLines())
		{
			string part1 = rucksack.Substring(0, rucksack.Length / 2);
			string part2 = rucksack.Substring(rucksack.Length / 2);

			var result = part1.Intersect(part2);
			char common = result.First();

			if (common == char.ToLower(common))
			{
				score += common - 'a' + 1;
			}
			if (common == char.ToUpper(common))
			{
				score += common - 'A' + 27;
			}
		}

		return score.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long score = 0;

		var rucksacks = input.ParseLines().ToArray();

		for (int i = 0; i < rucksacks.Length; i = i + 3)
		{
			string part1 = rucksacks[i];
			string part2 = rucksacks[i + 1];
			string part3 = rucksacks[i + 2];

			var result = part1.Intersect(part2).Intersect(part3);

			char common = result.First();

			if (common == char.ToLower(common))
			{
				score += common - 'a' + 1;
			}
			if (common == char.ToUpper(common))
			{
				score += common - 'A' + 27;
			}
		}

		return score.ToString();
	}
}
