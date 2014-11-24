using System.Data;
using JetCode.Configuration;
using JetCode.BusinessEntity;

namespace JetCode.DataServiceBase
{
    public interface IDBEditLog
    {
        int LogDelete(DataTable list, BusinessBase parent, string userid);
        int LogInsert(BusinessBase entity, string userid);
        int LogUpdate(BusinessBase entity, string userid);
    }

    internal class DefaultDBLog : IDBEditLog
    {
        public int LogDelete(DataTable list, BusinessBase parent, string userid)
        {
            return 1;
        }

        public int LogInsert(BusinessBase entity, string userid)
        {
            return 1;
        }

        public int LogUpdate(BusinessBase entity, string userid)
        {
            return 1;
        }
    }

    internal class DBEditLogBuilder
    {
        internal static IDBEditLog GetDBEditLog()
        {
            try
            {
                return (IDBEditLog)ClassFactory.GetFactory("DBEditLog");
            }
            catch
            {
                return new DefaultDBLog();
            }
        }
    }
}
