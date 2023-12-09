using Shared;
using Shared.Helpers;

namespace Year2023.Day09;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		foreach (string line in input.AsLines())
		{
			IList<long> numbers = line.TrimSplit(" ").Select(l => l.ToLong()).ToList();

			List<long> lastNumbers = new List<long>();

			var diff = numbers;

			while(!diff.All(d => d == 0))
			{
				lastNumbers.Add(diff.Last());
				diff = CalcDiff(diff);
			}

			lastNumbers.Reverse();

			long nextValue = 0;
			foreach (long lastNumber in lastNumbers)
			{
				nextValue += lastNumber;
			}

			result += nextValue;
		}

		return result.ToString();
	}

	private IList<long> CalcDiff(IList<long> numbers)
	{
		var diff = new List<long>();

		for(int i = 1; i<numbers.Count; i++)
		{
			diff.Add(numbers[i] - numbers[i-1]);
		}

		return diff;
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		foreach (string line in input.AsLines())
		{
			IList<long> numbers = line.TrimSplit(" ").Select(l => l.ToLong()).ToList();

			List<long> firstNumbers = new List<long>();

			var diff = numbers;

			while (!diff.All(d => d == 0))
			{
				firstNumbers.Add(diff.First());
				diff = CalcDiff(diff);
			}

			firstNumbers.Reverse();

			long nextValue = 0;
			foreach (long firstNumber in firstNumbers)
			{
				nextValue = firstNumber - nextValue;
			}

			result += nextValue;
		}

		return result.ToString();
	}
}
