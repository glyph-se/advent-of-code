namespace Year2025.Day06;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		List<List<string>> numbers = new();

		foreach (string line in input.ParseLines())
		{
			numbers.Add(line.TrimSplit(" ").ToList());
		}

		IEnumerable<IEnumerable<string>> transposed = numbers.Transpose();

		foreach (var problem in transposed)
		{
			var operand = problem.Last();

			if (operand == "+")
			{
				result += problem.SkipLast(1).Aggregate(0L, (acc, x) => acc + long.Parse(x));
			}
			else if (operand == "*")
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

		IEnumerable<IEnumerable<char>> transposed = input.ParseLines().Transpose();

		string transposedInput = string.Join("\n", transposed.Select(c => string.Concat(c).Trim()));

		foreach (string line in transposedInput.ParseLineBlocks())
		{
			var problem = line.ParseLines();
			char operand = problem.First().Last();

			List<string> numbers = new();
			numbers.Add(new string(problem.First().SkipLast(1).ToArray()).Trim());
			numbers.AddRange(problem.Skip(1));

			if (operand == '+')
			{
				result += numbers.Aggregate(0L, (acc, x) => acc + long.Parse(x));
			}
			else if (operand == '*')
			{
				result += numbers.Aggregate(1L, (acc, x) => acc * long.Parse(x));
			}
		}

		return result.ToString();
	}
}
