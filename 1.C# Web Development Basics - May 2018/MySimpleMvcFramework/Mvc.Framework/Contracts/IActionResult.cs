namespace Mvc.Framework.Contracts
{
    public interface IActionResult : IInvocable
    {
        IRenderable Action { get; set; }
    }
}
