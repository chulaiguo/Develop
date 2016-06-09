using System;

namespace JetCode.Factory
{
    public abstract class UtilityBase
    {
        public abstract Type ToDotNetType(string sqlType);
    }
}