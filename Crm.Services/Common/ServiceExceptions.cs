
namespace Crm.Services.Common
{
    public abstract class ServiceException : Exception
    {
        protected ServiceException(string message) : base(message) { }
    }

    public sealed class UnsupportedFileException : ServiceException
    {
        public UnsupportedFileException(string message) : base(message) { }
    }

    public sealed class StorageException : ServiceException
    {
        public StorageException(string message) : base(message) { }
    }

    public sealed class ParseException : ServiceException
    {
        public ParseException(string message) : base(message) { }
    }
}
