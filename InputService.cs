namespace AdventOfCode
{
    internal static class InputService
    {

        public static async Task<string?> GetInput(int year, int day)
        {
            string? inputs = ReadFromConsole();

            if (inputs == null)
            {
                inputs = await ReadOrDownloadFileAsync(year, day);
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Returning {inputs?.Length} characters");
            Console.ResetColor();
            Console.WriteLine();

            return inputs;
        }

        private static string? ReadFromConsole()
        {
            Console.WriteLine("Type input or enter to use/download from web:");
            Console.Write("> ");
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

        private async static Task<string?> ReadOrDownloadFileAsync(int year, int day)
        {
            string filePath = Environment.CurrentDirectory + "\\..\\..\\.." + $"\\Year{year}\\Day{day:D2}\\input";

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Using file {filePath}");
            Console.ResetColor();

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

            return fileContents;
        }
    }
}