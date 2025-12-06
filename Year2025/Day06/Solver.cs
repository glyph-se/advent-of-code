using Shared;

namespace Year2025.Day06;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;
		
		List<List<string>> numbers = new();

		foreach (string line in input.AsLines())
		{
			numbers.Add(line.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList());
		}

		IEnumerable<IEnumerable<string>> transposed = numbers.Transpose();

		foreach (var problem in transposed)
		{
			var operand = problem.Last();

			if(operand == "+")
			{
				result += problem.SkipLast(1).Aggregate(0L, (acc, x) => acc + long.Parse(x));
			}
			else if(operand == "*")
			{
				result += problem.SkipLast(1).Aggregate(1L, (acc, x) => acc * long.Parse(x));
			}
		}

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		IEnumerable<IEnumerable<char>> transposed = input.AsLines().Transpose();

		// Add extra line to process last block
		transposed = transposed.Concat(Enumerable.Repeat(Enumerable.Repeat(' ', transposed.Count()), 1));

		List<long> numbersSaved = new();
		char operandSaved = ' ';

		foreach (var number in transposed)
		{
			var operand = number.Last();

			if(operand == '+' || operand == '*')
			{
				operandSaved = operand;
			}

			if(number.All(c => c == ' '))
			{
				if (operandSaved == '+')
				{
					result += numbersSaved.Aggregate(0L, (acc, x) => acc + x);
				}
				else if (operandSaved == '*')
				{
					result += numbersSaved.Aggregate(1L, (acc, x) => acc * x);
				}
				numbersSaved.Clear();
				continue;
			}

			numbersSaved.Add(long.Parse(new string(number.SkipLast(1).ToArray()).Trim()));
		}

		return result.ToString();
	}
}
