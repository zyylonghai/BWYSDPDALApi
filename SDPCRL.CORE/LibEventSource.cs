using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SDPCRL.CORE
{
    /// <summary> 事件管理者</summary>
    public class LibEventManager
    {
        private static Hashtable _listenerList = new Hashtable();
        private static LibEventListener _eventListener;

        #region 私有函数
        /// <summary>注册监听器</summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private static LibEventListener GetListener(object sender)
        {
            if (!_listenerList.ContainsKey(sender))
            {
                _eventListener = new LibEventListener(sender);
                _listenerList.Add(sender, _eventListener);
            }
            return _listenerList[sender] as LibEventListener;
        }
        #endregion

        #region 公开函数
        /// <summary>订阅事件</summary>
        /// <param name="sender"></param>
        /// <param name="eventType"></param>
        public static void SubscribeEvent(object sender, LibEventType eventType)
        {
            switch (eventType)
            {
                case LibEventType.FormCommunitation:
                case LibEventType.SqlException:
                    GetListener(sender).DoSubscribeEvent(eventType);
                    break;
                    //case LibEventType.ModelEdit :
                    //    GetListener(sender).DoSubscribeEvent(eventType);
                    //    break;
            }
        }
        /// <summary> 触发事件 </summary>
        /// <param name="sender"></param>
        /// <param name="eventType"></param>
        /// <param name="avgs"></param>
        public static void TouchEvent(object sender, LibEventType eventType, params object[] avgs)
        {
            switch (eventType)
            {
                case LibEventType.FormCommunitation:
                    Dictionary<object, object> param = null;
                    string tag = string.Empty;
                    if (avgs != null && avgs.Length > 1)
                    {
                        tag = avgs[0].ToString();
                        param = avgs[1] as Dictionary<object, object>;
                    }
                    GetListener(sender).eventSource.OnFormAcceptMsg(tag, param);
                    break;
                case LibEventType.SqlException:
                    foreach (object item in _listenerList.Keys)
                    {
                        if (item.GetType().Equals(typeof(LibSqlExceptionEventSource)))
                        {
                            LibSqlExceptionEventSource a = (LibSqlExceptionEventSource)item;
                            if (a.TouchObj == sender)
                            {
                                if (avgs != null && avgs.Length > 0)
                                {
                                    (_listenerList[item] as LibEventListener).eventSource.OnSqlException(avgs[0] as Exception);
                                }
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>注销监听器 </summary>
        /// <param name="sender"></param>
        public static void LogOutListener(object sender)
        {
            if (_listenerList.ContainsKey(sender))
                _listenerList.Remove(sender);
        }
        #endregion
        /// <summary> 事件源</summary>
        class LibEventSource
        {
            public delegate void FormCommunitionEventHandle(string tag, Dictionary<object, object> param);
            //public delegate void ModelEditEventHandle(bool ischange);
            public delegate void SqlExceptionEventHandle(Exception ex);

            public event FormCommunitionEventHandle DoFormAcceptMsg;
            public event SqlExceptionEventHandle DoSqlException;
            //public event ModelEditEventHandle DoModelEdit;

            public void OnFormAcceptMsg(string tag, Dictionary<object, object> param)
            {
                if (DoFormAcceptMsg != null)
                    DoFormAcceptMsg(tag, param);
            }
            public void OnSqlException(Exception ex)
            {
                if (DoSqlException != null)
                    DoSqlException(ex);
            }
            //public void OnModelEdit(bool ischange)
            //{
            //    if (DoModelEdit != null)
            //        DoModelEdit(ischange);
            //}
            public LibEventSource(object sender)
            {

            }
            public LibEventSource()
            {

            }
        }
        /// <summary>事件监听器</summary>
        class LibEventListener
        {
            #region 私有变量
            private LibEventSource _eventSource;
            private object _obj;
            #endregion

            #region 构造函数
            public LibEventListener(object obj)
            {
                _obj = obj;
            }
            #endregion
            public LibEventSource eventSource
            {
                get
                {
                    if (_eventSource == null)
                    {
                        _eventSource = new LibEventSource();
                    }
                    return _eventSource;
                }
            }
            /// <summary>订阅事件</summary>
            public void DoSubscribeEvent(LibEventType eventType)
            {
                switch (eventType)
                {
                    case LibEventType.FormCommunitation:
                        eventSource.DoFormAcceptMsg += new LibEventSource.FormCommunitionEventHandle(eventSource_DoFormAcceptMsg);
                        break;
                    case LibEventType.SqlException:
                        eventSource.DoSqlException += new LibEventSource.SqlExceptionEventHandle(eventSource_DoSqlException);
                        break;
                        //case LibEventType.ModelEdit :
                        //    eventSource.DoModelEdit += new LibEventSource.ModelEditEventHandle(eventSource_DoModelEdit);
                        //    break;
                }
            }


            public void DoUnSubscribeEvent(LibEventType eventType)
            {
                switch (eventType)
                {
                    case LibEventType.FormCommunitation:
                        eventSource.DoFormAcceptMsg -= new LibEventSource.FormCommunitionEventHandle(eventSource_DoFormAcceptMsg);
                        break;
                }
            }

            private void eventSource_DoFormAcceptMsg(string tag, Dictionary<object, object> agrs)
            {
                ILibEventListener eventListener = _obj as ILibEventListener;
                if (eventListener != null)
                {
                    LibFormAcceptmsgEventArgs fargs = new LibFormAcceptmsgEventArgs();
                    fargs.ArgsDic = agrs;
                    fargs.Tag = tag;
                    fargs.EventSourse = _obj;
                    eventListener.DoEvents(LibEventType.FormCommunitation, fargs);
                }
                //eventListener.DoFormAcceptMsg(tag, agrs);
            }
            void eventSource_DoSqlException(Exception ex)
            {
                LibSqlExceptionEventSource eventlistener = _obj as LibSqlExceptionEventSource;

            }
            //void eventSource_DoModelEdit(bool ischange)
            //{
            //    ILibModelEventListener eventListener = _obj as ILibModelEventListener;
            //    eventListener.ModelEdit(ischange);
            //}


        }
    }


    public interface ILibEventListener
    {
        //void DoFormAcceptMsg(string tag, Dictionary<object, object> agrs);
        void DoEvents(LibEventType eventType, LibEventArgs args);

    }
    //public interface ILibModelEventListener
    //{
    //    void ModelEdit(bool ischange);
    //}

    public class LibParamEventArgs : EventArgs
    {
        private object[] _param;
        public object Param
        {
            get;
            set;
        }
        public LibParamEventArgs(params object[] param)
        {
            this._param = param;
        }
    }

    public enum LibEventType
    {
        /// <summary>窗体之间通讯事件 </summary>
        FormCommunitation = 0,
        /// <summary>点击事件</summary>
        OnClick = 1,
        /// <summary>执行sql语法异常事件</summary>
        SqlException = 9
        ///// <summary>模型修改事件 </summary>
        //ModelEdit=2
    }
}
