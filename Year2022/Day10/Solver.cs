using AdventOfCode.Common;

namespace AdventOfCode.Year2022.Day10
{
    internal class Solver : ISolver
    {
        public async Task<string> PartOne(string input)
        {
            await Task.Yield();

            int xValue = 1;
            int sumSignal = 0;

            var lines = StringParsing.AsLines(input).ToList();

            for (int cycle = 0; cycle < 1000;)
            {
                if (lines.Count == 0)
                {
                    break;
                }

                var command = lines[0];
                lines.RemoveAt(0);

                if (command == "noop")
                {
                    cycle++;

                    if (cycle == 20 || cycle == 60 || cycle == 100 || cycle == 140 || cycle == 180 || cycle == 220)
                    {
                        sumSignal += xValue * cycle;
                    }
                }
                else
                {
                    var split = command.Split(' ');

                    cycle++;

                    if (cycle == 20 || cycle == 60 || cycle == 100 || cycle == 140 || cycle == 180 || cycle == 220)
                    {
                        sumSignal += xValue * cycle;
                    }

                    cycle++;

                    if (cycle == 20 || cycle == 60 || cycle == 100 || cycle == 140 || cycle == 180 || cycle == 220)
                    {
                        sumSignal += xValue * cycle;
                    }

                    int change = int.Parse(split[1]);
                    xValue += change;
                }
            }

            return sumSignal.ToString();
        }

        public async Task<string> PartTwo(string input)
        {
            await Task.Yield();


            string result = string.Empty;
            result += "\n";

            int xValue = 1;
            int sumSignal = 0;

            var lines = StringParsing.AsLines(input).ToList();

            for (int cycle = 0; cycle < 1000;)
            {
                if (lines.Count == 0)
                {
                    break;
                }

                var command = lines[0];
                lines.RemoveAt(0);

                if (command == "noop")
                {
                    cycle++;

                    if (xValue == ((cycle - 1) % 40) || xValue - 1 == ((cycle - 1) % 40) || xValue + 1 == ((cycle - 1) % 40))
                    {
                        result += "#";
                    }
                    else
                    {
                        result += ".";
                    }

                    if (cycle % 40 == 0)
                    {
                        result += "\n";
                    }
                }
                else
                {
                    var split = command.Split(' ');

                    cycle++;

                    if (xValue == ((cycle - 1) % 40) || xValue - 1 == ((cycle - 1) % 40) || xValue + 1 == ((cycle - 1) % 40))
                    {
                        result += "#";
                    }
                    else
                    {
                        result += ".";
                    }

                    if (cycle % 40 == 0)
                    {
                        result += "\n";
                    }

                    cycle++;

                    if (xValue == ((cycle - 1) % 40) || xValue - 1 == ((cycle - 1) % 40) || xValue + 1 == ((cycle - 1) % 40))
                    {
                        result += "#";
                    }
                    else
                    {
                        result += ".";
                    }

                    if (cycle % 40 == 0)
                    {
                        result += "\n";
                    }

                    int change = int.Parse(split[1]);
                    xValue += change;
                }
            }

            return result.ToString();
        }
    }
}
