using Shared;
using Shared.Helpers;

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

			for(int tryMirror = 0; tryMirror < a.Count-1; tryMirror++)
			{
				bool isMirror = true;
				for (int i = 0; i < a.Count/2; i++)
				{
					try
					{
						if (a[tryMirror - i] != a[tryMirror + i + 1])
						{
							isMirror = false;
							break;
						}
					}
					catch(ArgumentOutOfRangeException) { }
				}

				if (isMirror)
				{
					result += (tryMirror+1)*100;
				}
			}
		}

		foreach (string blocks in input.AsLineBlocks())
		{
			IEnumerable<IEnumerable<char>> a = blocks.AsLines().Select(b => b.ToCharArray().AsEnumerable<char>());

			var b = a.Transpose().ToList();

			for (int tryMirror = 0; tryMirror < b.Count - 1; tryMirror++)
			{
				bool isMirror = true;
				for (int i = 0; i < b.Count/2; i++)
				{
					try
					{
						string s1 = string.Join("", b.ElementAt(tryMirror - i));
						string s2 = string.Join("", b.ElementAt(tryMirror + i + 1));
						if (s1 != s2)
						{
							isMirror = false;
							break;
						}
					}
					catch (ArgumentOutOfRangeException) { }
				}

				if (isMirror)
				{
					result += (tryMirror + 1);
				}
			}
		}

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		Levenshtein levenshtein = new Levenshtein();
		await Task.Yield();

		long result = 0;

		foreach (string blocks in input.AsLineBlocks())
		{
			var a = blocks.AsLines();

			for (int tryMirror = 0; tryMirror < a.Count - 1; tryMirror++)
			{
				bool isMirror = true;
				int smudges = 0;
				for (int i = 0; i < a.Count / 2; i++)
				{
					try
					{
						var s1 = a[tryMirror - i];
						var s2 = a[tryMirror + i + 1];
						for(int j = 0; j< s1.Length; j++)
						{
							if (s1[j] != s2[j])
							{
								smudges++;
							}
						}
					}
					catch (ArgumentOutOfRangeException) { }
				}
				if(smudges == 1)
				{
					result += (tryMirror + 1) * 100;
				}
			}
		}

		foreach (string blocks in input.AsLineBlocks())
		{
			IEnumerable<IEnumerable<char>> a = blocks.AsLines().Select(b => b.ToCharArray().AsEnumerable<char>());

			var b = a.Transpose().ToList();

			for (int tryMirror = 0; tryMirror < b.Count - 1; tryMirror++)
			{
				bool isMirror = true;
				int smudges = 0;
				for (int i = 0; i < b.Count / 2; i++)
				{
					try
					{
						string s1 = string.Join("", b.ElementAt(tryMirror - i));
						string s2 = string.Join("", b.ElementAt(tryMirror + i + 1));

						for (int j = 0; j < s1.Length; j++)
						{
							if (s1[j] != s2[j])
							{
								smudges++;
							}
						}
					}
					catch (ArgumentOutOfRangeException) { }
				}

				if (smudges == 1)
				{
					result += (tryMirror + 1);
				}
			}
		}

		return result.ToString();
	}
}
