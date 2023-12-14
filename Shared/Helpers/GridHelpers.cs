using System.Text;

namespace Shared.Helpers
{
	public static class GridHelpers
	{
		public static IReadOnlyDictionary<(int x, int y), TReturn> AsGridDict<TReturn>(this string input, Func<char, int, int, TReturn> constructor)
		{
			var lines = input.AsLines();

			Dictionary<(int x, int y), TReturn> grid = new();

			for (int row = 0; row < lines.Count; row++)
			{
				string line = lines[row];
				for (int col = 0; col < line.Length; col++)
				{
					char c = line[col];
					grid.Add((col, row), constructor(c, col, row));
				}
			}

			return grid.AsReadOnly();
		}

		public static TReturn[,] AsGridMatrix<TReturn>(this string input, Func<char, int, int, TReturn> constructor)
		{
			var lines = input.AsLines();

			TReturn[,] grid = new TReturn[lines[0].Length, lines.Count];

			for (int row = 0; row < lines.Count; row++)
			{
				string line = lines[row];
				for (int col = 0; col < line.Length; col++)
				{
					char c = line[col];
					grid[col, row] = constructor(c, col, row);
				}
			}

			return grid;
		}

		public static TReturn[,] ExtendGridMatrix<TReturn>(this TReturn[,] orgGrid, int extensionCount, Func<int, int, TReturn> extensionFunc) where TReturn : Point
		{
			TReturn[,] grid = new TReturn[orgGrid.GetLength(0) + 2 * extensionCount, orgGrid.GetLength(1) + 2 * extensionCount];

			for (int i = 0; i < orgGrid.GetLength(0); i++)
			{
				for (int j = 0; j < orgGrid.GetLength(1); j++)
				{
					var point = orgGrid[i, j];

					if (point != null)
					{
						grid[i + extensionCount, j + extensionCount] = point;
						point.x += extensionCount;
						point.y += extensionCount;
					}
				}
			}


			for (int i = 0; i < grid.GetLength(0); i++)
			{
				for (int extension = 0; extension < extensionCount; extension++)
				{
					grid[i, extension] = extensionFunc(i, extension);
					grid[i, grid.GetUpperBound(1) - extension] = extensionFunc(i, grid.GetUpperBound(1) - extension);
				}
			}

			for (int j = 0; j < grid.GetLength(1); j++)
			{
				for (int extension = 0; extension < extensionCount; extension++)
				{
					grid[extension, j] = extensionFunc(extension, j);
					grid[grid.GetUpperBound(0) - extension, j] = extensionFunc(grid.GetUpperBound(0) - extension, j);
				}
			}

			return grid;
		}

		public static TReturn[,] ExtendGridMatrixWithPoint<TReturn>(this TReturn[,] orgGrid, int extensionCount, Func<char, int, int, TReturn> extensionFunc) where TReturn : Point
		{
			return ExtendGridMatrix(orgGrid, extensionCount, (x,y) => extensionFunc('.', x,y));
		}

		public static TReturn?[,] ExtendGridMatrix<TReturn>(this TReturn?[,] orgGrid, int extensionCount) where TReturn : Point
		{
			TReturn?[,] grid = new TReturn[orgGrid.GetLength(0) + 2 * extensionCount, orgGrid.GetLength(1) + 2 * extensionCount];

			for (int i = 0; i < orgGrid.GetLength(0); i++)
			{
				for (int j = 0; j < orgGrid.GetLength(1); j++)
				{
					var point = orgGrid[i, j];

					if (point != null)
					{
						grid[i + extensionCount, j + extensionCount] = point;
						point.x += extensionCount;
						point.y += extensionCount;
					}
				}
			}

			return grid;
		}

		public static TReturn?[,] ExtendGridMatrix<TReturn>(this TReturn?[,] orgGrid, int extension) where TReturn : struct
		{
			TReturn?[,] grid = new TReturn?[orgGrid.GetLength(0) + 2 * extension, orgGrid.GetLength(1) + 2 * extension];

			for (int i = 0; i < orgGrid.GetLength(0); i++)
			{
				for (int j = 0; j < orgGrid.GetLength(1); j++)
				{
					grid[i + extension, j + extension] = orgGrid[i, j];
				}
			}

			return grid;
		}

