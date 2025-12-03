using Shared;

namespace Year2023.Day22;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		List<Brick> bricks = ParseBricks(input);

		bricks = bricks
			.OrderBy(b => Math.Min(b.p1.z, b.p2.z))
			.ToList();

		int fell = Fall(bricks);

		// Now they are all resting

		foreach (Brick b in bricks)
		{
			// Deep copy
			var without = bricks
				.Select(b => new Brick(new Point3D(b.p1.x, b.p1.y, b.p1.z), new Point3D(b.p2.x, b.p2.y, b.p2.z)))
				.ToList();

			// Remove our brick and see how many falls.
			without.Remove(b);

			int count = Fall(without);

			if (count == 0)
			{
				result++;
			}
		}

		return result.ToString();
	}

	private static List<Brick> ParseBricks(string input)
	{
		List<Brick> bricks = new();

		foreach (string line in input.AsLines())
		{
			var split = line.Split('~');

			var p1s = split[0].Split(",");
			var p2s = split[1].Split(",");

			var p1 = new Point3D(p1s[0].ToInt(), p1s[1].ToInt(), p1s[2].ToInt());
			var p2 = new Point3D(p2s[0].ToInt(), p2s[1].ToInt(), p2s[2].ToInt());
			bricks.Add(new Brick(p1, p2));
		}

		return bricks;
	}

	public int Fall(List<Brick> bricks)
	{
		HashSet<Point3D> occupied = new();

		int count = 0;

		foreach (Brick b in bricks)
		{
			int lowestZ = b.Cells().Min(p => p.z);
			bool did_move = false;

			for (int i = lowestZ; i >= 0; i--)
			{
				b.p1.z--;
				b.p2.z--;

				if (b.Cells().Any(p => occupied.Contains(p)) || b.Cells().Any(p => p.z == 0))
				{
					b.p1.z++;
					b.p2.z++;
					b.Cells().ForEach(p => occupied.Add(p));
					break;
				}

				did_move = true;
			}

			if(did_move )
			{
				count++;
			}
		}

		return count;
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		List<Brick> bricks = ParseBricks(input);

		// Sort
		bricks = bricks
			.OrderBy(b => Math.Min(b.p1.z, b.p2.z))
			.ToList();

		int fell = Fall(bricks);

		// Now they are all resting

		foreach (Brick b in bricks)
		{
			// Deep copy
			var without = bricks
				.Select(b => new Brick(new Point3D(b.p1.x, b.p1.y, b.p1.z), new Point3D(b.p2.x, b.p2.y, b.p2.z)))
				.ToList();

			// Remove our brick and see how many falls.
			without.Remove(b);

			int count = Fall(without);

			result += count;
		}

		return result.ToString();
	}

	public class Brick
	{

		public Point3D p1;
		public Point3D p2;

		public Brick(Point3D p1, Point3D p2)
		{
			this.p1 = p1;
			this.p2 = p2;
		}

		public List<Point3D> Cells()
		{
			List<Point3D> result = new List<Point3D>();
			if(p1.x != p2.x)
			{
				int min = Math.Min(p1.x, p2.x);
				int max = Math.Max(p1.x, p2.x);
				for(int i = min; i<=max; i++)
				{
					result.Add(new Point3D(i, p1.y, p1.z));
				}
			}
			else if (p1.y != p2.y)
			{
				int min = Math.Min(p1.y, p2.y);
				int max = Math.Max(p1.y, p2.y);
				for (int i = min; i <= max; i++)
				{
					result.Add(new Point3D(p1.x, i, p1.z));
				}
			}
			else if (p1.z != p2.z)
			{
				int min = Math.Min(p1.z, p2.z);
				int max = Math.Max(p1.z, p2.z);
				for (int i = min; i <= max; i++)
				{
					result.Add(new Point3D(p1.x, p1.y, i));
				}
			}
			else
			{
				result.Add(new Point3D(p1.x, p1.y, p2.z));
			}

			return result;
		}

		public override bool Equals(object? obj)
		{
			return obj is Brick brick &&
						 EqualityComparer<Point3D>.Default.Equals(p1, brick.p1) &&
						 EqualityComparer<Point3D>.Default.Equals(p2, brick.p2);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(p1, p2);
		}
	}
}
