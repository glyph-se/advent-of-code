namespace Year2025.Day00;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		/*
		var grid = input.ParseGridMatrix((c, x, y) => new CharPoint(c, x, y));
		grid = grid.ExtendGridMatrix(1, (x, y) => new CharPoint('.', x, y));

		for (int y = 1; y < grid.LengthY(); y++)
		{
			for (int x = 1; x < grid.LengthX(); x++)
			{

			}
		}
		*/

		/*
		foreach (string line in input.ParseLines())
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