		public static IReadOnlyList<TReturn> AsList<TReturn>(this TReturn[,] grid)
		{
			List<TReturn> list = new();

			foreach (var element in grid)
			{
				if (element != null)
				{
					list.Add(element);
				}
			}

			return list;
		}

		public static IReadOnlyList<TReturn> AsGridList<TReturn>(this string input, Func<char, int, int, TReturn> constructor)
		{
			var lines = input.AsLines();

			List<TReturn> list = new();
			for (int row = 0; row < lines.Count; row++)
			{
				string line = lines[row];
				for (int col = 0; col < line.Length; col++)
				{
					char c = line[col];
					list.Add(constructor(c, col, row));
				}
			}
			return list;
		}

		public static IEnumerable<(int dx, int dy)> AllDirs()
		{
			return Diagonals().Concat(UpDowns());
		}

		public static IEnumerable<(int dx, int dy)> Diagonals()
		{
			return new List<(int dx, int dy)>() { (1, -1), (1, 1), (-1, 1), (-1, -1) };
		}

		public static IEnumerable<(int dx, int dy)> UpDowns()
		{
			return new List<(int dx, int dy)>() { (0, 1), (1, 0), (-1, 0), (0, -1) };
		}

		public static void PrintGrid(this Point[,] grid, Func<Point, string> printFunc)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				for (int x = 0; x < grid.GetLength(0); x++)
				{
					Point current = grid[x, y];

					Console.Write(printFunc(current));
				}
				Console.WriteLine();
			}
		}

		public static void PrintGrid(this CharPoint[,] grid)
		{
			PrintGrid(grid, p => ((CharPoint)p).c.ToString());
		}

		/// <summary>
		/// Returns the column with number 'col' of this matrix as a 1D-Array.
		/// </summary>
		/// <remarks>
		/// From https://stackoverflow.com/a/38846559
		/// </remarks>
		public static T[] GetCol<T>(this T[,] matrix, int col)
		{
			var colLength = matrix.GetLength(0);
			var colVector = new T[colLength];

			for (var i = 0; i < colLength; i++)
				colVector[i] = matrix[i, col];

			return colVector;
		}

		/// <summary>
		/// Returns the row with number 'row' of this matrix as a 1D-Array.
		/// </summary>
		/// <remarks>
		/// From https://stackoverflow.com/a/38846559
		/// </remarks>
		public static T[] GetRow<T>(this T[,] matrix, int row)
		{
			var rowLength = matrix.GetLength(1);
			var rowVector = new T[rowLength];

			for (var i = 0; i < rowLength; i++)
				rowVector[i] = matrix[row, i];

			return rowVector;
		}

		/// <remarks>
		/// Taken from https://stackoverflow.com/a/18035050
		/// </remarks>
		public static T[,] RotateMatrixCounterClockwise<T>(this T[,] oldMatrix) where T : Point
		{
			T[,] newMatrix = new T[oldMatrix.GetLength(1), oldMatrix.GetLength(0)];
			int newColumn, newRow = 0;
			for (int oldColumn = oldMatrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
			{
				newColumn = 0;
				for (int oldRow = 0; oldRow < oldMatrix.GetLength(0); oldRow++)
				{
					newMatrix[newRow, newColumn] = oldMatrix[oldRow, oldColumn];
					newMatrix[newRow, newColumn].x = newRow;
					newMatrix[newRow, newColumn].y = newColumn;
					newColumn++;
				}
				newRow++;
			}
			return newMatrix;
		}

		/// <summary>
		/// Converts the grid to a string. Useful if you want it as a key in a dictionary or similar.
		/// </summary>
		public static string AsString(this CharPoint[,] grid)
		{
			StringBuilder sb = new StringBuilder();
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				for (int x = 0; x < grid.GetLength(0); x++)
				{
					CharPoint current = grid[x, y];

					sb.Append(current.c);
				}
				sb.AppendLine();
			}

			return sb.ToString();
		}
	}
}
