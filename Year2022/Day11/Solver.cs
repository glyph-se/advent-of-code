using System.Numerics;
using AdventOfCode.Common;

namespace AdventOfCode.Year2022.Day11
{
    internal class Solver : ISolver
    {
        public async Task<string> PartOne(string input)
        {
            await Task.Yield();

            ulong result = 0;

            List<Monkey> monkeys = new();

            var allMonkeyInput = input.AsLineBlocks();
            foreach (var monkeyInput in allMonkeyInput)
            {
                List<string> lines = monkeyInput
                    .AsLines()
                    .Select(l => l.Trim())
                    .ToList();

                Monkey m = new Monkey();
                m.number = int.Parse(lines[0].Replace("Monkey ", "").Replace(":", ""));
                m.items = lines[1]
                    .Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Last()
                    .Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => BigInteger.Parse(s))
                    .ToList();
                if (lines[2] == "Operation: new = old * old")
                {
                    m.operation = new Func<BigInteger, BigInteger>(i => i * i);
                }
                else if (lines[2].Contains("*"))
                {
                    m.operation = new Func<BigInteger, BigInteger>(i => i * BigInteger.Parse(lines[2].Split(" ").Last().ToString()));
                }
                else if (lines[2].Contains("+"))
                {
                    m.operation = new Func<BigInteger, BigInteger>(i => i + BigInteger.Parse(lines[2].Split(" ").Last().ToString()));
                }

                m.test = BigInteger.Parse(lines[3].Split(" ").Last());
                m.trueTarget = int.Parse(lines[4].Split(" ").Last());
                m.falseTarget = int.Parse(lines[5].Split(" ").Last());

                monkeys.Add(m);
            }

            for (int round = 1; round <= 20; round++)
            {
                foreach (Monkey m in monkeys)
                {
                    foreach (BigInteger item in m.items)
                    {
                        BigInteger worryLevel = m.operation(item);
                        worryLevel = worryLevel / 3;

                        if (worryLevel % m.test == 0)
                        {
                            monkeys.Single(a => a.number == m.trueTarget).items.Add(worryLevel);
                        }
                        else
                        {
                            monkeys.Single(a => a.number == m.falseTarget).items.Add(worryLevel);
                        }
                        m.inspectionCount++;
                    }
                    m.items.Clear();
                }

                Console.WriteLine($"Round {round}");
                foreach (Monkey m in monkeys)
                {
                    Console.Write(m.number + ":");
                    Console.Write(string.Join(",", m.items));
                    Console.WriteLine();
                }
            }

            var sorted = monkeys.OrderByDescending(m => m.inspectionCount).ToList();

            result = sorted[0].inspectionCount * sorted[1].inspectionCount;

            return result.ToString();
        }

        public async Task<string> PartTwo(string input)
        {
            await Task.Yield();

            ulong result = 0;

            BigInteger magic_mod = 1;

            List<Monkey> monkeys = new();

            var allMonkeyInput = input.AsLineBlocks();
            foreach (var monkeyInput in allMonkeyInput)
            {
                List<string> lines = monkeyInput
                    .AsLines()
                    .Select(l => l.Trim())
                    .ToList();

                Monkey m = new Monkey();
                m.number = int.Parse(lines[0].Replace("Monkey ", "").Replace(":", ""));
                m.items = lines[1]
                    .Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Last()
                    .Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => BigInteger.Parse(s))
                    .ToList();
                if (lines[2] == "Operation: new = old * old")
                {
                    m.operation = new Func<BigInteger, BigInteger>(i => i * i);
                }
                else if (lines[2].Contains("*"))
                {
                    m.operation = new Func<BigInteger, BigInteger>(i => i * BigInteger.Parse(lines[2].Split(" ").Last().ToString()));
                }
                else if (lines[2].Contains("+"))
                {
                    m.operation = new Func<BigInteger, BigInteger>(i => i + BigInteger.Parse(lines[2].Split(" ").Last().ToString()));
                }

                m.test = BigInteger.Parse(lines[3].Split(" ").Last());
                m.trueTarget = int.Parse(lines[4].Split(" ").Last());
                m.falseTarget = int.Parse(lines[5].Split(" ").Last());

                magic_mod = magic_mod * m.test;

                monkeys.Add(m);
            }

            for (int round = 1; round <= 10000; round++)
            {
                foreach (Monkey m in monkeys)
                {
                    foreach (BigInteger item in m.items)
                    {
                        BigInteger worryLevel = m.operation(item);
                        worryLevel = worryLevel % magic_mod;

                        if (worryLevel % m.test == 0)
                        {
                            monkeys.Single(a => a.number == m.trueTarget).items.Add(worryLevel);
                        }
                        else
                        {
                            monkeys.Single(a => a.number == m.falseTarget).items.Add(worryLevel);
                        }
                        m.inspectionCount++;
                    }
                    m.items.Clear();
                }

                if (round == 1)
                {
                    Console.WriteLine("Round " + round);
                    Console.WriteLine(monkeys[0].inspectionCount);
                    Console.WriteLine(monkeys[1].inspectionCount);
                    Console.WriteLine(monkeys[2].inspectionCount);
                    Console.WriteLine(monkeys[3].inspectionCount);
                    Console.WriteLine();
                }

                if (round == 20)
                {
                    Console.WriteLine("Round " + round);
                    Console.WriteLine(monkeys[0].inspectionCount);
                    Console.WriteLine(monkeys[1].inspectionCount);
                    Console.WriteLine(monkeys[2].inspectionCount);
                    Console.WriteLine(monkeys[3].inspectionCount);
                    Console.WriteLine();
                }

                if (round % 1000 == 0)
                {

                    Console.WriteLine("Round " + round);
                    Console.WriteLine(monkeys[0].inspectionCount);
                    Console.WriteLine(monkeys[1].inspectionCount);
                    Console.WriteLine(monkeys[2].inspectionCount);
                    Console.WriteLine(monkeys[3].inspectionCount);
                    Console.WriteLine();
                }
            }

            var sorted = monkeys.OrderByDescending(m => m.inspectionCount).ToList();

            result = sorted[0].inspectionCount * sorted[1].inspectionCount;

            return result.ToString();
        }

        public class Monkey
        {
            public int number = 0;
            public List<BigInteger> items = new();
            public BigInteger test;
            public int trueTarget = 0;
            public int falseTarget = 0;
            public Func<BigInteger, BigInteger> operation = new Func<BigInteger, BigInteger>(i => i);
            public ulong inspectionCount = 0;

        }

    }
}
