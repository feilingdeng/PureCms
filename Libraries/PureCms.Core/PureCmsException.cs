using System;
using System.Runtime.Serialization;

namespace PureCms.Core
{
    [Serializable]
    public class PureCmsException : ApplicationException
    {
        public PureCmsException() { }

        public PureCmsException(string message) : base(message) {

        }

        public PureCmsException(string message, Exception inner) : base(message, inner) { }

        protected PureCmsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
