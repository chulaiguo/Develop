using System;
using System.Configuration;

namespace JetCode
{
    public static class Identity
    {
        private static SecurityToken _SecurityInfo = null;
        private static ITokenCreator _TokenCreator = null;

        private static ITokenCreator GetTokenCreator()
        {
            Type tokenCreatorType = GetTokenCreatorType();
            return ((tokenCreatorType == null) ? new DefaultTokenCreator() : ((ITokenCreator)Activator.CreateInstance(tokenCreatorType, null)));
        }

        private static Type GetTokenCreatorType()
        {
            string str = ConfigurationManager.AppSettings["TokenCreator"];
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return Type.GetType(str);
        }

        public static void ResetQueryParas()
        {
            if (_SecurityInfo != null)
            {
                _SecurityInfo.ResetQueryParas();
            }
        }

        public static void SetToken(string userid, string password)
        {
            _SecurityInfo = new SecurityToken(userid, password);
        }

        public static SecurityToken Token
        {
            get
            {
                if (_SecurityInfo != null)
                {
                    return _SecurityInfo;
                }
                if (_TokenCreator == null)
                {
                    _TokenCreator = GetTokenCreator();
                }
                return _TokenCreator.NewToken();
            }
        }
    }
}
