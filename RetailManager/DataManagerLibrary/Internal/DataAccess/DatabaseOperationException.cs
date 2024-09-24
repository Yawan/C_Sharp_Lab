using System;
using System.Runtime.Serialization;

namespace DataManager.Library.Internal.DataAccess
{
    [Serializable]
    internal class DatabaseOperationException : Exception
    {
        private string v;
        private object ex;

        public DatabaseOperationException()
        {
        }

        public DatabaseOperationException(string message) : base(message)
        {
        }

        public DatabaseOperationException(string v, object ex)
        {
            this.v = v;
            this.ex = ex;
        }

        public DatabaseOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DatabaseOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}