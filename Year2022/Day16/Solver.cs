using System.Diagnostics;
using AdventOfCode.Common;

namespace AdventOfCode.Year2022.Day16
{
    internal class Solver : ISolver
    {
        /// <summary>
        /// Old solution that only worked for part1, to slow for part2
        /// </summary>
        public static int FlowSlow(Node current, int totalMinutes)
        {
            List<Flow3_PathWithP> paths = new();
            paths.Add(new Flow3_PathWithP(current, new HashSet<Node>(), 0));

            Dictionary<Flow3_Path, int> best = new();


            for (int minute = 1; minute <= totalMinutes; minute++)
            {
                List<Flow3_PathWithP> new_paths = new();

                foreach (Flow3_PathWithP path in paths)
                {
                    Flow3_Path pathBest = new Flow3_Path(path.location, path.opened);

                    if (best.ContainsKey(pathBest) && path.preassureReleased <= best[pathBest])
                    {
                        continue;
                    }

                    best[pathBest] = path.preassureReleased;

                    if (!path.opened.Contains(path.location) && path.location.flowRate > 0)
                    {
                        new_paths.Add(new Flow3_PathWithP(
                            path.location,
                            path.opened.Append(path.location).ToHashSet(),
                            path.preassureReleased + path.location.flowRate * (totalMinutes - minute)
                            ));
                    }
                    foreach (Node n in path.location.edges)
                    {
                        new_paths.Add(new Flow3_PathWithP(
                            n,
                            path.opened,
                            path.preassureReleased
                            ));
                    }
                }

                paths = new_paths;
            }

            return paths.Max(p => p.preassureReleased);
        }

        public static int FlowJustMe(Node start, int totalMinutes)
        {
            Queue<(int time, Node location, int preassure, HashSet<Node> opened)> paths = new();
            int maxPreassure = 0;
            Dictionary<(int time, Node location), int> scores = new();

            paths.Enqueue((1, start, 0, new HashSet<Node>()));

            while (paths.Count > 0)
            {
                (int time, Node location, int preassure, HashSet<Node> opened) = paths.Dequeue();


                if (scores.ContainsKey((time, location)) && scores[(time, location)] >= preassure)
                {
                    continue;
                }
                scores[(time, location)] = preassure;

                if (time == totalMinutes)
                {
                    maxPreassure = Math.Max(maxPreassure, preassure);
                    continue;
                }

                // If we open here
                if (location.flowRate > 0 && !opened.Contains(location))
                {
                    int openScore = preassure + location.flowRate + opened.Sum(n => n.flowRate);
                    paths.Enqueue((time + 1, location, openScore, opened.Append(location).ToHashSet()));
                }

                // If we walk to next
                int newScore = preassure + opened.Sum(n => n.flowRate);

                foreach (Node n in location.edges)
                {
                    paths.Enqueue((time + 1, n, newScore, opened));
                }
            }


            return maxPreassure;
        }

