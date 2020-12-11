using System;

namespace Application.Common.Exceptions
{
    public class ExistingRecordException : Exception
    {
        public ExistingRecordException()
            : base()
        {
        }

        public ExistingRecordException(string message)
            : base(message)
        {
        }

        public ExistingRecordException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ExistingRecordException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}
