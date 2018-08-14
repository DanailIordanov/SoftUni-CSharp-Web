namespace Mvc.Framework.Contracts
{
    public interface IViewable : IActionResult
    {
        IRenderable View { get; }
    }
}
