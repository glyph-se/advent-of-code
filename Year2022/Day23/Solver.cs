using AdventOfCode.Common;

namespace AdventOfCode.Year2022.Day23
{
    internal class Solver : ISolver
    {
        private List<(int dx, int dy)> allDirs = new()
        {
            (1,1),
            (-1,-1),
            (1,0),
            (-1,0),
            (0,1),
            (0,-1),
            (1,-1),
            (-1,1),
        };



        public async Task<string> PartOne(string input)
        {
            List<Direction> proposalOrder = new() { Direction.N, Direction.S, Direction.W, Direction.E };

            await Task.Yield();

            long result = 0;


            Elf[,] orgGrid = input.AsGridMatrix(((c, x, y) => CreateElf(c, x, y)));

            var allElves = orgGrid.AsList();

            Elf[,] grid = orgGrid.ExtendGridMatrix(1000);

            for (int round = 1; round <= 10; round++)
            {
                Dictionary<(int x, int y), List<Elf>> proposalsForThisRound = new();

                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        Elf elf = grid[x, y];

                        if (elf != null)
                        {
                            bool allEmpty = allDirs.All(d => grid[x + d.dx, y + d.dy] == null);

                            if (allEmpty == true)
                            {
                                elf.proposal = Direction.None;
                                continue;
                            }

                            foreach (Direction dir in proposalOrder)
                            {
                                bool dirEmpty = DirectionToCoords(dir).All(d => grid[x + d.dx, y + d.dy] == null);

                                if (dirEmpty)
                                {
                                    elf.proposal = dir;

                                    var key = (x + DirectionToCoord(dir).dx, y + DirectionToCoord(dir).dy);

                                    if (proposalsForThisRound.TryGetValue(key, out List<Elf> list))
                                    {
                                        list.Add(elf);
                                    }
                                    else
                                    {
                                        proposalsForThisRound.Add(key, new List<Elf>() { elf });
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }


                foreach (List<Elf> elves in proposalsForThisRound.Values)
                {
                    if (elves.Count == 1)
                    {
                        Elf elf = elves.Single();

                        // MOVE

                        grid[elf.x, elf.y] = null!;
                        elf.x = elf.ProposalCoord().x;
                        elf.y = elf.ProposalCoord().y;
                        grid[elf.x, elf.y] = elf;
                    }

                    foreach (Elf e in elves)
                    {
                        e.proposal = Direction.None;
                    }
                }

                var firstProposal = proposalOrder.First();
                proposalOrder.RemoveAt(0);
                proposalOrder.Add(firstProposal);
            }

            int minY = allElves.Min(e => e.y);
            int maxY = allElves.Max(e => e.y);
            int minX = allElves.Min(e => e.x);
            int maxX = allElves.Max(e => e.x);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Elf elf = grid[x, y];

                    if (elf == null)
                    {
                        result++;
                    }
                }
            }

            return result.ToString();
        }

        public async Task<string> PartTwo(string input)
        {
            List<Direction> proposalOrder = new() { Direction.N, Direction.S, Direction.W, Direction.E };

            await Task.Yield();

            List<Elf> allElves = new();

            Elf[,] orgGrid = input.AsGridMatrix(((c, x, y) => CreateElf(c, x, y)));

            Elf[,] grid = new Elf[orgGrid.GetLength(0) + 2000, orgGrid.GetLength(1) + 2000];

            for (int i = 0; i < orgGrid.GetLength(0); i++)
            {
                for (int j = 0; j < orgGrid.GetLength(1); j++)
                {
                    var elf = orgGrid[i, j];

                    if (elf != null)
                    {
                        grid[i + 1000, j + 1000] = elf;
                        elf.x += 1000;
                        elf.y += 1000;
                        allElves.Add(elf);
                    }
                }
            }

            for (int round = 1; round <= 1000; round++)
            {
                Dictionary<(int x, int y), List<Elf>> proposalsForThisRound = new();

                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        Elf elf = grid[x, y];

                        if (elf != null)
                        {
                            bool allEmpty = allDirs.All(d => grid[x + d.dx, y + d.dy] == null);

                            if (allEmpty == true)
                            {
                                elf.proposal = Direction.None;
                                continue;
                            }

                            foreach (Direction dir in proposalOrder)
                            {
                                bool dirEmpty = DirectionToCoords(dir).All(d => grid[x + d.dx, y + d.dy] == null);

                                if (dirEmpty)
                                {
                                    elf.proposal = dir;

                                    var key = (x + DirectionToCoord(dir).dx, y + DirectionToCoord(dir).dy);

                                    if (proposalsForThisRound.TryGetValue(key, out List<Elf> list))
                                    {
                                        list.Add(elf);
                                    }
                                    else
                                    {
                                        proposalsForThisRound.Add(key, new List<Elf>() { elf });
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }

                bool anyMoved = false;

                foreach (List<Elf> elves in proposalsForThisRound.Values)
                {
                    if (elves.Count == 1)
                    {
                        Elf elf = elves.Single();

                        // MOVE
                        anyMoved = true;
                        grid[elf.x, elf.y] = null!;
                        elf.x = elf.ProposalCoord().x;
                        elf.y = elf.ProposalCoord().y;
                        grid[elf.x, elf.y] = elf;
                    }

                    foreach (Elf e in elves)
                    {
                        e.proposal = Direction.None;
                    }
                }

                if (anyMoved == false)
                {
                    return round.ToString();
                }

                var firstProposal = proposalOrder.First();
                proposalOrder.RemoveAt(0);
                proposalOrder.Add(firstProposal);
            }

            return "error";
        }

        private void PrintGrid(Elf[,] grid)
        {
            Console.Write("  ");
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                Console.Write(x % 10);
            }
            Console.WriteLine();
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Console.Write($"{y:00}");
                for (int x = 0; x < grid.GetLength(0); x++)
                {

                    if (grid[x, y] == null)
                        Console.Write(".");
                    else
                        Console.Write("#");
                }
                Console.WriteLine();
            }

            Console.WriteLine("---------------------------------------------------------------------");
        }

        private static Elf CreateElf(char c, int col, int row)
        {
            if (c == '#')
            {
                return new Elf()
                {
                    x = col,
                    y = row,
                };
            }

            return null!;
        }

        private static List<(int dx, int dy)> DirectionToCoords(Direction direction)
        {
            switch (direction)
            {
                case Direction.N:
                    return new List<(int dx, int dy)>()
                    {
                        (-1,-1),
                        (0, -1),
                        (1, -1)
                    };
                case Direction.S:
                    return new List<(int dx, int dy)>()
                    {
                        (-1,1),
                        (0, 1),
                        (1, 1)
                    }; ;
                case Direction.W:
                    return new List<(int dx, int dy)>()
                    {
                        (-1,-1),
                        (-1, 0),
                        (-1, 1)
                    };
                case Direction.E:
                    return new List<(int dx, int dy)>()
                    {
                        (1,-1),
                        (1, 0),
                        (1, 1)
                    };
            }


            throw new Exception("error");
        }

        private static (int dx, int dy) DirectionToCoord(Direction direction)
        {
            switch (direction)
            {
                case Direction.N:
                    return (0, -1);
                case Direction.S:
                    return (0, 1);
                case Direction.W:
                    return (-1, 0);
                case Direction.E:
                    return (1, 0);
            }


            throw new Exception("error");
        }

        public class Elf : Point
        {
            public List<Direction> order;
            public Direction proposal;

            public Elf()
            {
                order = new() { Direction.N, Direction.S, Direction.S, Direction.E };
                proposal = Direction.None;
            }

            public (int x, int y) ProposalCoord()
            {
                return (this.x + DirectionToCoord(proposal).dx, this.y + DirectionToCoord(proposal).dy);
            }
        }

        public enum Direction
        {
            None,
            N,
            S,
            W,
            E
        }
    }


}
