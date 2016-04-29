using System;

namespace ImportBookings.Exceptions
{
    public class KeyAlreadyExistsException : Exception
    {
        public KeyAlreadyExistsException()
            : base()
        { }

        public KeyAlreadyExistsException(string message)
            : base(message)
        { }

        public KeyAlreadyExistsException(string format, params object[] args)
            : base(string.Format(format, args))
        { }

        public KeyAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public KeyAlreadyExistsException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        { }
    }
}
