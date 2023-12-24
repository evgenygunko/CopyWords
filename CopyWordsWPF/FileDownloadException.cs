using System;

namespace CopyWordsWPF
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
    }
}
