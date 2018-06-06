namespace ValidateURL
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;

    public class StartUp
    {
        public static void Main()
        {
            var url = WebUtility.UrlDecode(Console.ReadLine());
            var regex = new Regex(@"(https?|ftp):\/\/([\d\w-]+\.[\w]+)(?::(\d+))*((?:\/[\w\d.]+)*)(?:\?*([\w&=]+))*(?:#*(\w+))*");

            try
            {
                var match = regex.Match(url);

                if (match.Success)
                {
                    var groups = match.Groups;

                    var protocol = groups[1].Value;
                    var host = groups[2].Value;
                    var port = groups[3].Value;
                    var path = groups[4].Value;
                    var queryStrings = groups[5].Value;
                    var fragment = groups[6].Value;

                    var myURL = new URL(protocol, host, port, path, queryStrings, fragment);
                    Console.Write(myURL.ToString());
                }
                else
                {
                    Console.WriteLine("Invalid URL");
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid URL");
            }
        }
    }
}
