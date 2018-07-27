namespace MyHandmadeWebServer.ByTheCake.Controllers
{
    using Infrastructure;
    using Server.Http.Contracts;

    public class HomeController : Controller
    {
        public IHttpResponse Index() => this.FileViewResponse(@"Home\index");

        public IHttpResponse About() => this.FileViewResponse(@"Home\about");
    }
}
