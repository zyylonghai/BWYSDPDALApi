using System;
using System.Collections.Generic;
using System.Text;

namespace SDPCRL.CORE
{
    public class LibSqlExceptionEventSource
    {
        private object _subscribeobj;
        private object _touchobj;
        public LibSqlExceptionEventSource(object subscribeObj, object touchObj)
        {
            this._subscribeobj = subscribeObj;
            this._touchobj = touchObj;
        }
        public object SubscribeObj { get { return _subscribeobj; } }
        public object TouchObj { get { return _touchobj; } }
    }
}
