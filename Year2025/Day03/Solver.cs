using Shared;

namespace Year2025.Day03;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		foreach (string line in input.AsLines())
		{
			var bank = line.ToCharArray().Select(c => c.ToString().ToInt()).ToArray();
			result += MaxJoltage(bank, 2);
		}

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		foreach (string line in input.AsLines())
		{
			var bank = line.ToCharArray().Select(c => c.ToString().ToInt()).ToArray();
			result += MaxJoltage(bank, 12);
		}

		return result.ToString();
	}

	private long MaxJoltage(int[] bank, int count)
	{
		long result = 0;

		for (int i = 1; i <= count; i++)
		{
			(int Index, int Item) max = bank[..^(count - i)].Index().MaxBy(x => x.Item);

			bank = bank[(max.Index + 1)..];
			result = 10 * result + max.Item;
		}

		return result;
	}
}
