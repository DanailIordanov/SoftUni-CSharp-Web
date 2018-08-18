namespace WebServer.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidResponseException : Exception
    {
        public InvalidResponseException()
        {
        }

        public InvalidResponseException(string message)
            : base(message)
        {
        }

        public InvalidResponseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidResponseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
