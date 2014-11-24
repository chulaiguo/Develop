using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace JetCode.Logger
{
    public class ApplicationLog
    {
        private static readonly ISysLog _SysLog = SysLogBuilder.GetSysLog();

        private string GetFormattedMessage(string mesg)
        {
            StackTrace trace = new StackTrace();
            MethodBase method = trace.GetFrame(2).GetMethod();
            return string.Format("[{0}][{1}.{2}][{3}]", trace.FrameCount, method.DeclaringType.Name, this.GetMethodInfo(method), mesg);
        }

        private string GetMethodInfo(MethodBase method)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}(", method.Name);
            ParameterInfo[] parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                builder.Append(parameters[i].ParameterType);
                if (i != (parameters.Length - 1))
                {
                    builder.Append(", ");
                }
            }
            builder.Append(")");
            return builder.ToString();
        }

        private string GetSecurityTokenInfo(SecurityToken token)
        {
            return string.Format("(UserId = {0}, Password = {1}, Secret = {2}, Ticks = {3})", new object[] { token.UserId, token.Password, token.Secret, token.Ticks });
        }

        public void WriteDebug(string mesg)
        {
            if (_SysLog.IsDebugEnabled)
            {
                _SysLog.WriteDebug(this.GetFormattedMessage(mesg));
            }
        }

        public void WriteDebug(string mesg, Exception ex)
        {
            if (_SysLog.IsDebugEnabled)
            {
                _SysLog.WriteDebug(this.GetFormattedMessage(mesg), ex);
            }
        }

        public void WriteDebugMethodBegin()
        {
            if (_SysLog.IsDebugEnabled)
            {
                _SysLog.WriteDebug(this.GetFormattedMessage("Started successfully ..."));
            }
        }

        public void WriteDebugMethodBegin(SecurityToken token)
        {
            if (_SysLog.IsDebugEnabled)
            {
                _SysLog.WriteDebug(this.GetFormattedMessage(string.Format("Started successfully with token {0}", this.GetSecurityTokenInfo(token))));
            }
        }

        public void WriteDebugMethodEnd()
        {
            if (_SysLog.IsDebugEnabled)
            {
                _SysLog.WriteDebug(this.GetFormattedMessage("Finished successfully ..."));
            }
        }

        public void WriteDebugMethodEnd(SecurityToken token)
        {
            if (_SysLog.IsDebugEnabled)
            {
                _SysLog.WriteDebug(this.GetFormattedMessage(string.Format("Finished successfully with token {0}", this.GetSecurityTokenInfo(token))));
            }
        }

        public void WriteError(string mesg)
        {
            if (_SysLog.IsDebugEnabled)
            {
                _SysLog.WriteError(this.GetFormattedMessage(mesg));
            }
        }

        public void WriteError(string mesg, Exception ex)
        {
            if (_SysLog.IsDebugEnabled)
            {
                _SysLog.WriteError(this.GetFormattedMessage(mesg), ex);
            }
        }

        public void WriteFatal(string mesg)
        {
            if (_SysLog.IsDebugEnabled)
            {
                _SysLog.WriteFatal(this.GetFormattedMessage(mesg));
            }
        }

        public void WriteFatal(string mesg, Exception ex)
        {
            if (_SysLog.IsDebugEnabled)
            {
                _SysLog.WriteFatal(this.GetFormattedMessage(mesg), ex);
            }
        }

        public void WriteInfo(string mesg)
        {
            if (_SysLog.IsDebugEnabled)
            {
                _SysLog.WriteInfo(this.GetFormattedMessage(mesg));
            }
        }

        public void WriteInfo(string mesg, Exception ex)
        {
            if (_SysLog.IsDebugEnabled)
            {
                _SysLog.WriteInfo(this.GetFormattedMessage(mesg), ex);
            }
        }

        public void WriteInfoMethodBegin()
        {
            if (_SysLog.IsInfoEnabled)
            {
                _SysLog.WriteInfo(this.GetFormattedMessage("Started successfully ..."));
            }
        }

        public void WriteInfoMethodBegin(SecurityToken token)
        {
            if (_SysLog.IsInfoEnabled)
            {
                _SysLog.WriteInfo(this.GetFormattedMessage(string.Format("Started successfully with token {0}", this.GetSecurityTokenInfo(token))));
            }
        }

        public void WriteInfoMethodEnd()
        {
            if (_SysLog.IsInfoEnabled)
            {
                _SysLog.WriteInfo(this.GetFormattedMessage("Finished successfully ..."));
            }
        }

        public void WriteInfoMethodEnd(SecurityToken token)
        {
            if (_SysLog.IsInfoEnabled)
            {
                _SysLog.WriteInfo(this.GetFormattedMessage(string.Format("Finished successfully with token {0}", this.GetSecurityTokenInfo(token))));
            }
        }

        public void WriteWarn(string mesg)
        {
            if (_SysLog.IsDebugEnabled)
            {
                _SysLog.WriteWarn(this.GetFormattedMessage(mesg));
            }
        }

        public void WriteWarn(string mesg, Exception ex)
        {
            if (_SysLog.IsDebugEnabled)
            {
                _SysLog.WriteWarn(this.GetFormattedMessage(mesg), ex);
            }
        }
    }
}
