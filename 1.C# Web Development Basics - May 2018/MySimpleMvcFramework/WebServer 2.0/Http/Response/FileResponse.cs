﻿namespace WebServer.Http.Response
{
    using Enums;
    using Exceptions;
    using Common;

    public class FileResponse : HttpResponse
    {
        public FileResponse(HttpStatusCode statusCode, byte[] fileContents)
        {
            CoreValidator.ThrowIfNull(fileContents, nameof(fileContents));

            this.ValidateStatusCode(statusCode);
            this.StatusCode = statusCode;
            this.FileData = fileContents;

            this.Headers.Add(HttpHeader.ContentLength, fileContents.Length.ToString());
            this.Headers.Add(HttpHeader.ContentDisposition, "attachment");
        }

        public byte[] FileData { get; }

        private void ValidateStatusCode(HttpStatusCode statusCode)
        {
            var statusCodeNumber = (int)statusCode;

            if (statusCodeNumber <= 299 || statusCodeNumber >= 400)
            {
                throw new InvalidResponseException("File responses need a status code between 300 and 399 (inclusive).");
            }
        }
    }
}
