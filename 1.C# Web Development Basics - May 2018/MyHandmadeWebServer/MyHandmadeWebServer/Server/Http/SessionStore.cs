using System.Collections.Concurrent;

namespace MyHandmadeWebServer.Server.Http
{
    public static class SessionStore
    {
        public const string SessionCookieKey = "MYSID";

        private static readonly ConcurrentDictionary<string, HttpSession> sessions = new ConcurrentDictionary<string, HttpSession>();

        public static HttpSession Get(string id) => sessions.GetOrAdd(id, _ => new HttpSession(id));
    }
}
