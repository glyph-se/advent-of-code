namespace Year2022.Day08;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		int result = 0;

		var rowStrings = input.ParseLines();

		List<List<Tree>> rowTrees = new();

		foreach (string rowInt in rowStrings)
		{
			List<Tree> rowTree = new List<Tree>();
			foreach (char c in rowInt)
			{
				Tree t = new();
				t.height = int.Parse(c.ToString());
				rowTree.Add(t);
			}
			rowTrees.Add(rowTree);
		}
		var colTrees = rowTrees.Transpose();

		foreach (Tree t in rowTrees.First())
		{
			t.seen = true;
		}
		foreach (Tree t in rowTrees.Last())
		{
			t.seen = true;
		}
		foreach (Tree t in colTrees.First())
		{
			t.seen = true;
		}
		foreach (Tree t in colTrees.Last())
		{
			t.seen = true;
		}

		foreach (IEnumerable<Tree> row in rowTrees)
		{
			Tree prev = new()
			{
				height = -1
			};

			foreach (var curent in row)
			{
				if (curent.height > prev.height)
				{
					curent.seen = true;
					prev = curent;
				}
			}

			prev = new()
			{
				height = -1
			};
			foreach (var curent in row.Reverse())
			{
				if (curent.height > prev.height)
				{
					curent.seen = true;
					prev = curent;
				}
			}
		}

		foreach (IEnumerable<Tree> col in colTrees)
		{
			Tree prev = new()
			{
				height = -1
			};

			foreach (var curent in col)
			{
				if (curent.height > prev.height)
				{
					curent.seen = true;
					prev = curent;
				}
			}

			prev = new()
			{
				height = -1
			};
			foreach (var curent in col.Reverse())
			{
				if (curent.height > prev.height)
				{
					curent.seen = true;
					prev = curent;
				}
			}
		}


		foreach (var row in rowTrees)
		{
			result += row.Count(t => t.seen);
		}

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		int result = 0;

		var rowStrings = input.ParseLines();

		List<List<Tree>> rowTrees = new();

		foreach (string rowInt in rowStrings)
		{
			List<Tree> rowTree = new List<Tree>();
			foreach (char c in rowInt)
			{
				Tree t = new();
				t.height = int.Parse(c.ToString());
				rowTree.Add(t);
			}
			rowTrees.Add(rowTree);
		}
		var colTrees = rowTrees.Transpose().Select(e => e.ToList()).ToList();

		foreach (List<Tree> row in rowTrees)
		{
			for (int i = 1; i < row.Count - 1; i++)
			{
				Tree current = row[i];

				for (int j = i - 1; j >= 0; j--)
				{
					Tree check = row[j];

					if (current.height > check.height)
					{
						current.leftScore++;
					}
					else
					{
						current.leftScore++;
						break;
					}
				}

				for (int j = i + 1; j < row.Count; j++)
				{
					Tree check = row[j];
					if (current.height > check.height)
					{
						current.rightScore++;
					}
					else
					{
						current.rightScore++;
						break;
					}
				}
			}
		}

		foreach (List<Tree> col in colTrees)
		{
			for (int i = 1; i < col.Count - 1; i++)
			{
				Tree current = col[i];

				for (int j = i - 1; j >= 0; j--)
				{
					Tree check = col[j];
					if (current.height > check.height)
					{
						current.upScore++;
					}
					else
					{
						current.upScore++;
						break;
					}
				}

				for (int j = i + 1; j < col.Count; j++)
				{
					Tree check = col[j];
					if (current.height > check.height)
					{
						current.downScore++;
					}
					else
					{
						current.downScore++;
						break;
					}
				}
			}
		}

		int topScore = 0;

		foreach (var row in rowTrees)
		{
			foreach (Tree t in row)
			{
				int score = t.leftScore * t.rightScore * t.upScore * t.downScore;
				if (score > topScore)
				{
					topScore = score;
				}
			}
		}

		result = topScore;

		return result.ToString();
	}

	public class Tree
	{
		public int height = 0;
		public int leftScore = 0;
		public int rightScore = 0;
		public int upScore = 0;
		public int downScore = 0;
		public bool seen = false;
	}
}
