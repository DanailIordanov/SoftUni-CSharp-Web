namespace Mvc.Framework.ViewEngine
{
    using Contracts;

    using System;

    public class ActionResult : IActionResult
    {
        public ActionResult(string fullViewName)
        {
            this.Action = (IRenderable)Activator.CreateInstance(Type.GetType(fullViewName));
        }

        public IRenderable Action { get; set; }

        public string Invoke()
        {
            return this.Action.Render();
        }
    }
}
