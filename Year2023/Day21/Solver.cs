using Shared;
using Shared.Helpers;

namespace Year2023.Day21;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		var grid = input.AsGridMatrix((c, x, y) => new CharPoint(c, x, y));
		grid = grid.ExtendGridMatrix(1, (x, y) => new CharPoint('#', x, y));

		var start = grid.AsList().Where(g => g.c == 'S').Single();

		HashSet<CharPoint> visited = new();
		visited.Add(start);

		for(int move = 1; move <= 64; move++)
		{
			HashSet<CharPoint> newVisited = new();
			foreach (var pos in visited)
			{
				foreach (var dir in GridHelpers.UpDowns())
				{
					var next = grid[pos.x+dir.dx,pos.y+dir.dy];
					if (next.c != '#')
					{
						newVisited.Add(next);
					}
				}
			}
			visited = newVisited;
		}

		result = visited.Count;

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		var grid = input.AsGridMatrix((c, x, y) => new CharPoint(c, x, y));
		grid = grid.ExtendGridMatrix(1, (x, y) => new CharPoint('#', x, y));

		var start = grid.AsList().Where(g => g.c == 'S').Single();

		HashSet<CharPoint> visited = new();
		visited.Add(start);

		for (int move = 1; move <= 26501365; move++)
		{
			HashSet<CharPoint> newVisited = new();
			foreach (var pos in visited)
			{
				foreach (var dir in GridHelpers.UpDowns())
				{
					var next = grid[pos.x + dir.dx, pos.y + dir.dy];
					if (next.c == 'W')
					{

					}
					if (next.c != '#')
					{
						newVisited.Add(next);
					}
				}
			}
			visited = newVisited;
		}

		result = visited.Count;

		return result.ToString();
	}
}