        public static int FlowWithElephant(Node start, int totalMinutes)
        {
            Queue<(int time, Node location, Node elefant, int preassure, HashSet<Node> opened)> paths = new();
            int maxPreassure = 0;
            Dictionary<(int time, Node location, Node elephant), int> scores = new();

            paths.Enqueue((1, start, start, 0, new HashSet<Node>()));

            while (paths.Count > 0)
            {
                (int time, Node location, Node elephant, int preassure, HashSet<Node> opened) = paths.Dequeue();


                if (scores.ContainsKey((time, location, elephant)) && scores[(time, location, elephant)] >= preassure)
                {
                    continue;
                }
                scores[(time, location, elephant)] = preassure;

                if (time == totalMinutes)
                {
                    maxPreassure = Math.Max(maxPreassure, preassure);
                    continue;
                }

                // Båda två öppnar
                if (location.flowRate > 0 && !opened.Contains(location) && elephant.flowRate > 0 && !opened.Contains(elephant))
                {
                    int openScore = preassure + location.flowRate + elephant.flowRate + opened.Sum(n => n.flowRate);
                    paths.Enqueue((time + 1, location, elephant, openScore, opened.Append(location).Append(elephant).ToHashSet()));


                }

                // Om bara jag öppnar
                if (location.flowRate > 0 && !opened.Contains(location))
                {
                    int openScore = preassure + location.flowRate + opened.Sum(n => n.flowRate);

                    foreach (Node m in elephant.edges)
                    {
                        paths.Enqueue((time + 1, location, m, openScore, opened.Append(location).ToHashSet()));
                    }
                }

                // Om bara elefant öppnar
                if (elephant.flowRate > 0 && !opened.Contains(elephant))
                {
                    int openScore = preassure + elephant.flowRate + opened.Sum(n => n.flowRate);

                    foreach (Node n in location.edges)
                    {
                        paths.Enqueue((time + 1, n, elephant, openScore, opened.Append(elephant).ToHashSet()));
                    }
                }

                // Båda går
                int newScore = preassure + opened.Sum(n => n.flowRate);

                foreach (Node n in location.edges)
                {
                    foreach (Node m in elephant.edges)
                    {
                        paths.Enqueue((time + 1, n, m, newScore, opened));
                    }
                }
            }


            return maxPreassure;
        }

        public async Task<string> PartOne(string input)
        {
            await Task.Yield();

            List<Node> nodes = ParseInput(input);

            Node start = nodes.Single(n => n.name == "AA");


            return FlowJustMe(start, 30).ToString();
        }

        public async Task<string> PartTwo(string input)
        {
            await Task.Yield();

            List<Node> nodes = ParseInput(input);

            Node start = nodes.Single(n => n.name == "AA");

            return FlowWithElephant(start, 26).ToString();
        }


        private static List<Node> ParseInput(string input)
        {
            List<Node> nodes = new();

            foreach (string line in input.AsLines())
            {
                string line2 = line.Replace("Valve ", "");
                line2 = line2.Replace("has flow rate=", "");
                line2 = line2.Replace("tunnels lead to valves ", "");
                line2 = line2.Replace("tunnel leads to valve ", "");

                var split = line2.Split(';', StringSplitOptions.TrimEntries);

                var tunnels = split[1].Split(",", StringSplitOptions.TrimEntries).ToList();

                split = split[0].Split(" ");

                string name = split[0];
                int flow = int.Parse(split[1]);

                Node n = new Node();
                n.name = name;
                n.flowRate = flow;
                n.edgeString = tunnels;

                nodes.Add(n);
            }

            foreach (Node n in nodes)
            {
                foreach (string s in n.edgeString)
                {
                    n.edges.Add(nodes.Single(n => n.name == s));
                }
            }

            /*foreach (Node n in nodes)
            {
                n.edgesWithDist = Dijkstra(n);
                n.edgesWithDist.Remove(n);
                foreach (Node e in n.edges)
                {
                    n.edgesWithDist[e] = 1;
                }

                foreach (KeyValuePair<Node, int> kvp in n.edgesWithDist)
                {
                    if (kvp.Key.flowRate == 0 && kvp.Key.name != "AA")
                    {
                        n.edgesWithDist.Remove(kvp.Key);
                    }
                }
             }*/

            foreach (Node n in nodes.ToList())
            {
                if (n.flowRate == 0 && n.name != "AA")
                {
                    nodes.Remove(n);
                }
            }

            return nodes;
        }


        [DebuggerDisplay("Node = {name}")]
        public class Node : IEquatable<Node>
        {
            public List<Node> edges = new();
            public Dictionary<Node, int> edgesWithDist = new();
            public string name = null!;
            public int flowRate = 0;
            public List<string> edgeString = null!;
            public bool isOpen = false;


            public override bool Equals(object? obj)
            {
                return Equals(obj as Node);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(name);
            }

            public bool Equals(Node? other)
            {
                return other is not null &&
                    name == other.name;
            }
        }


        public record Flow3_Path(Node location, HashSet<Node> opened);


        public record Flow3_PathWithP(Node location, HashSet<Node> opened, int preassureReleased);
    }
}
