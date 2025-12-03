using Shared;

namespace Year2023.Day12;

public class Solver : ISolver
{
	private Dictionary<DpKey, long> DpCache = new Dictionary<DpKey, long>();

	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		foreach (string line in input.AsLines())
		{
			(string a, string b) = line.Split2(" ");
			var springs = a.ToCharArray().ToList(); ;
			var groups = b.TrimSplit(",").Select(c => c.ToInt()).ToList();

			result += Recurse(springs, groups, 0, 0, 0);
			DpCache.Clear();
		}

		return result.ToString();
	}

	/// <summary>
	/// OK is .
	/// DMG is #
	/// </summary>
	public long Recurse(IList<char> springs, IList<int> groups, int springPos, int groupPos, int currentGroupLength)
	{
		DpKey key = (springPos, groupPos, currentGroupLength);
		if (DpCache.ContainsKey(key))
		{
			return DpCache[key];
		}

		if (springPos == springs.Count)
		{
			if (groupPos == groups.Count && currentGroupLength == 0)
			{
				return 1;
			}

			if (groupPos == groups.Count - 1 && groups[groupPos] == currentGroupLength)
			{
				return 1;
			}
			return 0;
		}

		long placements = 0;
		char currentSpring = springs[springPos];

		if (currentSpring == '.' || currentSpring == '?')
		{
			if (currentGroupLength == 0)
			{
				placements += Recurse(springs, groups, springPos + 1, groupPos, 0);
			}
			else if (currentGroupLength > 0 && groupPos < groups.Count && groups[groupPos] == currentGroupLength)
			{
				placements += Recurse(springs, groups, springPos + 1, groupPos + 1, 0);
			}
		}

		if (currentSpring == '#' || currentSpring == '?')
		{
			placements += Recurse(springs, groups, springPos + 1, groupPos, currentGroupLength + 1);
		}

		DpCache.Add(key, placements);

		return placements;
	}


	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		foreach (string line in input.AsLines())
		{
			(string a, string b) = line.Split2(" ");
			a = string.Join("?", [a, a, a, a, a]);
			b = string.Join(",", [b, b, b, b, b]);

			var springs = a.ToCharArray().ToList(); ;
			var groups = b.TrimSplit(",").Select(c => c.ToInt()).ToList();

			result += Recurse(springs, groups, 0, 0, 0);
			DpCache.Clear();
		}

		return result.ToString();
	}
}

internal record struct DpKey(int springPos, int groupPos, int currentGroupLength)
{
	public static implicit operator (int springPos, int groupPos, int currentGroupLength)(DpKey value)
	{
		return (value.springPos, value.groupPos, value.currentGroupLength);
	}

	public static implicit operator DpKey((int springPos, int groupPos, int currentGroupLength) value)
	{
		return new DpKey(value.springPos, value.groupPos, value.currentGroupLength);
	}
}