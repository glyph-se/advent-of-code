using Shared;

namespace Year2023.Day13;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		foreach (string blocks in input.AsLineBlocks())
		{
			var a = blocks.AsLines();

			result += FindMirror(a, 0) * 100;
		}

		foreach (string blocks in input.AsLineBlocks())
		{
			var a = blocks
				.AsLines()
				.Select(b => b.ToCharArray());

			List<string> transposed = a.Transpose()
				.Select(c => string.Concat(c))
				.ToList();

			result += FindMirror(transposed, 0);
		}

		return result.ToString();
	}

	public int FindMirror(IList<string> pattern, int smudges)
	{
		for (int tryMirror = 1; tryMirror < pattern.Count; tryMirror++)
		{
			int foundSmudges = 0;

			var afterMirror = pattern.Skip(tryMirror).ToList();
			var beforeMirror = pattern.Take(tryMirror).Reverse().ToList();

			for (int i = 0; i < Math.Min(beforeMirror.Count, afterMirror.Count); i++)
			{
				string s1 = beforeMirror[i];
				string s2 = afterMirror[i];
				for (int pos = 0; pos < s1.Length; pos++)
				{
					if (s1[pos] != s2[pos])
					{
						foundSmudges++;
					}
				}
			}
			if (foundSmudges == smudges)
			{
				return tryMirror;
			}
		}

		// No mirror found
		return 0;
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		foreach (string blocks in input.AsLineBlocks())
		{
			var a = blocks.AsLines();

			result += FindMirror(a, 1) * 100;
		}

		foreach (string blocks in input.AsLineBlocks())
		{
			var a = blocks
				.AsLines()
				.Select(b => b.ToCharArray());

			List<string> transposed = a.Transpose()
				.Select(c => string.Concat(c))
				.ToList();

			result += FindMirror(transposed, 1);
		}

		return result.ToString();
	}
}
