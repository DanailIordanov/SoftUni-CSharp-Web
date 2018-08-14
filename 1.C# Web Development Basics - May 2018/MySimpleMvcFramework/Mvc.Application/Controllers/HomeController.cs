namespace Mvc.Application.Controllers
{
    using Mvc.Framework.Contracts;
    using Mvc.Framework.Controllers;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
