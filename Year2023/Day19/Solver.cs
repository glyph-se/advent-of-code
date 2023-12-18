using Shared;
using Shared.Helpers;

namespace Year2023.Day19;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		/*
		var grid = input.AsGridMatrix((c, x, y) => new CharPoint(c, x, y));
		grid = grid.ExtendGridMatrix(1, (x, y) => new CharPoint('.', x, y));
		*/

		/*
		foreach (string line in input.AsLines())
		{

		}
		*/

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		return result.ToString();
	}
}
