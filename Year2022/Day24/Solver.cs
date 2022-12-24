using AdventOfCode.Common;

namespace AdventOfCode.Year2022.Day24
{
    internal class Solver : ISolver
    {
        public async Task<string> PartOne(string input)
        {
            await Task.Yield();

            HashSet<(int x, int y)> grid = new();
            List<Blizzard> blizzards = new();

            var lines = input.AsLines();

            for (int row = 0; row < lines.Count; row++)
            {
                string line = lines[row];
                for (int col = 0; col < line.Length; col++)
                {
                    char c = line[col];
                    if (c == '.')
                    {
                        grid.Add((col, row));
                    }
                    Blizzard? b = CreateBlizzard(c, col, row);

                    if (b != null)
                    {
                        blizzards.Add(b);
                        grid.Add((col, row));
                    }
                }
            }

            int maxX = grid.Max(f => f.x);
            int maxY = grid.Max(f => f.y) - 1;
            (int x, int y) start = (1, 0);
            (int x, int y) end = (maxX, maxY + 1);

            return FindMinutes(start, end, maxX, maxY, blizzards, grid).ToString();
        }

        private void MoveBlizzards(List<Blizzard> blizzards, int maxX, int maxY)
        {
            // Move all blizzards
            foreach (Blizzard b in blizzards)
            {
                b.position.x += b.direction.dx;
                b.position.y += b.direction.dy;

                if (b.position.x > maxX)
                {
                    b.position.x = 1;
                }
                if (b.position.y > maxY)
                {
                    b.position.y = 1;
                }
                if (b.position.x < 1)
                {
                    b.position.x = maxX;
                }
                if (b.position.y < 1)
                {
                    b.position.y = maxY;
                }
            }
        }

        private Blizzard? CreateBlizzard(char c, int x, int y)
        {
            Blizzard b = new();
            b.position = (x, y);

            switch (c)
            {
                case '>':
                    b.direction = (1, 0);
                    break;
                case '<':
                    b.direction = (-1, 0);
                    break;
                case '^':
                    b.direction = (0, -1);
                    break;
                case 'v':
                    b.direction = (0, 1);
                    break;
                default:
                    return null;
            }

            return b;
        }

        public async Task<string> PartTwo(string input)
        {
            await Task.Yield();

            HashSet<(int x, int y)> grid = new();
            List<Blizzard> blizzards = new();

            var lines = input.AsLines();

            for (int row = 0; row < lines.Count; row++)
            {
                string line = lines[row];
                for (int col = 0; col < line.Length; col++)
                {
                    char c = line[col];
                    if (c == '.')
                    {
                        grid.Add((col, row));
                    }
                    Blizzard? b = CreateBlizzard(c, col, row);

                    if (b != null)
                    {
                        blizzards.Add(b);
                        grid.Add((col, row));
                    }
                }
            }

            int maxX = grid.Max(f => f.x);
            int maxY = grid.Max(f => f.y) - 1;
            (int x, int y) start = (1, 0);
            (int x, int y) end = (maxX, maxY + 1);

            int part1 = FindMinutes(start, end, maxX, maxY, blizzards, grid);
            int part2 = FindMinutes(end, start, maxX, maxY, blizzards, grid);
            int part3 = FindMinutes(start, end, maxX, maxY, blizzards, grid);

            return (part1 + part2 + part3).ToString();
        }

        private int FindMinutes((int x, int y) start, (int x, int y) end, int maxX, int maxY, List<Blizzard> blizzards, HashSet<(int x, int y)> grid)
        {
            Queue<(int x, int y)> positions = new();
            positions.Enqueue(start);

            for (int minute = 1; minute < 1000; minute++)
            {
                MoveBlizzards(blizzards, maxX, maxY);

                HashSet<(int x, int y)> blizzardPos = blizzards.Select(b => b.position).ToHashSet();

                Queue<(int x, int y)> nextPositions = new();

                while (positions.Count != 0)
                {
                    (int x, int y) currentPos = positions.Dequeue();

                    (int, int)[] dirs = { (0, 0), (0, 1), (1, 0), (-1, 0), (0, -1) };

                    foreach ((int dx, int dy) in dirs)
                    {
                        (int x, int y) next = (currentPos.x + dx, currentPos.y + dy);
                        if (grid.Contains(next) && !blizzardPos.Contains(next))
                        {
                            nextPositions.Enqueue(next);
                        }
                    }
                }

                if (nextPositions.Contains(end))
                {
                    return minute;
                }

                positions = new(nextPositions.ToHashSet());
            }

            return -1;
        }

        public class Blizzard
        {
            public (int dx, int dy) direction;
            public (int x, int y) position;
        }

        public record State(int minutes, (int x, int y) me);
    }
}
