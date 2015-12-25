using JetCode.Configuration;

namespace JetCode.DataServiceBase
{
    public interface IPermission
    {
        TablePermission GetPermission(SecurityToken token, string table);
    }

    internal class DefaultPermission : IPermission
    {
        public TablePermission GetPermission(SecurityToken token, string table)
        {
            return new TablePermission(true, true, true, true);
        }
    }

    internal class PermissionBuilder
    {
        internal static IPermission GetPermission()
        {
            try
            {
                return (IPermission)ClassFactory.GetFactory("Permission");
            }
            catch
            {
                return new DefaultPermission();
            }
        }
    }
}