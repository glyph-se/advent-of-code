using Year2022.Common;

namespace Year2022.Day22
{
	internal static class Day22StringParsing
	{

		/// <summary>
		/// This parser has INVERTED x and y because it had this bug in it when I solved this day
		/// </summary>
		/// <typeparam name="TReturn"></typeparam>
		/// <param name="input"></param>
		/// <param name="constructor"></param>
		/// <returns></returns>
		public static TReturn[,] AsGridDay22<TReturn>(string input, Func<char, int, int, TReturn> constructor)
		{
			var lines = input.AsLines();

			TReturn[,] grid = new TReturn[lines.Count, lines[0].Length];

			for (int row = 0; row < lines.Count; row++)
			{
				string line = lines[row];
				for (int col = 0; col < line.Length; col++)
				{
					char c = line[col];
					grid[row, col] = constructor(c, row, col);
				}
			}

			return grid;
		}
	}
}

