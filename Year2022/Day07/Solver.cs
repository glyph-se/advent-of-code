using AdventOfCode.Common;

namespace AdventOfCode.Year2022.Day07
{
    internal class Solver : ISolver
    {
        public async Task<string> PartOne(string input)
        {
            await Task.Yield();

            int result = 0;

            DiskDir currentDir = null;
            List<DiskDir> allDirs = new();
            List<DiskDir> breadCrumb = new();

            foreach (string line in StringParsing.AsLines(input))
            {
                if (line.Equals("$ cd .."))
                {
                    breadCrumb.RemoveAt(breadCrumb.Count - 1);
                    continue;
                }

                if (line.StartsWith("$ cd "))
                {
                    currentDir = new DiskDir(line.Split(' ')[2]);

                    allDirs.Add(currentDir);
                    breadCrumb.Add(currentDir);
                    continue;
                }

                if (line.Equals("$ ls"))
                {
                    continue;
                }

                if (line.StartsWith("dir "))
                {
                    continue;
                }

                string[] fileLine = line.Split(' ');
                int currentFileSize = int.Parse(fileLine[0]);

                foreach (DiskDir d in breadCrumb)
                {
                    d.Size += currentFileSize;
                }
            }

            foreach (DiskDir d in allDirs)
            {
                if (d.Size <= 100000)
                {
                    result += d.Size;
                }
            }

            return result.ToString();
        }


        public async Task<string> PartTwo(string input)
        {
            await Task.Yield();

            int result = 0;

            DiskDir currentDir = new DiskDir("/");
            List<DiskDir> allDirs = new();
            List<DiskDir> breadCrumb = new();

            foreach (string line in StringParsing.AsLines(input))
            {
                if (line.Equals("$ cd .."))
                {
                    breadCrumb.RemoveAt(breadCrumb.Count - 1);
                    continue;
                }

                if (line.StartsWith("$ cd "))
                {
                    currentDir = new DiskDir(line.Split(' ')[2]);

                    allDirs.Add(currentDir);
                    breadCrumb.Add(currentDir);
                    continue;
                }

                if (line.Equals("$ ls"))
                {
                    continue;
                }

                if (line.StartsWith("dir "))
                {
                    continue;
                }

                string[] fileLine = line.Split(' ');
                int currentFileSize = int.Parse(fileLine[0]);

                foreach (DiskDir d in breadCrumb)
                {
                    d.Size += currentFileSize;
                }
            }

            int totalDiskSpace = 70_000_000;
            int neededSpace = 30_000_000;
            int usedSpace = allDirs.First().Size;
            int freeSpace = totalDiskSpace - usedSpace;

            var sortedDirs = allDirs.OrderBy(s => s.Size);

            foreach (DiskDir d in sortedDirs)
            {
                if (freeSpace + d.Size >= neededSpace)
                {
                    return d.Size.ToString();
                }
            }

            return "error";
        }

        private class DiskDir
        {
            public DiskDir(string name)
            {
                Name = name;
            }

            public string Name { get; init; }

            public int Size { get; set; }
        }
    }
}
