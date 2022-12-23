namespace AdventOfCode.Common
{
    internal static class GridHelpers
    {


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
                    grid[col, row] = constructor(c, row, col);
                }
            }

            return grid;
        }

        public static TReturn[,] ExtendGridMatrix<TReturn>(this TReturn[,] orgGrid, int extension) where TReturn : Point
        {
            TReturn[,] grid = new TReturn[orgGrid.GetLength(0) + 2 * extension, orgGrid.GetLength(1) + 2 * extension];

            for (int i = 0; i < orgGrid.GetLength(0); i++)
            {
                for (int j = 0; j < orgGrid.GetLength(1); j++)
                {
                    var point = orgGrid[i, j];

                    if (point != null)
                    {
                        grid[i + extension, j + extension] = point;
                        point.x += extension;
                        point.y += extension;
                    }
                }
            }

            return grid;
        }

        public static Nullable<TReturn>[,] ExtendNullableGridMatrix<TReturn>(this Nullable<TReturn>[,] orgGrid, int extension) where TReturn : struct
        {
            Nullable<TReturn>[,] grid = new Nullable<TReturn>[orgGrid.GetLength(0) + 2 * extension, orgGrid.GetLength(1) + 2 * extension];

            for (int i = 0; i < orgGrid.GetLength(0); i++)
            {
                for (int j = 0; j < orgGrid.GetLength(1); j++)
                {
                    grid[i + extension, j + extension] = orgGrid[i, j];
                }
            }

            return grid;
        }

        public static IReadOnlyList<TReturn> AsList2<TReturn>(this TReturn[,] grid)
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
    }
}
