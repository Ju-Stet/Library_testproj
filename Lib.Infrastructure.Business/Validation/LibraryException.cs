using System;
using System.Runtime.Serialization;

namespace Lib.Infrastructure.Business.Validation
{
    [Serializable]
    public class LibraryException : Exception
    {
        public LibraryException() { }

        public LibraryException(string message) : base(message) { }

        protected LibraryException(SerializationInfo info, StreamingContext context) : base()
        {
            Source = info.GetValue("Source", typeof(string)).ToString();
            HResult = (Int32)info.GetValue("HResult", typeof(int));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Source", this.Source);
            info.AddValue("HResult", this.HResult);
        }
    }
}
