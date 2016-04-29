using System;

namespace ImportBookings.Exceptions
{
    public class SendErrorException : Exception
    {
        public SendErrorException()
            : base()
        { }

        public SendErrorException(string message)
            : base(message)
        { }

        public SendErrorException(string format, params object[] args)
            : base(string.Format(format, args))
        { }

        public SendErrorException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public SendErrorException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        { }
    }
}
