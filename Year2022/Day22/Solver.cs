using System.Diagnostics;
using AdventOfCode.Common;

namespace AdventOfCode.Year2022.Day22
{
    internal class Solver : ISolver
    {
        private readonly (int row, int col) RIGHT = (0, 1);
        private readonly (int row, int col) LEFT = (0, -1);
        private readonly (int row, int col) DOWN = (1, 0);
        private readonly (int row, int col) UP = (-1, 0);

        public async Task<string> PartOne(string input)
        {
            await Task.Yield();

            var split = input.AsLineBlocks();

            bool?[,] orgGrid = split[0].AsGrid<bool?>((c, x, y) => CreateNode(c, x, y));

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


            int result = 1000 * (player.pos.row) + 4 * (player.pos.col) + dirResult;

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

            int result = 0;



            return result.ToString();
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
        }
    }
}
