
using System.Diagnostics;

namespace Year2025.Day08;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		List<Point3D> points = ParsePoints(input);
		PriorityQueue<(Point3D, Point3D), long> pairDistances = CalculcatePairDistances(points);

		// HACK: In example we make 10 connections, in full data we make 1000
		int connectionsToMake = points.Count == 20 ? 10 : 1000;

		Dictionary<Point3D, int> networkIds = new();
		int freeId = 0;

		for (int i = 0; i < connectionsToMake; i++)
		{
			(Point3D p1, Point3D p2) = pairDistances.Dequeue();

			ConnectPoints(p1, p2, networkIds, ref freeId);
		}

		// Count networks
		Dictionary<int, int> networkPointCount = new();
		foreach (KeyValuePair<Point3D, int> kvp in networkIds)
		{
			if (!networkPointCount.ContainsKey(kvp.Value))
			{
				networkPointCount.Add(kvp.Value, 0);
			}

			networkPointCount[kvp.Value]++;
		}

		var sortedCount = networkPointCount.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Value).ToArray();

		result = sortedCount[0] * sortedCount[1] * sortedCount[2];

		return result.ToString();
	}

	

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		List<Point3D> points = ParsePoints(input);
		PriorityQueue<(Point3D, Point3D), long> pairDistances = CalculcatePairDistances(points);

		Dictionary<Point3D, int> networkIds = new();
		int freeId = 0;

		while (pairDistances.Count > 0)
		{
			(Point3D p1, Point3D p2) = pairDistances.Dequeue();

			ConnectPoints(p1, p2, networkIds, ref freeId);

			if (networkIds.Count == points.Count && networkIds.Values.Distinct().Count() == 1)
			{
				result = p1.x * p2.x;
				break;
			}
		}

		return result.ToString();
	}

	private static List<Point3D> ParsePoints(string input)
	{
		List<Point3D>  points = new();
		foreach (string line in input.AsLines())
		{
			(string x, string y, string z) = line.Split3(",");
			points.Add(new Point3D(x.ToInt(), y.ToInt(), z.ToInt()));
		}

		return points;
	}

	private static PriorityQueue<(Point3D, Point3D), long> CalculcatePairDistances(List<Point3D> points)
	{
		PriorityQueue<(Point3D, Point3D), long> pairDistances = new();
		foreach (IEnumerable<Point3D> pair in points.DifferentCombinations(2))
		{
			Point3D p1 = pair.First();
			Point3D p2 = pair.Skip(1).First();

			long dx = p1.x - p2.x;
			long dy = p1.y - p2.y;
			long dz = p1.z - p2.z;

			long dist = dx * dx + dy * dy + dz * dz;

			pairDistances.Enqueue((p1, p2), dist);
		}

		return pairDistances;
	}

	private static void ConnectPoints(Point3D p1, Point3D p2, Dictionary<Point3D, int> networkIds, ref int freeId)
	{
		if (!networkIds.ContainsKey(p1) && !networkIds.ContainsKey(p2))
		{
			networkIds.Add(p1, freeId);
			networkIds.Add(p2, freeId);
			freeId++;
		}
		else if (networkIds.ContainsKey(p1) && networkIds.ContainsKey(p2))
		{
			if (networkIds[p1] == networkIds[p2])
			{
				return;
			}

			int oldId = networkIds[p2];
			int newId = networkIds[p1];
			var pointsToUpdate = networkIds.Where(kvp => kvp.Value == oldId).Select(kvp => kvp.Key);

			foreach (Point3D p in pointsToUpdate)
			{
				networkIds[p] = newId;
			}
		}
		else if (networkIds.ContainsKey(p1))
		{
			networkIds[p2] = networkIds[p1];
		}
		else if (networkIds.ContainsKey(p2))
		{
			networkIds[p1] = networkIds[p2];
		}
		else
		{
			Debug.Fail("Should be unreachable");
		}
	}
}
