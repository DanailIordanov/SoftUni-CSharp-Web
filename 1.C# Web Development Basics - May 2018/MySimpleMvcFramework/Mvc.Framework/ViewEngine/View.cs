namespace Mvc.Framework.ViewEngine
{
    using Contracts;
    
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class View : IRenderable
    {
        public const string BaseLayoutFileName = "Layout";
        public const string ContentPlaceholder = "{{{content}}}";
        public const string HtmlExtension = ".html";
        public const string LocalErrorPath = @"\Mvc.Framework\Errors\Error.html";

        private readonly string viewFullPath;
        private readonly IDictionary<string, string> viewData;

        public View(string viewFullPath, IDictionary<string, string> viewData)
        {
            this.viewFullPath = viewFullPath;
            this.viewData = viewData;
        }

        public string Render()
        {
            var resultHtml = this.ReadFile();

            if (this.viewData.Any())
            {
                foreach (var data in this.viewData)
                {
                    resultHtml = resultHtml.Replace($"{{{{{{{data.Key}}}}}}}", data.Value);
                }
            }

            return resultHtml;
        }

        private string ReadFile()
        {
            var layoutHtml = this.ReadLayoutFile();

            var viewFullName = $"{this.viewFullPath}{HtmlExtension}";

            if (!File.Exists(viewFullName))
            {
                return this.GetErrorHtml($"The requested view {viewFullName} could not be found!");
            }

            var viewHtml = File.ReadAllText(viewFullName);

            layoutHtml = layoutHtml.Replace(ContentPlaceholder, viewHtml);

            return layoutHtml;
        }

        private string ReadLayoutFile()
        {
            var layoutHtmlPath = string.Format(
                @"{0}\{1}{2}",
                MvcContext.Get.ViewsFolder,
                BaseLayoutFileName,
                HtmlExtension);

            if (!File.Exists(layoutHtmlPath))
            {
                return this.GetErrorHtml($"Layout view {layoutHtmlPath} could not be found!");
            }

            var layoutHtml = File.ReadAllText(layoutHtmlPath);

            return layoutHtml;
        }

        private string GetErrorHtml(string error)
        {
            var errorPath = this.GetErrorPath();
            var errorHtml = File.ReadAllText(errorPath);

            this.viewData["error"] = error;

            return errorHtml;
        }
        
        private string GetErrorPath()
        {
            var appDirectoryPath = Directory.GetCurrentDirectory();
            var parentDirectoryPath = Directory.GetParent(appDirectoryPath).FullName;

            var errorPath = $"{parentDirectoryPath}{LocalErrorPath}";

            return errorPath;
        }
    }
}
