using System.Diagnostics;
using Shared;
using Year2022.Common;

namespace Year2022.Day22
{
	public class Solver : ISolver
	{
		private readonly (int row, int col) RIGHT = (0, 1);
		private readonly (int row, int col) LEFT = (0, -1);
		private readonly (int row, int col) DOWN = (1, 0);
		private readonly (int row, int col) UP = (-1, 0);

		public async Task<string> PartOne(string input)
		{
			await Task.Yield();

			var split = input.AsLineBlocks();

			bool?[,] orgGrid = Day22StringParsing.AsGridDay22(split[0], (c, x, y) => CreateNode(c, x, y));

			bool?[,] grid = orgGrid.ExtendNullableGridMatrix(1);

			List<string> movements = new();

			foreach (var r in split[1].Split('R'))
			{
				foreach (var l in r.Split('L'))
				{
					movements.Add(l);
					movements.Add("L");
				}

				movements.RemoveAt(movements.Count - 1);

				movements.Add("R");
			}
			movements.RemoveAt(movements.Count - 1);

			Player player = new();
			player.dir = RIGHT;
			player.pos = (1, 1);

			// Go to first space that exists
			MoveFromVoid(grid, player);

			foreach (string movement in movements)
			{
				if (int.TryParse(movement, out int steps))
				{
					for (int step = 1; step <= steps; step++)
					{
						Player newPos = new();
						newPos.dir = player.dir;
						newPos.pos = player.pos;

						newPos.pos.row += newPos.dir.row;
						newPos.pos.col += newPos.dir.col;


						if (grid[newPos.pos.row, newPos.pos.col] == true)
						{
							// Here is wall, stop
							break;
						}

						if (grid[newPos.pos.row, newPos.pos.col] == null)
						{
							// Here is void, start over at other side
							if (player.dir == DOWN)
							{
								newPos.pos.row = 0;
							}
							if (player.dir == RIGHT)
							{
								newPos.pos.col = 0;
							}
							if (player.dir == UP)
							{
								newPos.pos.row = grid.GetLength(0) - 1;
							}
							if (player.dir == LEFT)
							{
								newPos.pos.col = grid.GetLength(1) - 1;
							}

							MoveFromVoid(grid, newPos);

							if (grid[newPos.pos.row, newPos.pos.col] == true)
							{
								// Here is wall, stop
								break;
							}
						}

						player.pos = newPos.pos;
					}
				}
				else if (movement == "R")
				{
					if (player.dir == RIGHT)
					{
						player.dir = DOWN;
					}
					else if (player.dir == DOWN)
					{
						player.dir = LEFT;
					}
					else if (player.dir == LEFT)
					{
						player.dir = UP;
					}
					else if (player.dir == UP)
					{
						player.dir = RIGHT;
					}
				}
				else if (movement == "L")
				{
					if (player.dir == RIGHT)
					{
						player.dir = UP;
					}
					else if (player.dir == UP)
					{
						player.dir = LEFT;
					}
					else if (player.dir == LEFT)
					{
						player.dir = DOWN;
					}
					else if (player.dir == DOWN)
					{
						player.dir = RIGHT;
					}
				}

			}

			int dirResult = 0;
			if (player.dir == DOWN)
			{
				dirResult = 1;
			}
			if (player.dir == LEFT)
			{
				dirResult = 2;
			}
			if (player.dir == UP)
			{
				dirResult = 3;
			}


			int result = 1000 * player.pos.row + 4 * player.pos.col + dirResult;

			return result.ToString();

			static void MoveFromVoid(bool?[,] grid, Player player)
			{
				while (true)
				{
					if (grid[player.pos.row, player.pos.col] != null)
					{
						break;
					}

					player.pos.row += player.dir.row;
					player.pos.col += player.dir.col;
				}
			}
		}

