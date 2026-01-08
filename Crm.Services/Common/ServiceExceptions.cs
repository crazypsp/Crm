namespace Crm.Services.Common
{
    public abstract class ServiceException : Exception
    {
        protected ServiceException(string message) : base(message) { }
        protected ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }

    public sealed class UnsupportedFileException : ServiceException
    {
        public UnsupportedFileException(string message) : base(message) { }
        public UnsupportedFileException(string message, Exception innerException) : base(message, innerException) { }
    }

    public sealed class StorageException : ServiceException
    {
        public StorageException(string message) : base(message) { }
        public StorageException(string message, Exception innerException) : base(message, innerException) { }
    }

    public sealed class ParseException : ServiceException
    {
        public ParseException(string message) : base(message) { }
        public ParseException(string message, Exception innerException) : base(message, innerException) { }
    }
}