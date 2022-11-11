using AdventOfCode.Common;

namespace AdventOfCode.Year2021.Day02
{
    internal class Solver : ISolver
    {
        public async Task<string> PartOne(string input)
        {
            await Task.Yield();

            var lines = Helpers.AsLines(input);

            int forwardPos = 0;
            int heightPos = 0;

            foreach (var line in lines)
            {
                var items = line.Split(' ');
                string command = items[0];
                int amount = int.Parse(items[1]);

                if (command == "forward")
                {
                    forwardPos += amount;
                }
                if (command == "up")
                {
                    heightPos -= amount;
                }
                if (command == "down")
                {
                    heightPos += amount;
                }
            }

            return (heightPos * forwardPos).ToString();
        }

        public async Task<string> PartTwo(string input)
        {
            await Task.Yield();

            await Task.Yield();

            var lines = Helpers.AsLines(input);

            int forwardPos = 0;
            int heightPos = 0;
            int aim = 0;

            foreach (var line in lines)
            {
                var items = line.Split(' ');
                string command = items[0];
                int amount = int.Parse(items[1]);

                if (command == "forward")
                {
                    forwardPos += amount;
                    heightPos += amount * aim;
                }
                if (command == "up")
                {
                    aim -= amount;
                }
                if (command == "down")
                {
                    aim += amount;
                }
            }

            return (heightPos * forwardPos).ToString();
        }
    }
}
