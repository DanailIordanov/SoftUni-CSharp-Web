namespace MyHandmadeWebServer.Application.Views.Home
{
    using Server.Contracts;

    public class IndexView : IView
    {
        public string View() => "<body><p>Welcome<p></body>";
    }
}