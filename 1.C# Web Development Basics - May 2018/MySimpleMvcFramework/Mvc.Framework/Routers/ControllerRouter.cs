namespace Mvc.Framework.Routers
{
    using Attributes.Methods;
    using Controllers;
    using WebServer.Contracts;
    using WebServer.Http.Contracts;
    using WebServer.Http.Response;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Mvc.Framework.Contracts;

    public class ControllerRouter : IHandleable
    {
        private IDictionary<string, string> getParams;
        private IDictionary<string, string> postParams;
        private string requestMethod;
        private string controllerName;
        private string actionName;
        private object[] methodParameters;

        public IHttpResponse Handle(IHttpRequest request)
        {
            this.getParams = new Dictionary<string, string>(request.UrlParameters);
            this.postParams = new Dictionary<string, string>(request.FormData);
            this.requestMethod = request.Method.ToString().ToUpper();

            this.PrepareControllerAndActionNames(request);

            var methodInfo = this.GetActionForExecution();

            if (methodInfo == null)
            {
                return new NotFoundResponse();
            }

            this.PrepareMethodParameters(methodInfo);

            try
            {
                return this.GetResponse(methodInfo, this.GetControllerInstance());
            }
            catch (Exception ex)
            {
                return new InternalServerErrorResponse(ex);
            }
        }

        private void PrepareControllerAndActionNames(IHttpRequest request)
        {
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
        }

        private string CapitalizeFirstLetter(string str) => char.ToUpper(str[0]) + str.Substring(1);

        private MethodInfo GetActionForExecution()
        {
            foreach (MethodInfo methodInfo in this.GetSuitableMethods())
            {
                var attributes = methodInfo
                    .GetCustomAttributes()
                    .Where(a => a is HttpMethodAttribute)
                    .Cast<HttpMethodAttribute>();

                if (!attributes.Any() && this.requestMethod == "GET")
                {
                    return methodInfo;
                }

                foreach (var attribute in attributes)
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
            var controller = this.GetControllerInstance();
            if (controller == null)
            {
                return new MethodInfo[0];
            }

            return this.GetControllerInstance()
                .GetType()
                .GetMethods()
                .Where(m => m.Name == this.actionName);
        }

        private Controller GetControllerInstance()
        {
            var controllerFullName = string.Format(
                                "{0}.{1}.{2}, {0}",
                                MvcContext.Get.AssemblyName,
                                MvcContext.Get.ControllersFolder,
                                this.controllerName);

            var type = Type.GetType(controllerFullName);
            if (type == null)
            {
                return null;
            }

            var controller = (Controller)Activator.CreateInstance(type);

            return controller;
        }
        
        private void PrepareMethodParameters(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            this.methodParameters = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var currentParam = parameters[i];

                if (currentParam.ParameterType.IsPrimitive || currentParam.ParameterType == typeof(string))
                {
                    this.ProcessPrimitiveParameter(currentParam, i);
                }
                else
                {
                    this.ProcessModelParameter(currentParam, i);
                }
            }
        }
        
        private void ProcessPrimitiveParameter(ParameterInfo parameter, int index)
        {
            var parameterValue = this.getParams[parameter.Name];
            this.methodParameters[index] = Convert.ChangeType(parameterValue, parameter.ParameterType);
        }

        private void ProcessModelParameter(ParameterInfo parameter, int index)
        {
            var bindingModelType = parameter.ParameterType;
            var bindingModel = Activator.CreateInstance(bindingModelType);

            var properties = bindingModelType.GetProperties();
            foreach (var property in properties)
            {
                property.SetValue(
                    bindingModel,
                    Convert.ChangeType(this.postParams[property.Name], property.PropertyType)
                    );
            }

            this.methodParameters[index] = Convert.ChangeType(bindingModel, bindingModelType);
        }

        private IHttpResponse GetResponse(MethodInfo methodInfo, object controller)
        {
            var actionResult = methodInfo.Invoke(controller, this.methodParameters) as IActionResult;

            if (actionResult == null)
            {
                var actionResultAsHttpResponse = actionResult as IHttpResponse;

                if (actionResultAsHttpResponse == null)
                {
                    throw new InvalidOperationException("Controller actions should return either IActionResult or IHttpResponse!");
                }

                return actionResultAsHttpResponse;
            }

            return actionResult.Invoke();
        }
    }
}
