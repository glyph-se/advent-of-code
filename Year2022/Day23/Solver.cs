﻿using AdventOfCode.Common;

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

            List<Elf> allElves = new();

            Elf[,] orgGrid = input.AsGrid(((c, x, y) => CreateElf(c, x, y)));

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

            PrintGrid(grid);

            for (int round = 1; round <= 10; round++)
            {
                List<(int x, int y)> proposalsForThisRound = new();

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
                                    proposalsForThisRound.Add((x + DirectionToCoord(dir).dx, y + DirectionToCoord(dir).dy));
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        Elf elf = grid[x, y];

                        if (elf != null)
                        {
                            if (elf.proposal == Direction.None)
                            {
                                continue;
                            }

                            if (proposalsForThisRound.Count(x => x == elf.ProposalCoord()) == 1)
                            {
                                // MOVE
                                elf.x = elf.ProposalCoord().x;
                                elf.y = elf.ProposalCoord().y;
                                grid[x, y] = null;
                                grid[elf.x, elf.y] = elf;
                            }

                            elf.proposal = Direction.None;
                        }
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

            long result = 0;

            //Elf[,] grid = input.AsGrid(((c, x, y) => CreateElf(c, x, y)));

            List<Elf> allElves = new();


            Elf[,] orgGrid = input.AsGrid(((c, x, y) => CreateElf(c, x, y)));

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

            PrintGrid(grid);

            for (int round = 1; round <= 1000; round++)
            {
                List<(int x, int y)> proposalsForThisRound = new();

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
                                    proposalsForThisRound.Add((x + DirectionToCoord(dir).dx, y + DirectionToCoord(dir).dy));
                                    break;
                                }
                            }
                        }
                    }
                }

                bool anyMoved = false;

                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        Elf elf = grid[x, y];

                        if (elf != null)
                        {
                            if (elf.proposal == Direction.None)
                            {
                                continue;
                            }

                            if (proposalsForThisRound.Count(x => x == elf.ProposalCoord()) == 1)
                            {
                                anyMoved = true;
                                // MOVE
                                elf.x = elf.ProposalCoord().x;
                                elf.y = elf.ProposalCoord().y;
                                grid[x, y] = null;
                                grid[elf.x, elf.y] = elf;
                            }

                            elf.proposal = Direction.None;
                        }
                    }
                }

                if (anyMoved == false)
                {
                    return round.ToString();
                }

                var firstProposal = proposalOrder.First();
                proposalOrder.RemoveAt(0);
                proposalOrder.Add(firstProposal);

                PrintGrid(grid);
                //Console.WriteLine("---------------------------------------------------------------------");
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

        private void PrintGrid(Elf[,] grid)
        {
            return;
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
        }

        private static Elf CreateElf(char c, int row, int col)
        {
            if (c == '#')
            {
                return new Elf()
                {
                    x = col,
                    y = row,
                };
            }

            return null;
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

        public class Elf
        {
            public int x;
            public int y;
            public List<Direction> order;
            public Direction proposal;

            public Elf()
            {
                order = new() { Direction.N, Direction.S, Direction.S, Direction.E };
                proposal = Direction.None;
            }

            public (int x, int y) ProposalCoord()
            {
                return (x + DirectionToCoord(proposal).dx, y + DirectionToCoord(proposal).dy);
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