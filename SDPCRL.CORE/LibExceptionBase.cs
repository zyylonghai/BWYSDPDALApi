using System;
using System.Collections.Generic;
using System.Text;

namespace SDPCRL.CORE
{
    public class LibExceptionBase : Exception
    {
        public LibExceptionBase()
            : base()
        {

        }

        public LibExceptionBase(string message)
            : base(message)
        {

        }

        public override string Message
        {
            get
            {
                return base.Message;
            }
        }

        #region 公开函数

        public void AddErrorMessage(string msg)
        {

        }
        #endregion
    }

    public interface IlibException
    {
        void BeforeThrow();
    }
}
