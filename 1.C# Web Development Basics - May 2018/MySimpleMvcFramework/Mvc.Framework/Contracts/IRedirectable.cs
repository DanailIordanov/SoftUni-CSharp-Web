namespace Mvc.Framework.Contracts
{
    public interface IRedirectable : IActionResult
    {
        string RedirectUrl { get; }
    }
}
