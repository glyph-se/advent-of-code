namespace Year2022.Day24;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		HashSet<Point> grid = input
			.AsGridList((c, x, y) => CreatePoint(c, x, y))
			.Where(p => p != null)
			.Cast<Point>()
			.ToHashSet();

		List<Blizzard> blizzards = input
			.AsGridList((c, x, y) => CreateBlizzard(c, x, y))
			.Where(b => b != null)
			.Cast<Blizzard>()
			.ToList();


		int maxX = grid.Max(f => f.x);
		int maxY = grid.Max(f => f.y) - 1;
		Point start = new Point(1, 0);
		Point end = new Point(maxX, maxY + 1);

		return FindMinutes(start, end, maxX, maxY, blizzards, grid).ToString();
	}

	private void MoveBlizzards(List<Blizzard> blizzards, int maxX, int maxY)
	{
		// Move all blizzards
		foreach (Blizzard b in blizzards)
		{
			b.position.x += b.direction.dx;
			b.position.y += b.direction.dy;

			if (b.position.x > maxX)
			{
				b.position.x = 1;
			}
			if (b.position.y > maxY)
			{
				b.position.y = 1;
			}
			if (b.position.x < 1)
			{
				b.position.x = maxX;
			}
			if (b.position.y < 1)
			{
				b.position.y = maxY;
			}
		}
	}

	private Blizzard? CreateBlizzard(char c, int x, int y)
	{
		Blizzard b = new(x, y);

		switch (c)
		{
			case '>':
				b.direction = (1, 0);
				break;
			case '<':
				b.direction = (-1, 0);
				break;
			case '^':
				b.direction = (0, -1);
				break;
			case 'v':
				b.direction = (0, 1);
				break;
			default:
				return null;
		}

		return b;
	}
	private Point? CreatePoint(char c, int x, int y)
	{
		if (c == '.' || c == '>' || c == '<' || c == '^' || c == 'v')
		{
			return new Point(x, y);
		}
		return null;
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		HashSet<Point> grid = input
			.AsGridList((c, x, y) => CreatePoint(c, x, y))
			.Where(p => p != null)
			.Cast<Point>()
			.ToHashSet();


		List<Blizzard> blizzards = input
			.AsGridList((c, x, y) => CreateBlizzard(c, x, y))
			.Where(b => b != null)
			.Cast<Blizzard>()
			.ToList();

		int maxX = grid.Max(f => f.x);
		int maxY = grid.Max(f => f.y) - 1;
		Point start = new Point(1, 0);
		Point end = new Point(maxX, maxY + 1);

		int part1 = FindMinutes(start, end, maxX, maxY, blizzards, grid);
		int part2 = FindMinutes(end, start, maxX, maxY, blizzards, grid);
		int part3 = FindMinutes(start, end, maxX, maxY, blizzards, grid);

		return (part1 + part2 + part3).ToString();
	}

	private int FindMinutes(Point start, Point end, int maxX, int maxY, List<Blizzard> blizzards, HashSet<Point> grid)
	{
		Queue<Point> prevPositions = new();
		prevPositions.Enqueue(start);

		for (int minute = 1; minute < 1000; minute++)
		{
			MoveBlizzards(blizzards, maxX, maxY);

			HashSet<Point> blizzardPos = blizzards.Select(b => b.position).ToHashSet();

			Queue<Point> nextPositions = new();

			while (prevPositions.Count != 0)
			{
				Point prevPos = prevPositions.Dequeue();

				(int, int)[] dirs = { (0, 0), (0, 1), (1, 0), (-1, 0), (0, -1) };

				foreach ((int dx, int dy) in dirs)
				{
					Point next = new Point(prevPos.x + dx, prevPos.y + dy);
					if (grid.Contains(next) && !blizzardPos.Contains(next))
					{
						nextPositions.Enqueue(next);
					}
				}
			}

			if (nextPositions.Contains(end))
			{
				return minute;
			}

			prevPositions = new(nextPositions.ToHashSet());
		}

		return -1;
	}

	public class Blizzard
	{
		public Blizzard(int x, int y)
		{
			position = new Point(x, y);
		}
		public (int dx, int dy) direction;
		public Point position;
	}

	public record State(int minutes, Point me);
}
