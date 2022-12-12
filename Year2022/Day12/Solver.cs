using AdventOfCode.Common;

namespace AdventOfCode.Year2022.Day912
{
    internal class Solver : ISolver
    {
        public async Task<string> PartOne(string input)
        {
            await Task.Yield();

            int result = 0;

            var nodes = BuildGrid(input);

            Node startNode = nodes
                .Cast<Node>()
                .Single(n => n.isStart);
            Node endNode = nodes
                .Cast<Node>()
                .Single(n => n.isEnd);

            var distances = Dijkstra(startNode, endNode);

            result = distances[endNode];

            return result.ToString();
        }

        private static Node[,] BuildGrid(string input)
        {
            var lines = input.AsLines();

            Node[,] nodes = new Node[lines.Count, lines[0].Length];

            for (int row = 0; row < lines.Count; row++)
            {
                string line = lines[row];
                for (int col = 0; col < line.Length; col++)
                {
                    char c = line[col];

                    Node node = new Node()
                    {
                        elevation = c,
                        col = col,
                        row = row,
                    };

                    nodes[row, col] = node;

                    if (c == 'S')
                    {
                        node.isStart = true;
                        node.elevation = 'a';
                    }

                    if (c == 'E')
                    {
                        node.isEnd = true;
                        node.elevation = 'z';
                    }
                }
            }


            for (int row = 0; row < nodes.GetLength(0); row++)
            {
                for (int col = 0; col < nodes.GetLength(1); col++)
                {
                    Node node = nodes[row, col];

                    (int, int)[] dirs = { (0, 1), (1, 0), (-1, 0), (0, -1) };

                    foreach ((int x, int y) in dirs)
                    {
                        int edgeX = row + x;
                        int edgeY = col + y;

                        if (edgeX < 0 || edgeY < 0 || edgeX >= nodes.GetLength(0) || edgeY >= nodes.GetLength(1))
                        {
                            continue;
                        }

                        if (nodes[edgeX, edgeY].elevation <= node.elevation + 1)
                        {
                            node.edges.Add(nodes[edgeX, edgeY]);
                        }
                    }
                }
            }

            return nodes;
        }

        public static Dictionary<Node, int> Dijkstra(Node start, Node end)
        {
            Dictionary<Node, int> distances = new();
            PriorityQueue<Node, int> queue = new PriorityQueue<Node, int>();
            HashSet<Node> visited = new HashSet<Node>();

            visited.Add(start);
            queue.Enqueue(start, 0);
            distances[start] = 0;

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                if (current == end)
                {
                    //return distances[end];
                }

                foreach (Node next in current.edges)
                {
                    if (visited.Contains(next))
                    {
                        continue;
                    }

                    visited.Add(next);

                    int distance = distances[current] + 1;
                    distances[next] = distance;
                    queue.Enqueue(next, distance);
                }
            }

            return distances;
        }


        public async Task<string> PartTwo(string input)
        {
            await Task.Yield();

            var nodes = BuildGrid(input);

            Node endNode = nodes
                .Cast<Node>()
                .Single(n => n.isEnd);

            List<int> result = new();

            foreach (Node node in nodes.Cast<Node>().Where(n => n.elevation == 'a'))
            {
                var distances = Dijkstra(node, endNode);

                if (distances.ContainsKey(endNode))
                {
                    result.Add(distances[endNode]);
                }
            }

            return result.OrderBy(e => e).First().ToString();
        }

        public class Node
        {
            public bool isStart = false;
            public bool isEnd = false;
            public char elevation = '0';
            public List<Node> edges = new();
            public int col = 0;
            public int row = 0;

            public override bool Equals(object? obj)
            {
                return obj is Node node &&
                       col == node.col &&
                       row == node.row;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(col, row);
            }
        }
    }
}
