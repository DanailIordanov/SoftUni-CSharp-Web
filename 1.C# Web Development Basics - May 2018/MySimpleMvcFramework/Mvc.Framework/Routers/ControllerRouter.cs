namespace Mvc.Framework.Routers
{
    using Attributes.Methods;
    using Contracts;
    using Controllers;
    using WebServer.Enums;
    using WebServer.Contracts;
    using WebServer.Http.Contracts;
    using WebServer.Http.Response;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    
    public class ControllerRouter : IHandleable
    {
        private IDictionary<string, string> getParams;
        private IDictionary<string, string> postParams;
        private string requestMethod;
        private string controllerName;
        private string actionName;
        private object[] methodParams;

        public IHttpResponse Handle(IHttpRequest request)
        {
            this.getParams = request.UrlParameters;
            this.postParams = request.FormData;
            this.requestMethod = request.Method.ToString();

            var pathParams = request.Url.Split('/');
            var controllerName = string.Empty;
            var actionName = string.Empty;

            if (pathParams.Length >= 3)
            {
                pathParams = pathParams.Skip(1).Take(2).ToArray();

                controllerName = pathParams.First();

                actionName = pathParams.Last();
                if (actionName.Contains("?"))
                {
                    actionName = actionName.Substring(0, actionName.IndexOf('?') + 1);
                }
            }
            else
            {
                throw new InvalidOperationException("Url is not valid.");
            }

            this.controllerName = this.CapitalizeFirstLetter(controllerName) + "Controller";
            this.actionName = this.CapitalizeFirstLetter(actionName);

            var method = this.GetMethod();
            if (method == null)
            {
                return new NotFoundResponse();
            }

            var parameters = method.GetParameters();
            this.methodParams = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var currentParam = parameters[i];

                if (currentParam.ParameterType.IsPrimitive || currentParam.ParameterType == typeof(string))
                {
                    var parameterValue = this.getParams[currentParam.Name];
                    this.methodParams[i] = Convert.ChangeType(parameterValue, currentParam.ParameterType);
                }
                else
                {
                    var bindingModelType = currentParam.ParameterType;
                    var bindingModel = Activator.CreateInstance(bindingModelType);

                    var properties = bindingModelType.GetProperties();
                    foreach (var property in properties)
                    {
                        property.SetValue(
                            bindingModel,
                            Convert.ChangeType(this.postParams[property.Name], property.PropertyType)
                            );
                    }

                    this.methodParams[i] = Convert.ChangeType(bindingModel, bindingModelType);
                }
            }

            var actionResult = (IInvocable)method.Invoke(this.GetController(), this.methodParams);
            var content = actionResult.Invoke();

            return new ContentResponse(HttpStatusCode.Ok, content);
        }

        private MethodInfo GetMethod()
        {
            foreach (var methodInfo in this.GetSuitableMethods())
            {
                var attributes = methodInfo.GetCustomAttributes().Where(a => a is HttpMethodAttribute);

                if (!attributes.Any() && this.requestMethod == "GET")
                {
                    return methodInfo;
                }

                foreach (HttpMethodAttribute attribute in attributes)
                {
                    if (attribute.IsValid(this.requestMethod))
                    {
                        return methodInfo;
                    }
                }
            }

            return null;
        }

        private IEnumerable<MethodInfo> GetSuitableMethods()
        {
            var controller = this.GetController();
            if (controller == null)
            {
                return new MethodInfo[0];
            }

            return controller
                .GetType()
                .GetMethods()
                .Where(m => m.Name == this.actionName);
        }

        private Controller GetController()
        {
            var controllerFullName = string.Format(
                                "{0}.{1}.{2}, {0}",
                                MvcContext.Get.AssemblyName,
                                MvcContext.Get.ControllersFolder,
                                this.controllerName);

            var type = Type.GetType(controllerName);
            if (type == null)
            {
                return null;
            }

            var controller = (Controller)Activator.CreateInstance(type);

            return controller;
        }

        private string CapitalizeFirstLetter(string str) => char.ToUpper(str[0]) + str.Substring(1);
    }
}
