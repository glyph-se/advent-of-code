using Shared;

namespace Year2023.Day16;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		var grid = input.AsGridMatrix((c, x, y) => new CharPoint(c, x, y));
		grid = grid.ExtendGridMatrix(1, (x, y) => new CharPoint('#', x, y));

		// Start one to the left because first can be a mirror
		// And this is how the "DoBeam" function works...
		result = DoBeam(grid, new Beam((1, 0), (0, 1)));

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		var grid = input.AsGridMatrix((c, x, y) => new CharPoint(c, x, y));
		grid = grid.ExtendGridMatrix(1, (x, y) => new CharPoint('#', x, y));

		for(int i = 0; i <= grid.GetUpperBound(0); i++)
		{
			result = Math.Max(result, DoBeam(grid, new Beam((0, 1), (i, 0))));
			result = Math.Max(result, DoBeam(grid, new Beam((0, -1), (i, grid.GetUpperBound(1)))));
		}

		for (int j = 0; j <= grid.GetUpperBound(1); j++)
		{
			result = Math.Max(result, DoBeam(grid, new Beam((1, 0), (0, j))));
			result = Math.Max(result, DoBeam(grid, new Beam((-1, 0), (grid.GetUpperBound(0), j))));
		}

		return result.ToString();
	}


	private static long DoBeam(CharPoint[,] grid, Beam start)
	{
		Queue<Beam> beams = new Queue<Beam>();
		beams.Enqueue(start);

		HashSet<(int x, int y)> visited = new HashSet<(int x, int y)>();
		HashSet<Beam> seenBeams = new HashSet<Beam>();

		while (beams.Count > 0)
		{
			var current = beams.Dequeue();
			if (seenBeams.Contains(current))
			{
				continue;
			}
			seenBeams.Add(current);
			visited.Add(current.pos);

			(int x, int y) next = (current.pos.x + current.dir.dx, current.pos.y + current.dir.dy);

			var nextCell = grid[next.x, next.y];

			switch (nextCell.c)
			{
				case '.':
					beams.Enqueue(new Beam(current.dir, next));
					break;
				case '/':
					if (current.dir == (1, 0))
					{
						beams.Enqueue(new Beam((0, -1), next));
					}
					if (current.dir == (-1, 0))
					{
						beams.Enqueue(new Beam((0, 1), next));
					}
					if (current.dir == (0, 1))
					{
						beams.Enqueue(new Beam((-1, 0), next));
					}
					if (current.dir == (0, -1))
					{
						beams.Enqueue(new Beam((1, 0), next));
					}
					break;
				case '\\':
					if (current.dir == (1, 0))
					{
						beams.Enqueue(new Beam((0, 1), next));
					}
					if (current.dir == (-1, 0))
					{
						beams.Enqueue(new Beam((0, -1), next));
					}
					if (current.dir == (0, 1))
					{
						beams.Enqueue(new Beam((1, 0), next));
					}
					if (current.dir == (0, -1))
					{
						beams.Enqueue(new Beam((-1, 0), next));
					}
					break;
				case '-':
					if (current.dir.dy == 0)
					{
						beams.Enqueue(new Beam(current.dir, next));
					}
					else
					{
						beams.Enqueue(new Beam((1, 0), next));
						beams.Enqueue(new Beam((-1, 0), next));
					}
					break;
				case '|':
					if (current.dir.dx == 0)
					{
						beams.Enqueue(new Beam(current.dir, next));
					}
					else
					{
						beams.Enqueue(new Beam((0, 1), next));
						beams.Enqueue(new Beam((0, -1), next));
					}

					break;
				case '#':
					// Reached edge, nothing.
					break;
			}
		}

		// Remove the start location which is outside.
		visited.Remove(start.pos);

		return visited.Count;
	}

	public class Beam
	{
		public Beam((int dx, int dy) dir, (int x, int y) pos)
		{
			this.dir = dir;
			this.pos = pos;
		}

		public (int dx, int dy) dir;
		public (int x, int y) pos;

		public override bool Equals(object? obj)
		{
			return obj is Beam beam &&
						 dir.Equals(beam.dir) &&
						 pos.Equals(beam.pos);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(dir, pos);
		}
	}
}
