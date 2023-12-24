using System.Diagnostics;
using System.Linq;
using Shared;
using Shared.Helpers;
using static Year2023.Day24.Solver;

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

			points.Add(new Hail(split[0].ToLong(), split[1].ToLong(), split[2].ToLong(), split[3].ToLong(), split[4].ToLong(), split[5].ToLong()));
		}

		// https://stackoverflow.com/a/4543530

		IEnumerable<IEnumerable<Hail>> all = points.DifferentCombinations(2);

		
		long min = 200000000000000;
		long max = 400000000000000;
		
		/*
		long min = 7;
		long max = 27;
		*/


		foreach (var pair in all)
		{
			var hail1 = pair.ElementAt(0);
			var hail2 = pair.ElementAt(1);

			/*
			 * https://stackoverflow.com/a/4543530
			double a1 = hail1.dx;
			double b1 = hail1.dy;
			double c1 = (hail1.x * hail1.dx + hail1.y * hail1.dy);

			double a2 = hail2.dx;
			double b2 = hail2.dy;
			double c2 = (hail2.x * hail2.dx + hail2.y * hail2.dy);

			double delta = a1 * b2 - a2 *b1;

			if(delta == 0)
			{
				continue;
			}

			double x = (b2 * c1 - b1 * c2) / delta;
			double y = (a1 * c2 - a2 * c1) / delta;

			if (x > min && x < max && y > min && y < max) { result++; }*/

			// taken from https://stackoverflow.com/a/27474217

			(long x, long y) p1End = (hail1.x + hail1.dx, hail1.y + hail1.dy); // another point in line p1->n1
			(long x, long y) p2End = (hail2.x + hail2.dx, hail2.y + hail2.dy); // another point in line p2->n2

			double m1 = (double)(p1End.y - hail1.y) / (double)(p1End.x - hail1.x); // slope of line p1->n1
			double m2 = (double)(p2End.y - hail2.y) / (double)(p2End.x - hail2.x); // slope of line p2->n2

			double b1 = hail1.y - m1 * hail1.x; // y-intercept of line p1->n1
			double b2 = hail2.y - m2 * hail2.x; // y-intercept of line p2->n2

			double px = (double)(b2 - b1) / (double)(m1 - m2); // collision x
			double py = m1 * px + b1; // collision y

			// return statement
			if (px > min && px < max && py > min && py < max)
			{
				// Check for future or past
				if(hail1.dx > 0 && px < hail1.x)
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

				result++;
				
				
			}
		}

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

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
	}
}
