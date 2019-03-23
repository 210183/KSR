using System;

namespace FileSamplesRead.Exceptions
{
    public class FileReaderException : ApplicationException
    {
        public FileReaderException(string message) : base(message)
        {
        }
    }
}
