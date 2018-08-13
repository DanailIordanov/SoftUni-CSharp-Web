namespace Mvc.Framework.ViewEngine.Generic
{
    using Contracts.Generic;

    using System;

    public class ActionResult<T> : IActionResult<T>
    {
        public ActionResult(string fullViewName, T model)
        {
            this.Action = (IRenderable<T>)Activator.CreateInstance(Type.GetType(fullViewName));
            this.Action.Model = model;
        }

        public IRenderable<T> Action { get; set; }

        public string Invoke()
        {
            return this.Action.Render();
        }
    }
}
