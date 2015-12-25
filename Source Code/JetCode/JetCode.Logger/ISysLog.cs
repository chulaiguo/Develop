using System;
using JetCode.Configuration;

namespace JetCode.Logger
{
    public interface ISysLog
    {
        void WriteDebug(string msg);
        void WriteDebug(string msg, Exception ex);
        void WriteError(string msg);
        void WriteError(string msg, Exception ex);
        void WriteFatal(string msg);
        void WriteFatal(string msg, Exception ex);
        void WriteInfo(string msg);
        void WriteInfo(string msg, Exception ex);
        void WriteWarn(string msg);
        void WriteWarn(string msg, Exception ex);

        bool IsDebugEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }
    }

    public class DefaultSysLog : ISysLog
    {
        void ISysLog.WriteDebug(string msg)
        {
        }

        void ISysLog.WriteDebug(string msg, Exception ex)
        {
        }

        void ISysLog.WriteError(string msg)
        {
        }

        void ISysLog.WriteError(string msg, Exception ex)
        {
        }

        void ISysLog.WriteFatal(string msg)
        {
        }

        void ISysLog.WriteFatal(string msg, Exception ex)
        {
        }

        void ISysLog.WriteInfo(string msg)
        {
        }

        void ISysLog.WriteInfo(string msg, Exception ex)
        {
        }

        void ISysLog.WriteWarn(string msg)
        {
        }

        void ISysLog.WriteWarn(string msg, Exception ex)
        {
        }

        bool ISysLog.IsDebugEnabled
        {
            get { return true; }
        }

        bool ISysLog.IsErrorEnabled
        {
            get { return true; }
        }

        bool ISysLog.IsFatalEnabled
        {
            get { return true; }
        }

        bool ISysLog.IsInfoEnabled
        {
            get { return true; }
        }

        bool ISysLog.IsWarnEnabled
        {
            get { return true; }
        }
    }

    public class SysLogBuilder
    {
        public static ISysLog GetSysLog()
        {
            try
            {
                return (ISysLog)ClassFactory.GetFactory("SysLog");
            }
            catch
            {
                return new DefaultSysLog();
            }
        }
    }
}