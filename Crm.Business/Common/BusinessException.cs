
namespace Crm.Business.Common
{
    public abstract class BusinessException : Exception
    {
        protected BusinessException(string message) : base(message) { }
        protected BusinessException(string message, Exception innerException) : base(message, innerException) { }
    }

    public sealed class NotFoundException : BusinessException
    {
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public sealed class ForbiddenException : BusinessException
    {
        public ForbiddenException(string message) : base(message) { }
        public ForbiddenException(string message, Exception innerException) : base(message, innerException) { }
    }

    public sealed class ValidationException : BusinessException
    {
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
