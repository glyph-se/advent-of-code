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

		for (int move = 1; move <= 64; move++)
		{
			HashSet<CharPoint> newVisited = new();
			foreach (var pos in visited)
			{
				foreach (var dir in GridHelpers.UpDowns())
				{
					var next = grid[pos.x + dir.dx, pos.y + dir.dy];
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

		const long TARGET = 26501365L;

		long result = 0;

		var grid = input.AsGridMatrix((c, x, y) => new CharPoint(c, x, y));

		int gridSize = grid.GetLength(0);

		var start = grid.AsList().Where(g => g.c == 'S').Single();
		start.c = '.';

		Dictionary<long, long> moveCounts = new Dictionary<long, long>();

		HashSet<CharPoint> visited = new HashSet<CharPoint>();
		visited.Add(start);

		for (long move = 1; move <= TARGET; move++)
		{
			HashSet<CharPoint> newVisited = new();
			foreach (var pos in visited)
			{
				foreach (var dir in GridHelpers.UpDowns())
				{
					int newX = (pos.x + dir.dx);
					int newY = (pos.y + dir.dy);

					var next = grid[MathHelpers.mod(newX, gridSize), MathHelpers.mod(newY, gridSize)];
					if (next.c != '#')
					{
						newVisited.Add(new CharPoint('.', newX, newY));
					}
				}
			}

			visited = newVisited;


			if (move % gridSize == TARGET % gridSize)
			{
				moveCounts.Add(move, visited.Count);

				if (moveCounts.Count == 3)
				{
					// We only need three value
					break;
				}
			}
		}

		long a0 = moveCounts.Values.ElementAt(0);
		long a1 = moveCounts.Values.ElementAt(1);
		long a2 = moveCounts.Values.ElementAt(2);

		result = ResAfterMove(TARGET / gridSize, a0, a1, a2);

		return result.ToString();
	}

	public long ResAfterMove(long move, long a0, long a1, long a2)
	{
		long b0 = a0;
		long b1 = a1 - a0;
		long b2 = a2 - a1;

		long c0 = b0;
		long c1 = b1 * move;
		long c2 = (move * (move - 1) / 2) * (b2 - b1);

		return c0 + c1 + c2;
	}
}
