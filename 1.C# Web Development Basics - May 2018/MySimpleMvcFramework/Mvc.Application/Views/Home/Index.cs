namespace Mvc.Application.Views.Home
{
    using Mvc.Framework.Contracts;

    public class Index : IRenderable
    {
        public string Render() => "<h1>Hello!<h1>";
    }
}
