namespace Year2023.Day10;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		Pipe[,] grid = input.ParseGridMatrix((c, x, y) => new Pipe(c, x, y));
		grid = grid.ExtendGridMatrix(1, (x, y) => new Pipe('.', x, y));

		Pipe start = GetStartPipe(grid);

		Queue<Pipe> search = new Queue<Pipe>();
		search.Enqueue(start);

		while (search.Any())
		{
			Pipe current = search.Dequeue();

			Pipe right = grid[current.x + 1, current.y];
			if (right != null)
			{
				if (current.CanRight() && right.CanLeft() && right.dist == 0)
				{
					right.dist = current.dist + 1;
					search.Enqueue(right);
				}
			}

			Pipe left = grid[current.x - 1, current.y];
			if (left != null)
			{
				if (current.CanLeft() && left.CanRight() && left.dist == 0)
				{
					left.dist = current.dist + 1;
					search.Enqueue(left);
				}
			}

			Pipe up = grid[current.x, current.y - 1];
			if (up != null)
			{
				if (current.CanUp() && up.CanDown() && up.dist == 0)
				{
					up.dist = current.dist + 1;
					search.Enqueue(up);
				}
			}

			Pipe down = grid[current.x, current.y + 1];
			if (down != null)
			{
				if (current.CanDown() && down.CanUp() && down.dist == 0)
				{
					down.dist = current.dist + 1;
					search.Enqueue(down);
				}
			}
		}

		result = grid.AsList().Max(p => p.dist);

		return result.ToString();
	}

	private Pipe GetStartPipe(Pipe[,] grid)
	{
		Pipe start = grid.AsList().Where(p => p.c == 'S').Single();

		var right = grid[start.x + 1, start.y];
		var left = grid[start.x - 1, start.y];
		var up = grid[start.x, start.y - 1];
		var down = grid[start.x, start.y + 1];

		if (up.CanDown() && down.CanUp())
		{
			start.c = '|';
		}
		else if (right.CanLeft() && left.CanRight())
		{
			start.c = '-';
		}
		else if (up.CanDown() && right.CanLeft())
		{
			start.c = 'L';
		}
		else if (up.CanDown() && left.CanRight())
		{
			start.c = 'J';
		}
		else if (down.CanUp() && left.CanRight())
		{
			start.c = '7';
		}
		else if (down.CanUp() && right.CanLeft())
		{
			start.c = 'F';
		}
		else
		{
			throw new Exception("Error, start not found");
		}

		return start;
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		Pipe[,] grid = input.ParseGridMatrix((c, x, y) => new Pipe(c, x, y));
		grid = grid.ExtendGridMatrix(1, (x, y) => new Pipe('.', x, y));

		Pipe start = GetStartPipe(grid);

		Queue<Pipe> search = new Queue<Pipe>();
		search.Enqueue(start);

		while (search.Any())
		{
			Pipe current = search.Dequeue();

			Pipe right = grid[current.x + 1, current.y];
			if (right != null)
			{
				if (current.CanRight() && right.CanLeft() && right.dist == 0)
				{
					right.dist = current.dist + 1;
					search.Enqueue(right);
				}
			}

			Pipe left = grid[current.x - 1, current.y];
			if (left != null)
			{
				if (current.CanLeft() && left.CanRight() && left.dist == 0)
				{
					left.dist = current.dist + 1;
					search.Enqueue(left);
				}
			}

			Pipe up = grid[current.x, current.y - 1];
			if (up != null)
			{
				if (current.CanUp() && up.CanDown() && up.dist == 0)
				{
					up.dist = current.dist + 1;
					search.Enqueue(up);
				}
			}

			Pipe down = grid[current.x, current.y + 1];
			if (down != null)
			{
				if (current.CanDown() && down.CanUp() && down.dist == 0)
				{
					down.dist = current.dist + 1;
					search.Enqueue(down);
				}
			}
		}

		for (int y = 0; y < grid.GetUpperBound(1); y++)
		{
			bool inn = false;
			for (int x = 0; x < grid.GetUpperBound(0); x++)
			{
				Pipe current = grid[x, y];

				if (current == null) continue;

				if (current.dist != 0)
				{
					if (current.c == '|' || current.c == 'J' || current.c == 'L')
					{
						inn = !inn;
					}
				}
				else
				{
					if (inn)
					{
						result++;
					}
				}
			}
		}

		return result.ToString();
	}

	public class Pipe(char c, int x, int y) : CharPoint(c, x, y)
	{
		public int dist = 0;

		public bool CanRight()
		{
			return c == '-' || c == 'L' || c == 'F';
		}

		public bool CanLeft()
		{
			return c == '-' || c == 'J' || c == '7';
		}

		public bool CanUp()
		{
			return c == '|' || c == 'L' || c == 'J';
		}

		public bool CanDown()
		{
			return c == '|' || c == 'F' || c == '7';
		}
	}
}
