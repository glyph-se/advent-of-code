using System.Collections.Immutable;

namespace Year2025.Day05;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		var blocks = input.AsLineBlocks();

		ImmutableList<long> available = blocks[1].AsLongs();

		List<Range> freshRanges = new();

		foreach (string line in blocks[0].AsLines())
		{
			(string start, string end) = line.Split2("-");
			freshRanges.Add(new Range(start.ToLong(), end.ToLong()));
		}

		foreach (var avail in available)
		{
			if (freshRanges.Any(r => r.start <= avail && r.end >= avail))
			{
				result++;
			}
		}

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		var blocks = input.AsLineBlocks();

		List<Range> freshRanges = new();

		foreach (string line in blocks[0].AsLines())
		{
			(string start, string end) = line.Split2("-");
			freshRanges.Add(new Range(start.ToLong(), end.ToLong()));
		}

		var mergedRanges = MergeOverlappingRanges(freshRanges);

		result = mergedRanges.Sum(r => r.length);

		return result.ToString();
	}

	private static IEnumerable<Range> MergeOverlappingRanges(IEnumerable<Range> ranges)
	{
		var sorted = ranges.OrderBy(r => r.start).ToList();

		List<Range> merged = new List<Range>();

		Range current = sorted.First();

		foreach (var next in sorted.Skip(1))
		{
			if (next.start <= current.end)
			{
				current.end = Math.Max(current.end, next.end);
			}
			else
			{
				merged.Add(current);
				current = next;
			}
		}

		merged.Add(current);

		return merged;
	}

	public class Range
	{
		public Range(long start, long end)
		{
			this.start = start;
			this.end = end;
		}
		public long start;

		public long end;

		public long length => end - start + 1;
	}
}
