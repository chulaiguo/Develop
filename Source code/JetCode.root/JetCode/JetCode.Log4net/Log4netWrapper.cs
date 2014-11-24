using System;
using JetCode.Logger;
using log4net;

namespace JetCode.Log4net
{
    public class Log4netWrapper : ISysLog
    {
        private static readonly ILog _Log = LogManager.GetLogger("JetCode.Logger");

        public void WriteDebug(string msg)
        {
            _Log.Debug(msg);
        }

        public void WriteDebug(string msg, Exception ex)
        {
            _Log.Debug(msg, ex);
        }

        public void WriteError(string msg)
        {
            _Log.Error(msg);
        }

        public void WriteError(string msg, Exception ex)
        {
            _Log.Error(msg, ex);
        }

        public void WriteFatal(string msg)
        {
            _Log.Fatal(msg);
        }

        public void WriteFatal(string msg, Exception ex)
        {
            _Log.Fatal(msg, ex);
        }

        public void WriteInfo(string msg)
        {
            _Log.Info(msg);
        }

        public void WriteInfo(string msg, Exception ex)
        {
            _Log.Info(msg, ex);
        }

        public void WriteWarn(string msg)
        {
            _Log.Warn(msg);
        }

        public void WriteWarn(string msg, Exception ex)
        {
            _Log.Warn(msg, ex);
        }

        public bool IsDebugEnabled
        {
            get { return _Log.IsDebugEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return _Log.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return _Log.IsFatalEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return _Log.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return _Log.IsWarnEnabled; }
        }
    }
}