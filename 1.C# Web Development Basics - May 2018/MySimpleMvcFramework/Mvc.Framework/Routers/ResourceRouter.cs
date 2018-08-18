namespace Mvc.Framework.Routers
{
    using WebServer.Contracts;
    using WebServer.Enums;
    using WebServer.Http.Contracts;
    using WebServer.Http.Response;

    using System.IO;
    using System.Linq;

    public class ResourceRouter : IHandleable
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            var fileName = request.Path.Split("/").Last();
            var fileExtension = fileName.Split(".").Last();

            try
            {
                var fileContents = this.ReadFile(fileName, fileExtension);

                return new FileResponse(HttpStatusCode.Found, fileContents);
            }
            catch
            {
                return new NotFoundResponse();
            }
        }

        private byte[] ReadFile(string fileName, string fileExtension)
        {
            return File
                .ReadAllBytes(
                    string.Format(
                        @"{0}\{1}\{2}",
                        MvcContext.Get.ResourcesFolder,
                        fileExtension,
                        fileName));
        }
    }
}
