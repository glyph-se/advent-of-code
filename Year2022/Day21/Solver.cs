using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Year2022.Day21
{
    internal class Solver : ISolver
    {
        public async Task<string> PartOne(string input)
        {
            await Task.Yield();

            List<MonkeyPart1> monkeys = new();

            foreach (string line in input.AsLines())
            {
                string name1;
                string name2;
                long monkeyNumber;

                Match match = Regex.Match(line, "(\\w+): (\\d+)");

                if (match.Success)
                {
                    monkeyNumber = long.Parse(match.Groups[2].Value);
                    MonkeyPart1 monkey = new()
                    {
                        name = match.Groups[1].Value,
                        number = new Func<long>(() => monkeyNumber),
                    };
                    monkeys.Add(monkey);
                }

                match = Regex.Match(line, "(\\w+): (\\w+) \\+ (\\w+)");

                if (match.Success)
                {
                    name1 = match.Groups[2].Value;
                    name2 = match.Groups[3].Value;
                    MonkeyPart1 monkey = new()
                    {
                        name = match.Groups[1].Value,
                        number = new Func<long>(() => monkeys.Single(m => m.name == name1).number() + monkeys.Single(m => m.name == name2).number()),
                    };
                    monkeys.Add(monkey);
                }

                match = Regex.Match(line, "(\\w+): (\\w+) \\- (\\w+)");

                if (match.Success)
                {
                    name1 = match.Groups[2].Value;
                    name2 = match.Groups[3].Value;
                    MonkeyPart1 monkey = new()
                    {
                        name = match.Groups[1].Value,
                        number = new Func<long>(() => monkeys.Single(m => m.name == name1).number() - monkeys.Single(m => m.name == name2).number()),
                    };
                    monkeys.Add(monkey);
                }

                match = Regex.Match(line, "(\\w+): (\\w+) \\* (\\w+)");

                if (match.Success)
                {
                    name1 = match.Groups[2].Value;
                    name2 = match.Groups[3].Value;
                    MonkeyPart1 monkey = new()
                    {
                        name = match.Groups[1].Value,
                        number = new Func<long>(() => monkeys.Single(m => m.name == name1).number() * monkeys.Single(m => m.name == name2).number()),
                    };
                    monkeys.Add(monkey);
                }

                match = Regex.Match(line, "(\\w+): (\\w+) \\/ (\\w+)");

                if (match.Success)
                {
                    name1 = match.Groups[2].Value;
                    name2 = match.Groups[3].Value;
                    MonkeyPart1 monkey = new()
                    {
                        name = match.Groups[1].Value,
                        number = new Func<long>(() => monkeys.Single(m => m.name == name1).number() / monkeys.Single(m => m.name == name2).number()),
                    };
                    monkeys.Add(monkey);
                }

            }

            long result = 0;

            result = monkeys.Single(m => m.name == "root").number();

            return result.ToString();
        }

        public async Task<string> PartTwo(string input)
        {
            await Task.Yield();

            List<MonkeyPart2> monkeys = new();

            foreach (string line in input.AsLines())
            {
                string name1;
                string name2;
                long monkeyNumber;

                Match match = Regex.Match(line, "(\\w+): (\\d+)");

                if (match.Success)
                {
                    monkeyNumber = long.Parse(match.Groups[2].Value);
                    MonkeyPart2 monkey = new()
                    {
                        name = match.Groups[1].Value,
                        eq = new Func<string>(() => monkeyNumber.ToString()),
                    };
                    monkeys.Add(monkey);
                }

                match = Regex.Match(line, "(\\w+): (\\w+) \\+ (\\w+)");

                if (match.Success)
                {
                    name1 = match.Groups[2].Value;
                    name2 = match.Groups[3].Value;
                    MonkeyPart2 monkey = new()
                    {
                        name = match.Groups[1].Value,
                        eq = new Func<string>(() => "(" + monkeys.Single(m => m.name == name1).eq() + " + " + monkeys.Single(m => m.name == name2).eq() + ")"),
                        left = name1,
                        right = name2
                    };
                    monkeys.Add(monkey);
                }

                match = Regex.Match(line, "(\\w+): (\\w+) \\- (\\w+)");

                if (match.Success)
                {
                    name1 = match.Groups[2].Value;
                    name2 = match.Groups[3].Value;
                    MonkeyPart2 monkey = new()
                    {
                        name = match.Groups[1].Value,
                        eq = new Func<string>(() => "(" + monkeys.Single(m => m.name == name1).eq() + " - " + monkeys.Single(m => m.name == name2).eq() + ")"),
                    };
                    monkeys.Add(monkey);
                }

                match = Regex.Match(line, "(\\w+): (\\w+) \\* (\\w+)");

                if (match.Success)
                {
                    name1 = match.Groups[2].Value;
                    name2 = match.Groups[3].Value;
                    MonkeyPart2 monkey = new()
                    {
                        name = match.Groups[1].Value,
                        eq = new Func<string>(() => "(" + monkeys.Single(m => m.name == name1).eq() + " * " + monkeys.Single(m => m.name == name2).eq() + ")"),
                    };
                    monkeys.Add(monkey);
                }

                match = Regex.Match(line, "(\\w+): (\\w+) \\/ (\\w+)");

                if (match.Success)
                {
                    name1 = match.Groups[2].Value;
                    name2 = match.Groups[3].Value;
                    MonkeyPart2 monkey = new()
                    {
                        name = match.Groups[1].Value,
                        eq = new Func<string>(() => "(" + monkeys.Single(m => m.name == name1).eq() + " / " + monkeys.Single(m => m.name == name2).eq() + ")"),
                    };
                    monkeys.Add(monkey);
                }

            }

            long result = 0;

            MonkeyPart2 rootMonkey = monkeys
                .Single(m => m.name == "root");

            MonkeyPart2 leftMonkey = monkeys.Single(m => m.name == rootMonkey.left);
            MonkeyPart2 rightMonkey = monkeys.Single(m => m.name == rootMonkey.right);
            MonkeyPart2 humanMonkey = monkeys.Single(m => m.name == "humn");

            humanMonkey.eq = () => "x";

            return leftMonkey.eq() + "=" + rightMonkey.eq();
        }

        public class MonkeyPart1
        {
            public string name = null!;
            public Func<long> number = () => 0;
        }

        public class MonkeyPart2
        {
            public string name = null!;
            public Func<string> eq = () => "";
            public string left = null!;
            public string right = null!;
        }
    }
}
