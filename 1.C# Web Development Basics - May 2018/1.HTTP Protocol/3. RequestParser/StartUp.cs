namespace RequestParser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            var line = "";
            var paths = new List<string>();

            while ((line = Console.ReadLine()) != "END")
            {
                paths.Add(line);
            }

            var request = Console.ReadLine().Split();
            var result = new StringBuilder();

            var successText = "OK";
            var failedText = "Not Found";

            if (paths.Any(p => p.StartsWith(request[1]) && p.EndsWith(request[0].ToLower())))
            {
                result.AppendLine($"{request[2]} 200 {successText}");
                result.AppendLine($"Content-Length: {successText.Length}");
                result.AppendLine($"Content-Type: text/plain");
                result.AppendLine();
                result.AppendLine(successText);
            }
            else
            {
                result.AppendLine($"{request[2]} 200 {failedText}");
                result.AppendLine($"Content-Length: {failedText.Length}");
                result.AppendLine($"Content-Type: text/plain");
                result.AppendLine();
                result.AppendLine(failedText);
            }

            Console.Write(result.ToString());
        }
    }
}
