namespace MyHandmadeWebServer.ByTheCake.Infrastructure
{
    using Server.Enums;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Views;

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public abstract class Controller
    {
        private const string DefaultPath = @".\..\..\..\ByTheCake\Resources\{0}.html";
        private const string ContentPlaceholder = "{{{content}}}";

        protected Controller()
        {
            this.ViewData = new Dictionary<string, string>
            {
                ["authDisplay"] = "block",
                ["showError"] = "none"
            };
        }

        protected IDictionary<string, string> ViewData { get; private set; }

        protected IHttpResponse FileViewResponse(string fileName)
        {
            var result = this.ProcessFileHtml(fileName);

            if (this.ViewData.Any())
            {
                foreach (var pair in this.ViewData)
                {
                    result = result.Replace($"{{{{{{{pair.Key}}}}}}}", pair.Value);
                }
            }

            return new ViewResponse(HttpStatusCode.Ok, new FileView(result));
        }

        protected void AddError(string errorMessage)
        {
            this.ViewData["error"] = errorMessage;
            this.ViewData["showError"] = "block";
        }

        private string ProcessFileHtml(string fileName)
        {
            var layoutHtml = File.ReadAllText(string.Format(DefaultPath, "layout"));
            var fileHtml = File.ReadAllText(string.Format(DefaultPath, fileName));

            var result = layoutHtml.Replace(ContentPlaceholder, fileHtml);

            return result;
        }
    }
}
