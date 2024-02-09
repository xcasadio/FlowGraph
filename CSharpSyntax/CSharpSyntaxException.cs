using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CSharpSyntax
{
    [Serializable]
    public class CSharpSyntaxException : Exception
    {
        public CSharpSyntaxException()
        {
        }

        public CSharpSyntaxException(string message)
            : base(message)
        {
        }

        public CSharpSyntaxException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CSharpSyntaxException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
