using AdventOfCode.Common;

namespace AdventOfCode.Year2022.Day09
{
    internal class Solver : ISolver
    {
        public async Task<string> PartOne(string input)
        {
            await Task.Yield();

            Position head = new Position()
            {
                x = 0,
                y = 0,
            };
            Position tail = new Position()
            {
                x = 0,
                y = 0,
            };

            HashSet<Position> tailVisited = new HashSet<Position>();
            tailVisited.Add(new Position() { x = 0, y = 0 });

            foreach (string line in StringParsing.AsLines(input))
            {
                var split = line.Split(' ');
                string direction = split[0];
                int amount = int.Parse(split[1]);

                switch (direction)
                {
                    case "U":
                        for (int i = 1; i <= amount; i++)
                        {
                            head.y += 1;
                            MoveTailOnce(head, tail, tailVisited);
                            tailVisited.Add(new Position() { x = tail.x, y = tail.y });
                        }
                        break;
                    case "D":
                        for (int i = 1; i <= amount; i++)
                        {
                            head.y -= 1;
                            MoveTailOnce(head, tail, tailVisited);
                            tailVisited.Add(new Position() { x = tail.x, y = tail.y });
                        }
                        break;
                    case "R":

                        for (int i = 1; i <= amount; i++)
                        {
                            head.x += 1;
                            MoveTailOnce(head, tail, tailVisited);
                            tailVisited.Add(new Position() { x = tail.x, y = tail.y });
                        }
                        break;
                    case "L":

                        for (int i = 1; i <= amount; i++)
                        {
                            head.x -= 1;
                            MoveTailOnce(head, tail, tailVisited);
                            tailVisited.Add(new Position() { x = tail.x, y = tail.y });
                        }
                        break;
                }
                Console.WriteLine("----------------");
            }

            int result = tailVisited.Count;

            return result.ToString();
        }

        private static void MoveTailOnce(Position head, Position tail, HashSet<Position> tailVisited)
        {
            if (tail.x == head.x && tail.y == head.y)
            {
                // do nothing
            }


            if (tail.x - head.x > 1 && tail.y == head.y)
            {
                tail.x -= 1;
            }
            else if (tail.y - head.y > 1 && tail.x == head.x)
            {
                tail.y -= 1;
            }
            else if (head.x - tail.x > 1 && tail.y == head.y)
            {
                tail.x += 1;
            }
            else if (head.y - tail.y > 1 && tail.x == head.x)
            {
                tail.y += 1;
            }


            else if ((tail.x - head.x) + (tail.y - head.y) > 2)
            {
                tail.x -= 1;
                tail.y -= 1;
            }
            else if ((tail.x - head.x) + (head.y - tail.y) > 2)
            {
                tail.x -= 1;
                tail.y += 1;
            }
            else if ((head.x - tail.x) + (tail.y - head.y) > 2)
            {
                tail.x += 1;
                tail.y -= 1;
            }
            else if ((head.x - tail.x) + (head.y - tail.y) > 2)
            {
                tail.x += 1;
                tail.y += 1;
            }
            /*
            Console.WriteLine("Head: " + head);
            Console.WriteLine("Tail: " + tail);
            Console.WriteLine("--");
            */
        }

