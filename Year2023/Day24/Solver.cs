using System.Diagnostics;

namespace Year2023.Day24;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		List<Hail> points = new();

		foreach (string line in input.AsLines())
		{
			var split = line.TrimSplit(["@", ","]);

			points.Add(new Hail(
				split[0].ToLong(),
				split[1].ToLong(),
				split[2].ToLong(),
				split[3].ToLong(),
				split[4].ToLong(),
				split[5].ToLong()));
		}

		// Full
		long min = 200000000000000;
		long max = 400000000000000;

		if (points.Count == 5)
		{
			// Example
			min = 7;
			max = 27;
		}

		IEnumerable<IEnumerable<Hail>> all = points.DifferentCombinations(2);
		foreach (var pair in all)
		{
			var hail1 = pair.ElementAt(0);
			var hail2 = pair.ElementAt(1);

			(bool intersects, double px, double py, _, _) = IntersectsLong(hail1, hail2);

			if (!intersects) continue;

			// Check for future or past
			if (hail1.dx > 0 && px < hail1.x)
			{
				continue;
			}
			if (hail1.dx < 0 && px > hail1.x)
			{
				continue;
			}
			if (hail2.dx > 0 && px < hail2.x)
			{
				continue;
			}
			if (hail2.dx < 0 && px > hail2.x)
			{
				continue;
			}
			if (hail1.dy > 0 && py < hail1.y)
			{
				continue;
			}
			if (hail1.dy < 0 && py > hail1.y)
			{
				continue;
			}
			if (hail2.dy > 0 && py < hail2.y)
			{
				continue;
			}
			if (hail2.dy < 0 && py > hail2.y)
			{
				continue;
			}

			// Check intervall statement
			if (px > min && px < max && py > min && py < max)
			{
				result++;
			}
		}

		return result.ToString();
	}


	private static (bool intersects, long px, long py, double time1, double time2) IntersectsLong(Hail hail1, Hail hail2)
	{
		long d = hail1.dy * hail2.dx - hail1.dx * hail2.dy;
		if (d == 0)
		{
			// Parallell
			return (false, 0, 0, 0, 0);
		}

		// Taken from https://stackoverflow.com/a/27474217
		// I also considered // https://stackoverflow.com/a/4543530, but it had the wrong format.
		(long x, long y) p1End = (hail1.x + hail1.dx, hail1.y + hail1.dy); // another point in line p1->n1
		(long x, long y) p2End = (hail2.x + hail2.dx, hail2.y + hail2.dy); // another point in line p2->n2

		double m1 = (double)(p1End.y - hail1.y) / (double)(p1End.x - hail1.x); // slope of line p1->n1
		double m2 = (double)(p2End.y - hail2.y) / (double)(p2End.x - hail2.x); // slope of line p2->n2

		double b1 = (double)hail1.y - (double)(m1 * hail1.x); // y-intercept of line p1->n1
		double b2 = (double)hail2.y - (double)(m2 * hail2.x); // y-intercept of line p2->n2

		double px = (double)(b2 - b1) / (double)(m1 - m2); // collision x
		double py = m1 * px + b1; // collision y

		// Calculate time
		double time1 = (px - hail1.x) / hail1.dx;
		double time2 = (px - hail2.x) / hail2.dx;

		// Use this instead? https://math.stackexchange.com/a/3176648

		return (true, (long)px, (long)py, time1, time2);
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		List<Hail> points = new();

		foreach (string line in input.AsLines())
		{
			var split = line.TrimSplit(["@", ","]);

			points.Add(new Hail(
				split[0].ToLong(),
				split[1].ToLong(),
				split[2].ToLong(),
				split[3].ToLong(),
				split[4].ToLong(),
				split[5].ToLong()));
		}

		result = SolvePart2(points);

		return result.ToString();
	}

	[DebuggerDisplay("x={x},y={y},z={z} @ dx={dx},dy={dy},dz={dz}")]
	public class Hail
	{

		public long x;
		public long y;
		public long z;

		public long dx;
		public long dy;
		public long dz;

		public Hail(long x, long y, long z, long dx, long dy, long dz)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.dx = dx;
			this.dy = dy;
			this.dz = dz;
		}

		public override bool Equals(object? obj)
		{
			return obj is Hail hail &&
						 x == hail.x &&
						 y == hail.y &&
						 z == hail.z &&
						 dx == hail.dx &&
						 dy == hail.dy &&
						 dz == hail.dz;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(x, y, z, dx, dy, dz);
		}

		public Hail OffsetXY(long offsetX, long offsetY)
		{
			return new Hail(x, y, z, dx + offsetX, dy + offsetY, dz);
		}
	}

	public long SolvePart2(List<Hail> Hails)
	{
		int searchOffset = 1000;

		// If first four align, all needs to align, otherwise we have no solution.
		// Check the first four hails.
		// If they match the others must also do, otherwise it is impossible to hit all with one rock.
		Hail one = Hails.ElementAt(0);
		Hail two = Hails.ElementAt(1);
		Hail three = Hails.ElementAt(2);
		Hail four = Hails.ElementAt(3);

		// Search for rock velocity X and Y
		for (int x = -searchOffset; x <= searchOffset; x++)
		{
			for (int y = -searchOffset; y <= searchOffset; y++)
			{
				// Modify their velocities to simulate hitting by a rock which has no velocity.(x,y).
				// A rock with velocity (3,1) hitting a hail with velocity (1,1) is the same as
				// A rock with velocity (0,0) hitting a hail with velocity (-2,0).
				var intersection1 = IntersectsLong(two.OffsetXY(x, y), one.OffsetXY(x, y));
				var intersection2 = IntersectsLong(three.OffsetXY(x, y), one.OffsetXY(x, y));
				var intersection3 = IntersectsLong(four.OffsetXY(x, y), one.OffsetXY(x, y));

				// No match, keep searching
				if (!intersection1.intersects)
				{
					continue;
				}
				if (
					intersection1.px != intersection2.px || intersection1.px != intersection3.px ||
					intersection1.py != intersection2.py || intersection1.py != intersection3.py)
				{
					continue;
				}

				for (int z = -searchOffset; z <= searchOffset; z++)
				{
					long intersection1z = (long)(two.z + intersection1.time1 * (two.dz + z));
					long intersection2z = (long)(three.z + intersection2.time1 * (three.dz + z));
					long intersection3z = (long)(four.z + intersection3.time1 * (four.dz + z));

					// No match, keep searching
					if (intersection1z != intersection2z || intersection1z != intersection3z)
					{
						continue;
					}

					return intersection1.px + intersection1.py + intersection1z;
				}
			}
		}

		throw new Exception("Error");
	}
}
