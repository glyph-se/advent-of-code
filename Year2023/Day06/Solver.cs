using Shared;

namespace Year2023.Day06;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = CalculateResult(input);

		return result.ToString();
	}

	private static long CalculateResult(string input)
	{
		long result;
		var lines = input.AsLines();
		List<long> times = lines[0].ReplaceRemove("Time:").TrimSplit(" ").Select(s => s.ToLong()).ToList();
		List<long> distances = lines[1].ReplaceRemove("Distance:").TrimSplit(" ").Select(s => s.ToLong()).ToList();

		List<long> winnings = new List<long>();

		for (int i = 0; i < times.Count; i++)
		{
			long time = times[i];
			long distance = distances[i];
			long winner = 0;

			for (int test = 1; test < time; test++)
			{
				long myDistance = test * (time - test);

				if (myDistance > distance)
				{
					winner++;
				}
			}

			winnings.Add(winner);
		}

		result = winnings.Aggregate((a, b) => a * b);
		return result;
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		input = input.Replace(" ", "");
		long result = CalculateResult(input);

		return result.ToString();
	}
}
