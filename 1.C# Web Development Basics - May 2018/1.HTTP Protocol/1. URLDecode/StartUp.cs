namespace URLDecode
{
    using System;
    using System.Net;

    public class StartUp
    {
        public static void Main()
        {
            Console.WriteLine(WebUtility.UrlDecode(Console.ReadLine()));
        }
    }
}