        public async Task<string> PartTwo(string input)
        {
            await Task.Yield();


            Position head = new Position()
            {
                x = 0,
                y = 0,
            };
            Position tail1 = new Position()
            {
                x = 0,
                y = 0,
            };
            Position tail2 = new Position()
            {
                x = 0,
                y = 0,
            };
            Position tail3 = new Position()
            {
                x = 0,
                y = 0,
            };
            Position tail4 = new Position()
            {
                x = 0,
                y = 0,
            };
            Position tail5 = new Position()
            {
                x = 0,
                y = 0,
            };
            Position tail6 = new Position()
            {
                x = 0,
                y = 0,
            };
            Position tail7 = new Position()
            {
                x = 0,
                y = 0,
            };
            Position tail8 = new Position()
            {
                x = 0,
                y = 0,
            };
            Position tail9 = new Position()
            {
                x = 0,
                y = 0,
            };

            HashSet<Position> tailVisited = new HashSet<Position>();
            tailVisited.Add(new Position() { x = tail9.x, y = tail9.y });

            foreach (string line in StringParsing.AsLines(input))
            {
                var split = line.Split(' ');
                string direction = split[0];
                int amount = int.Parse(split[1]);

                switch (direction)
                {
                    case "U":
                        for (int i = 1; i <= amount; i++)
                        {
                            head.y += 1;
                            MoveTailOnce(head, tail1, tailVisited);
                            MoveTailOnce(tail1, tail2, tailVisited);
                            MoveTailOnce(tail2, tail3, tailVisited);
                            MoveTailOnce(tail3, tail4, tailVisited);
                            MoveTailOnce(tail4, tail5, tailVisited);
                            MoveTailOnce(tail5, tail6, tailVisited);
                            MoveTailOnce(tail6, tail7, tailVisited);
                            MoveTailOnce(tail7, tail8, tailVisited);
                            MoveTailOnce(tail8, tail9, tailVisited);
                            tailVisited.Add(new Position() { x = tail9.x, y = tail9.y });
                        }
                        break;
                    case "D":
                        for (int i = 1; i <= amount; i++)
                        {
                            head.y -= 1;
                            MoveTailOnce(head, tail1, tailVisited);
                            MoveTailOnce(tail1, tail2, tailVisited);
                            MoveTailOnce(tail2, tail3, tailVisited);
                            MoveTailOnce(tail3, tail4, tailVisited);
                            MoveTailOnce(tail4, tail5, tailVisited);
                            MoveTailOnce(tail5, tail6, tailVisited);
                            MoveTailOnce(tail6, tail7, tailVisited);
                            MoveTailOnce(tail7, tail8, tailVisited);
                            MoveTailOnce(tail8, tail9, tailVisited);
                            tailVisited.Add(new Position() { x = tail9.x, y = tail9.y });
                        }
                        break;
                    case "R":

                        for (int i = 1; i <= amount; i++)
                        {
                            head.x += 1;
                            MoveTailOnce(head, tail1, tailVisited);
                            MoveTailOnce(tail1, tail2, tailVisited);
                            MoveTailOnce(tail2, tail3, tailVisited);
                            MoveTailOnce(tail3, tail4, tailVisited);
                            MoveTailOnce(tail4, tail5, tailVisited);
                            MoveTailOnce(tail5, tail6, tailVisited);
                            MoveTailOnce(tail6, tail7, tailVisited);
                            MoveTailOnce(tail7, tail8, tailVisited);
                            MoveTailOnce(tail8, tail9, tailVisited);
                            tailVisited.Add(new Position() { x = tail9.x, y = tail9.y });
                        }
                        break;
                    case "L":

                        for (int i = 1; i <= amount; i++)
                        {
                            head.x -= 1;
                            MoveTailOnce(head, tail1, tailVisited);
                            MoveTailOnce(tail1, tail2, tailVisited);
                            MoveTailOnce(tail2, tail3, tailVisited);
                            MoveTailOnce(tail3, tail4, tailVisited);
                            MoveTailOnce(tail4, tail5, tailVisited);
                            MoveTailOnce(tail5, tail6, tailVisited);
                            MoveTailOnce(tail6, tail7, tailVisited);
                            MoveTailOnce(tail7, tail8, tailVisited);
                            MoveTailOnce(tail8, tail9, tailVisited);
                            tailVisited.Add(new Position() { x = tail9.x, y = tail9.y });
                        }
                        break;
                }
                // Console.WriteLine("----------------");
            }

            int result = tailVisited.Count;

            return result.ToString();
        }

        public class Position : IEquatable<Position?>
        {
            public int x;
            public int y;

            public override bool Equals(object? obj)
            {
                return Equals(obj as Position);
            }

            public bool Equals(Position? other)
            {
                return other is not null &&
                       x == other.x &&
                       y == other.y;
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(x, y);
            }

            public override string ToString()
            {
                return $"{x},{y}";
            }
        }
    }
}
