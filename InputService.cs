namespace AdventOfCode
{
    internal static class InputService
    {
        private static readonly string BaseDirectory = Environment.CurrentDirectory + "\\..\\..\\..\\";

        public static async Task<string?> GetInput(int year, int day)
        {
            Console.WriteLine("Choose problem input:");
            Console.WriteLine(" 1. Example 1 data");
            Console.WriteLine(" 2. Full data");
            Console.WriteLine(" 3. Custom data");
            Console.Write("> ");

            string? inputs;

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    inputs = await ReadOrInputFileAsync(year, day);
                    break;
                case "2":
                    inputs = await ReadOrDownloadFileAsync(year, day);
                    break;
                case "3":
                    inputs = ReadFromConsole();
                    break;
                default:
                    return null;
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
            Console.WriteLine("Type input (end with Ctrl+Z):");
            Console.Write("> ");
            IList<string> inputs = new List<string>();
            string? line;

            while ((line = Console.ReadLine()) != null)
            {
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

            string filePath = BaseDirectory + $"Year{year}\\Day{day:D2}\\full_input";

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
                    string auth = File.ReadAllText(BaseDirectory + ".sessionauth");
                    client.DefaultRequestHeaders.Add("Cookie", "session=" + auth);
                    byte[] data = await client.GetByteArrayAsync(uri);

                    await File.WriteAllBytesAsync(filePath, data);
                }

            }

            string fileContents = await File.ReadAllTextAsync(filePath, System.Text.Encoding.UTF8);

            return fileContents;
        }

        private async static Task<string?> ReadOrInputFileAsync(int year, int day)
        {
            string filePath = BaseDirectory + $"Year{year}\\Day{day:D2}\\example1_input";

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Using file {filePath}");
            Console.ResetColor();

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found, please enter data...");

                string? data = ReadFromConsole();

                await File.WriteAllTextAsync(filePath, data);
            }

            string fileContents = await File.ReadAllTextAsync(filePath, System.Text.Encoding.UTF8);

            return fileContents;
        }
    }
}