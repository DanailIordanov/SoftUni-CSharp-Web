namespace Mvc.Framework.Controllers
{
    using Attributes.Validation;
    using Contracts;
    using Models;
    using ViewEngine;

    using System.Runtime.CompilerServices;
    using System.Reflection;
    using System.Linq;

    public class Controller
    {
        public Controller()
        {
            this.ViewModel = new ViewModel();
        }

        protected ViewModel ViewModel { get; private set; }

        protected IViewable View([CallerMemberName]string caller = "")
        {
            var controllerName = this.GetType().Name.Replace(MvcContext.Get.ControllersSuffix, string.Empty);

            var fullViewName = string.Format(
                            "{0}.{1}.{2}.{3}, {0}",
                            MvcContext.Get.AssemblyName,
                            MvcContext.Get.ViewsFolder,
                            controllerName,
                            caller);

            var view = new View(fullViewName, this.ViewModel.Data);

            return new ViewResult(view);
        }

        protected IRedirectable Redirect(string redirectUrl) => new RedirectResult(redirectUrl);

        protected IActionResult NotFound() => new NotFoundResult();

        protected bool IsValidModel(object model)
        {
            var properties = model.GetType().GetProperties();

            foreach (var property in properties)
            {
                var attributes = property
                    .GetCustomAttributes()
                    .Where(a => a is PropertyValidationAttribute)
                    .Cast<PropertyValidationAttribute>();
                
                foreach (var attribute in attributes)
                {
                    var propertyValue = property.GetValue(model);

                    if (!attribute.IsValid(propertyValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
