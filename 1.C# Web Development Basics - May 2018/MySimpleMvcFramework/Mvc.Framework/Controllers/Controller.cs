namespace Mvc.Framework.Controllers
{
    using Contracts;
    using Contracts.Generic;
    using ViewEngine;
    using ViewEngine.Generic;

    using System.Runtime.CompilerServices;

    public class Controller
    {
        protected IActionResult View([CallerMemberName]string caller = "")
        {
            var controllerName = this.GetType().Name.Replace(MvcContext.Get.ControllersSuffix, string.Empty);

            var fullViewName = string.Format(
                            "{0}.{1}.{2}.{3}, {0}",
                            MvcContext.Get.AssemblyName,
                            MvcContext.Get.ViewsFolder,
                            controllerName,
                            caller);

            return new ActionResult(fullViewName);
        }

        protected IActionResult View(string controller, string action)
        {
            var fullViewName = string.Format(
                                "{0}.{1}.{2}.{3}, {0}",
                                MvcContext.Get.AssemblyName,
                                MvcContext.Get.ViewsFolder,
                                controller,
                                action);

            return new ActionResult(fullViewName);
        }

        protected IActionResult<T> View<T>(T model, [CallerMemberName]string caller = "")
        {
            var controllerName = this.GetType().Name.Replace(MvcContext.Get.ControllersSuffix, string.Empty);

            var fullViewName = string.Format(
                                "{0}.{1}.{2}.{3}, {0}",
                                MvcContext.Get.AssemblyName,
                                MvcContext.Get.ViewsFolder,
                                controllerName,
                                caller);

            return new ActionResult<T>(fullViewName, model);
        }

        protected IActionResult<T> View<T>(T model, string controller, string action)
        {
            var fullViewName = string.Format(
                                "{0}.{1}.{2}.{3}, {0}",
                                MvcContext.Get.AssemblyName,
                                MvcContext.Get.ViewsFolder,
                                controller,
                                action);

            return new ActionResult<T>(fullViewName, model);
        }
    }
}