		public async Task<string> PartTwo(string input)
		{
			await Task.Yield();

			if (input.Length != 35848)
			{
				return "only implemented for full";
			}

			var split = input.AsLineBlocks();

			bool?[,] orgGrid = Day22StringParsing.AsGridDay22(split[0], (c, x, y) => CreateNode(c, x, y));

			bool?[,] grid = new bool?[orgGrid.GetLength(0) + 2, orgGrid.GetLength(1) + 2];

			for (int i = 1; i < grid.GetLength(0) - 1; i++)
			{
				for (int j = 1; j < grid.GetLength(1) - 1; j++)
				{
					grid[i, j] = orgGrid[i - 1, j - 1];
				}
			}

			List<string> movements = new();

			foreach (var r in split[1].Split('R'))
			{
				foreach (var l in r.Split('L'))
				{
					movements.Add(l);
					movements.Add("L");
				}

				movements.RemoveAt(movements.Count - 1);

				movements.Add("R");
			}
			movements.RemoveAt(movements.Count - 1);

			Player player = new();
			player.dir = RIGHT;
			player.pos = (1, 1);

			// Go to first space that exists
			MoveFromVoid(grid, player);

			foreach (string movement in movements)
			{
				if (int.TryParse(movement, out int steps))
				{
					for (int step = 1; step <= steps; step++)
					{
						Player newPos = new();
						newPos.dir = player.dir;
						newPos.pos = player.pos;

						// Move forward one step
						newPos.pos.row += newPos.dir.row;
						newPos.pos.col += newPos.dir.col;


						if (grid[newPos.pos.row, newPos.pos.col] == true)
						{
							// Here is wall, stop at previous
							break;
						}

						if (grid[newPos.pos.row, newPos.pos.col] == null)
						{
							// We got into void, wrap around
							//Console.WriteLine($"This move leads to void {player}");
							newPos = WrapAround2(grid, player);
							//Console.WriteLine($"We are at {newPos} after wrap");

							//MoveFromVoid(grid, newPos);

							if (grid[newPos.pos.row, newPos.pos.col] == true)
							{
								// Here is wall, stop at previous
								break;
							}
						}

						player.pos = newPos.pos;
						player.dir = newPos.dir;
					}
				}
				else if (movement == "R")
				{
					if (player.dir == RIGHT)
					{
						player.dir = DOWN;
					}
					else if (player.dir == DOWN)
					{
						player.dir = LEFT;
					}
					else if (player.dir == LEFT)
					{
						player.dir = UP;
					}
					else if (player.dir == UP)
					{
						player.dir = RIGHT;
					}
				}
				else if (movement == "L")
				{
					if (player.dir == RIGHT)
					{
						player.dir = UP;
					}
					else if (player.dir == UP)
					{
						player.dir = LEFT;
					}
					else if (player.dir == LEFT)
					{
						player.dir = DOWN;
					}
					else if (player.dir == DOWN)
					{
						player.dir = RIGHT;
					}
				}

			}

			int dirResult = 0;
			if (player.dir == DOWN)
			{
				dirResult = 1;
			}
			if (player.dir == LEFT)
			{
				dirResult = 2;
			}
			if (player.dir == UP)
			{
				dirResult = 3;
			}


			int result = 1000 * player.pos.row + 4 * player.pos.col + dirResult;

			return result.ToString();

			static void MoveFromVoid(bool?[,] grid, Player player)
			{
				while (true)
				{
					if (grid[player.pos.row, player.pos.col] != null)
					{
						break;
					}

					player.pos.row += player.dir.row;
					player.pos.col += player.dir.col;
				}
			}

			Player WrapAround2(bool?[,] grid, Player player)
			{
				// Set variables for input to algorithm
				int x = player.pos.col;
				int y = player.pos.row;

				int? newX = null;
				int? newY = null;
				(int dx, int dy)? newDir = null;

				if (player.dir == RIGHT)
				{
					if (x == 150)
					{
						newX = 100;
						newY = 151 - y;
						newDir = (-1, 0);
					}
					if (x == 100)
					{
						if (51 <= y && y <= 100)
						{
							newX = 100 + (y - 50);
							newY = 50;
							newDir = (0, -1);
						}
						if (101 <= y && y <= 150)
						{
							newX = 150;
							newY = 51 - (y - 100);
							newDir = (-1, 0);
						}
					}
					if (x == 50)
					{
						newX = 50 + (y - 150);
						newY = 150;
						newDir = (0, -1);
					}
				}

				if (player.dir == LEFT)
				{
					if (x == 51)
					{
						if (1 <= y && y <= 50)
						{
							newX = 1;
							newY = 151 - y;
							newDir = (1, 0);
						}
						if (51 <= y && y <= 100)
						{
							newX = y - 50;
							newY = 101;
							newDir = (0, 1);
						}
					}
					if (x == 1)
					{
						if (101 <= y && y <= 150)
						{
							newX = 51;
							newY = 1 + (150 - y);
							newDir = (1, 0);
						}
						if (151 <= y && y <= 200)
						{
							newX = y - 150 + 50;
							newY = 1;
							newDir = (0, 1);
						}
					}
				}
				if (player.dir == DOWN)
				{
					if (y == 50)
					{
						newX = 100;
						newY = x - 50;
						newDir = (-1, 0);
					}
					if (y == 150)
					{
						newX = 50;
						newY = x + 100;
						newDir = (-1, 0);
					}
					if (y == 200)
					{
						newX = x + 100;
						newY = 1;
						newDir = (0, 1);
					}
				}
				if (player.dir == UP)
				{
					if (y == 1)
					{
						if (51 <= x && x <= 100)
						{
							newX = 1;
							newY = x + 100;
							newDir = (1, 0);
						}
						if (101 <= x && x <= 150)
						{
							newX = x - 100;
							newY = 200;
							newDir = (0, -1);
						}
					}
					if (y == 101)
					{
						newX = 51;
						newY = x + 50;
						newDir = (1, 0);
					}
				}

				Player wrapped = new();
				wrapped.pos.col = newX!.Value;
				wrapped.pos.row = newY!.Value;
				wrapped.dir.col = newDir!.Value.dx;
				wrapped.dir.row = newDir!.Value.dy;

				return wrapped;
			}
		}

		private static bool? CreateNode(char c, int row, int col)
		{
			if (c == '#')
			{
				return true;
			}
			if (c == ' ')
			{
				return null;
			}

			return false;
		}


		[DebuggerDisplay("pos={pos}, dir={dir}")]
		private class Player
		{
			public (int row, int col) pos;
			public (int row, int col) dir;

			public override string ToString()
			{
				return $"pos X = {pos.col}, pos Y = {pos.row}, dir X = {dir.col}, dir Y = {dir.row}";
			}
		}


	}
}