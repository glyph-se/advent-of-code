namespace AdventOfCode
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            int year = 2021;
            int day = 1;

            Type? solution = Type.GetType($"AdventOfCode.Year{year}.Day{day:D2}.Solution");

            if (solution == null)
            {
                Console.WriteLine("Check constants (type not found)!");
                return;
            }

            ISolution? instance = Activator.CreateInstance(solution) as ISolution;

            if (instance == null)
            {
                Console.WriteLine("Check constants (constructor failed)!");
                return;
            }

            Console.WriteLine("Type input or enter to use/download from web");
            string? inputs = ReadFromConsole();

            if (inputs == null)
            {
                inputs = await ReadOrDownloadFileAsync(year, day);
            }

            if (inputs == null)
            {
                Console.WriteLine("No input found!");
                return;
            }

            Console.WriteLine("Running part one...");
            string outputOne = await instance.PartOne(inputs);
            Console.WriteLine("Output is:" + outputOne);

            Console.WriteLine("Running part two...");
            string outputTwo = await instance.PartTwo(inputs);
            Console.WriteLine("Output is:" + outputTwo);

        }

        private async static Task<string?> ReadOrDownloadFileAsync(int year, int day)
        {
            string filePath = Environment.CurrentDirectory + "\\..\\..\\.." + $"\\Year{year}\\Day{day:D2}\\input";

            Console.WriteLine($"Using file {filePath}");

            if (!File.Exists(filePath))
            {
                Uri uri = new Uri($"https://adventofcode.com/{year}/day/{day}/input");

                Console.WriteLine($"File not found, downloading {uri}...");

                using (var handler = new HttpClientHandler())
                using (var client = new HttpClient(handler))
                {
                    string auth = "******";
                    client.DefaultRequestHeaders.Add("Cookie", "session=" + auth);
                    byte[] data = await client.GetByteArrayAsync(uri);

                    await File.WriteAllBytesAsync(filePath, data);
                }

            }

            string fileContents = await File.ReadAllTextAsync(filePath, System.Text.Encoding.UTF8);
            Console.WriteLine($"Returning {fileContents.Length} characters");

            return fileContents;
        }

        private static string? ReadFromConsole()
        {
            IList<string> inputs = new List<string>();
            string? line;

            while ((line = Console.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                inputs.Add(line);
            }

            if (inputs.Count == 0)
            {
                return null;
            }

            return string.Join('\n', inputs);
        }
    }
}