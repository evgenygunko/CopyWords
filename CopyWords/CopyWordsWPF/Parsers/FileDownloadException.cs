using System;

namespace CopyWordsWPF.Parsers
{
    public class FileDownloadException : Exception
    {
        public FileDownloadException()
        {
        }

        public FileDownloadException(string message)
            : base(message)
        {
        }

        public FileDownloadException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected FileDownloadException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
