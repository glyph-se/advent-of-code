namespace AdventOfCode.Year2022.Day17
{
    internal class Solver : ISolver
    {
        public async Task<string> PartOne(string input)
        {
            await Task.Yield();

            Queue<char> winds = new(input.Trim().ToCharArray());

            List<IRock> rocksInCave = new();


            int highestRock = -1;
            IRock rock;

            while (true)
            {
                // New rock
                rock = new Rock1();
                rock.xPos = 2;
                rock.yPos = highestRock + 3 + rock.shape.GetLength(0);
                while (MoveRockTwice(rock)) ;
                highestRock = Math.Max(highestRock, rock.yPos);
                rocksInCave.Add(rock);

                if (rocksInCave.Count == 2022) break;

                // New rock
                rock = new Rock2();
                rock.xPos = 2;
                rock.yPos = highestRock + 3 + rock.shape.GetLength(0);
                while (MoveRockTwice(rock)) ;
                highestRock = Math.Max(highestRock, rock.yPos);
                rocksInCave.Add(rock);

                if (rocksInCave.Count == 2022) break;

                // New rock
                rock = new Rock3();
                rock.xPos = 2;
                rock.yPos = highestRock + 3 + rock.shape.GetLength(0);
                while (MoveRockTwice(rock)) ;
                highestRock = Math.Max(highestRock, rock.yPos);
                rocksInCave.Add(rock);

                if (rocksInCave.Count == 2022) break;

                // New rock
                rock = new Rock4();
                rock.xPos = 2;
                rock.yPos = highestRock + 3 + rock.shape.GetLength(0);
                while (MoveRockTwice(rock)) ;
                highestRock = Math.Max(highestRock, rock.yPos);
                rocksInCave.Add(rock);

                if (rocksInCave.Count == 2022) break;

                // New rock
                rock = new Rock5();
                rock.xPos = 2;
                rock.yPos = highestRock + 3 + rock.shape.GetLength(0);
                while (MoveRockTwice(rock)) ;
                highestRock = Math.Max(highestRock, rock.yPos);
                rocksInCave.Add(rock);

                if (rocksInCave.Count == 2022) break;
            }

            int result = highestRock + 1;

            return result.ToString();

            bool MoveRockTwice(IRock rock)
            {
                //Print(rocksInCave, highestRock, rock);

                // Blow wind
                if (winds.Count == 0)
                {
                    winds = new(input.Trim().ToCharArray());
                }
                char wind = winds.Dequeue();

                //Console.WriteLine($"Wind is {wind}");

                if (wind == '<')
                {
                    rock.xPos--;

                    if (rock.xPos < 0 || rocksInCave.Any(r => r.BlockedPos().Intersect(rock.BlockedPos()).Any()))
                    {
                        // Can't blow
                        rock.xPos++;
                    }
                }
                else if (wind == '>')
                {
                    rock.xPos++;

                    if (rock.xPos + rock.shape.GetLength(1) > 7 || rocksInCave.Any(r => r.BlockedPos().Intersect(rock.BlockedPos()).Any()))
                    {
                        // Can't blow
                        rock.xPos--;
                    }
                }

                //Print(rocksInCave, highestRock, rock);

                //Console.WriteLine($"Trying fall");

                // Try fall
                rock.yPos--;

                if (rock.yPos - rock.shape.GetLength(0) + 1 < 0 || rocksInCave.Any(r => r.BlockedPos().Intersect(rock.BlockedPos()).Any()))
                {
                    // Can't fall down, move back up
                    rock.yPos++;

                    // Reached the end
                    return false;
                }

                return true;
            }
        }

        private void Print(List<IRock> rocksInCave, int highestRock, IRock extraRock = null)
        {
            if (extraRock != null)
            {
                rocksInCave.Add(extraRock);
                highestRock += 10;
            }

            IEnumerable<(int xPos, int yPos)> positions = rocksInCave.SelectMany(r => r.BlockedPos());

            HashSet<(int xPos, int yPos)> hpositions = positions.ToHashSet();

            for (int y = highestRock; y >= highestRock - 20; y--)
            {
                if (y < -1)
                {
                    continue;
                }
                if (y == -1)
                {
                    Console.WriteLine("+-------+");
                    continue;
                }

                Console.Write('|');
                for (int x = 0; x <= 6; x++)
                {

                    if (positions.Contains((x, y)))
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.Write('|');
                Console.WriteLine();
            }

            if (extraRock != null)
            {
                rocksInCave.Remove(extraRock);
                highestRock -= 10;
            }

            Console.WriteLine($"Highest {highestRock}");
        }

        private sealed record CacheKey(long winds, long rockType, List<int> diffFromTop)
        {
            public bool Equals(CacheKey? other)
            {
                return other != null &&
                    other.winds == winds &&
                    other.rockType == rockType &&
                    other.diffFromTop.SequenceEqual(diffFromTop);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(winds, rockType, diffFromTop);
            }
        }

        private record CacheValue(long rockCount, int towerHeight);

        public async Task<string> PartTwo(string input)
        {
            await Task.Yield();


            Queue<char> winds = new(input.Trim().ToCharArray());

            List<List<bool>> rock1 = new()
            {
                { new List<bool>() {true, true, true, true} },
            };

            List<List<bool>> rock2 = new()
            {
                { new List<bool>() {false, true, false} },
                { new List<bool>() {true, true, true} },
                { new List<bool>() { false, true, false} },
            };

            List<List<bool>> rock3 = new()
            {
                { new List<bool>() {false, false, true} },
                { new List<bool>() { false, false, true} },
                { new List<bool>() { true, true, true} },
            };

            List<List<bool>> rock4 = new()
            {
                { new List<bool>() {true} },
                { new List<bool>() {true} },
                { new List<bool>() {true} },
                { new List<bool>() {true} }
            };

            List<List<bool>> rock5 = new()
            {
                { new List<bool>() {true, true} },
                { new List<bool>() {true, true} },
            };


            int towerHeight = 0;

            SortedDictionary<int, List<bool>> tower = new();
            Dictionary<CacheKey, CacheValue> cache = new();

            long extraHeight = 0;

            for (long rockCount = 0; rockCount < 1000000000000L; rockCount++)
            {
                long rockType = rockCount % 5;

                List<List<bool>> rock = null;

                if (rockType == 0)
                    rock = rock1;
                if (rockType == 1)
                    rock = rock2;
                if (rockType == 2)
                    rock = rock3;
                if (rockType == 3)
                    rock = rock4;
                if (rockType == 4)
                    rock = rock5;

                int xPos = 2;
                int yPos = towerHeight + 3;

                while (true)
                {
                    // Blow wind
                    if (winds.Count == 0)
                    {
                        winds = new(input.Trim().ToCharArray());
                    }
                    char wind = winds.Dequeue();

                    if (wind == '<' && xPos > 0)
                    {
                        if (CanMove(rock, xPos - 1, yPos))
                        {
                            xPos--;
                        }
                    }
                    else if (wind == '>' && xPos < 7 - rock[0].Count)
                    {
                        if (CanMove(rock, xPos + 1, yPos))
                        {
                            xPos++;
                        }
                    }

                    // Move down

                    if (CanMove(rock, xPos, yPos - 1) && yPos > 0)
                    {
                        yPos--;
                    }
                    else
                    {
                        for (int sY = 0; sY < rock.Count; sY++)
                        {
                            int actualY = yPos + (rock.Count - sY);
                            List<bool> shapeLayer = rock[sY];

                            if (!tower.ContainsKey(actualY))
                            {
                                tower[actualY] = new List<bool>()
                                {
                                    false,
                                    false,
                                    false,
                                    false,
                                    false,
                                    false,
                                    false
                                };
                            }

                            List<bool> layer = tower[actualY];
                            for (int sX = 0; sX < shapeLayer.Count; sX++)
                            {
                                layer[xPos + sX] |= shapeLayer[sX];
                            }
                        }
                        break;
                    }
                }

                towerHeight = tower.Count;

                List<int> diffFromTop = new List<int>();

                for (int i = 0; i < 7; i++)
                {
                    for (int j = towerHeight; j > 0; j--)
                    {
                        if (tower[j][i])
                        {
                            diffFromTop.Add(towerHeight - j);
                            break;
                        }
                    }
                }

                CacheKey key = new CacheKey(winds.Count, (int)rockType, diffFromTop);
                if (cache.ContainsKey(key))
                {
                    CacheValue prev = cache[key];

                    long repeat = (1000000000000L - rockCount) / (rockCount - prev.rockCount);
                    rockCount += (rockCount - prev.rockCount) * repeat;
                    extraHeight += (towerHeight - prev.towerHeight) * repeat;
                }

                cache[key] = new CacheValue(rockCount, towerHeight);
            }

            return (towerHeight + extraHeight).ToString();

            bool CanMove(List<List<bool>> rock, int xPos, int yPos)
            {
                for (int rockY = 0; rockY < rock.Count; rockY++)
                {
                    int testY = yPos + (rock.Count - rockY);
                    if (!tower.ContainsKey(testY))
                    {
                        continue;
                    }

                    List<bool> rockLine = rock[rockY];
                    List<bool> towerLine = tower[testY];
                    for (int rockX = 0; rockX < rockLine.Count; rockX++)
                    {
                        if (towerLine[xPos + rockX] && rockLine[rockX])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        public interface IRock
        {
            char[,] shape { get; }

            int xPos { get; set; }

            int yPos { get; set; }

            List<(int xPos, int yPos)> BlockedPos();
        }

        public class Rock1 : IRock
        {
            public char[,] shape { get => new char[,] { { '#', '#', '#', '#' } }; }
            public int xPos { get; set; }
            public int yPos { get; set; }

            public void TryBlow(char c)
            {
                if (c == '<')
                {
                    if (xPos > 0) xPos--;
                }
                else if (c == '>')
                {
                    if (xPos + 4 < 7) xPos++;
                }
            }

            public List<(int xPos, int yPos)> BlockedPos()
            {
                return new List<(int xPos, int yPos)>()
            {
                { (xPos, yPos) },
                { (xPos+1, yPos) },
                { (xPos+2, yPos) },
                { (xPos+3, yPos) }
            };
            }
        }

        public class Rock2 : IRock
        {
            public char[,] shape
            {
                get => new char[,] {
                { '.', '#', '.',},
                { '#', '#', '#',},
                { '.', '#', '.',}
            };
            }
            public int xPos { get; set; }
            public int yPos { get; set; }

            public void TryBlow(char c)
            {
                if (c == '<')
                {
                    if (xPos > 0) xPos--;
                }
                else if (c == '>')
                {
                    if (xPos + 3 < 7) xPos++;
                }
            }

            public List<(int xPos, int yPos)> BlockedPos()
            {
                return new List<(int xPos, int yPos)>()
            {
                { (xPos+1, yPos) },
                { (xPos+0, yPos-1) },
                { (xPos+1, yPos-1) },
                { (xPos+2, yPos-1) },
                { (xPos+1, yPos-2) }
            };
            }
        }
        public class Rock3 : IRock
        {
            public char[,] shape
            {
                get => new char[,] {
                { '.', '.', '#', },
                { '.', '.', '#',},
                { '#', '#', '#',}
            };
            }
            public int xPos { get; set; }
            public int yPos { get; set; }

            public void TryBlow(char c)
            {
                if (c == '<')
                {
                    if (xPos > 0) xPos--;
                }
                else if (c == '>')
                {
                    if (xPos + 3 < 7) xPos++;
                }
            }

            public List<(int xPos, int yPos)> BlockedPos()
            {
                return new List<(int xPos, int yPos)>()
            {
                { (xPos+2, yPos) },
                { (xPos+2, yPos-1) },
                { (xPos, yPos-2) },
                { (xPos+1, yPos-2) },
                { (xPos+2, yPos-2) }
            };
            }
        }
        public class Rock4 : IRock
        {
            public char[,] shape
            {
                get => new char[,] {
                { '#'},
                { '#'},
                { '#'},
                { '#'}
            };
            }
            public int xPos { get; set; }
            public int yPos { get; set; }

            public void TryBlow(char c)
            {
                if (c == '<')
                {
                    if (xPos > 0) xPos--;
                }
                else if (c == '>')
                {
                    if (xPos + 1 < 7) xPos++;
                }
            }
            public List<(int xPos, int yPos)> BlockedPos()
            {
                return new List<(int xPos, int yPos)>()
            {
                { (xPos, yPos-0) },
                { (xPos, yPos-1) },
                { (xPos, yPos-2) },
                { (xPos, yPos-3) }
            };
            }
        }
        public class Rock5 : IRock
        {
            public char[,] shape
            {
                get => new char[,] {
                { '#', '#' },
                { '#', '#' }
            };
            }
            public int xPos { get; set; }
            public int yPos { get; set; }

            public void TryBlow(char c)
            {
                if (c == '<')
                {
                    if (xPos > 0) xPos--;
                }
                else if (c == '>')
                {
                    if (xPos + 2 < 7) xPos++;
                }
            }

            public List<(int xPos, int yPos)> BlockedPos()
            {
                return new List<(int xPos, int yPos)>()
            {
                { (xPos+0, yPos-0) },
                { (xPos+1, yPos-0) },
                { (xPos+0, yPos-1) },
                { (xPos+1, yPos-1) }
            };
            }
        }
    }
}
