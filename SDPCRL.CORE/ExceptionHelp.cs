using System;
using System.Collections.Generic;
using System.Text;

namespace SDPCRL.CORE
{
    public class ExceptionHelp
    {
        //IlibException ilibException = null;
        public ExceptionHelp()
        {

        }

        public void ThrowError<T>(T obj, string msg)
        {
            IlibException exception = obj as IlibException;
            if (exception != null)
            {
                exception.BeforeThrow();
            }
            throw new LibExceptionBase(msg);
        }
        public void AddMessage<T>(T obj, string msg, MessageType messageType)
        {

        }

    }

    public enum MessageType
    {
        Error = 1,
        Warning = 2,
        Message

    }
}
