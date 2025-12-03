using System.Collections.Immutable;
using Shared;

namespace Year2021.Day04;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		ImmutableList<string> blocks = input.AsLineBlocks();

		IEnumerable<int> numbersCalled = blocks
			.First()
			.Split(",")
			.Select(s => int.Parse(s));

		List<BingoBoard> boards = new List<BingoBoard>();

		foreach (string b in blocks.Skip(1))
		{
			var lines = b.AsLines().ToArray();

			int[,] boardBuilding = new int[5, 5];

			for (int i = 0; i < 5; i++)
			{
				var numbers = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);

				for (int j = 0; j < 5; j++)
				{
					boardBuilding[i, j] = int.Parse(numbers[j]);
				}
			}

			boards.Add(new BingoBoard(boardBuilding));
		}

		foreach (int number in numbersCalled)
		{
			foreach (BingoBoard board in boards)
			{
				for (int i = 0; i < 5; i++)
				{
					for (int j = 0; j < 5; j++)
					{
						if (board.Board[i, j] == number)
						{
							board.Board[i, j] = -number;
						}
					}
				}

				if (board.IsWon())
				{
					return (board.Sum() * number).ToString();
				}
			}


		}

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		ImmutableList<string> blocks = input.AsLineBlocks();

		IEnumerable<int> numbersCalled = blocks
			.First()
			.Split(",")
			.Select(s => int.Parse(s));

		List<BingoBoard> boards = new List<BingoBoard>();

		foreach (string b in blocks.Skip(1))
		{
			var lines = b.AsLines().ToArray();

			int[,] boardBuilding = new int[5, 5];

			for (int i = 0; i < 5; i++)
			{
				var numbers = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);

				for (int j = 0; j < 5; j++)
				{
					boardBuilding[i, j] = int.Parse(numbers[j]);
				}
			}

			boards.Add(new BingoBoard(boardBuilding));
		}

		foreach (int number in numbersCalled)
		{
			foreach (BingoBoard board in boards)
			{
				for (int i = 0; i < 5; i++)
				{
					for (int j = 0; j < 5; j++)
					{
						if (board.Board[i, j] == number)
						{
							board.Board[i, j] = int.MinValue;
						}
					}
				}

				if (boards.All(b => b.IsWon()))
				{
					var winner = board;
					return (winner.Sum() * number).ToString();
				}
			}


		}

		return result.ToString();
	}

	public class BingoBoard(int[,] board)
	{
		public int[,] Board { get; } = board;

		public int Sum()
		{
			int result = 0;
			for (int i = 0; i <= Board.GetUpperBound(0); i++)
			{
				for (int j = 0; j <= Board.GetUpperBound(1); j++)
				{
					int v = Board[i, j];
					if (v > 0)
					{
						result += v;
					}
				}
			}

			return result;
		}

		public bool IsWon()
		{
			// Check rows
			for (int i = 0; i <= Board.GetUpperBound(0); i++)
			{
				var row = Enumerable.Range(0, Board.GetLength(0))
					.Select(x => Board[x, i])
					.ToArray();

				if (row.All(x => x < 0))
				{
					return true;
				}
			}

			// Check cols
			for (int i = 0; i <= Board.GetUpperBound(1); i++)
			{
				var col = Enumerable.Range(0, Board.GetLength(1))
					.Select(x => Board[i, x])
					.ToArray();

				if (col.All(x => x < 0))
				{
					return true;
				}
			}

			return false;
		}
	}
}