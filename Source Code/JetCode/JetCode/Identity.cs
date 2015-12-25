using System;
using System.Configuration;

namespace JetCode
{
    public static class Identity
    {
        private static ITokenCreator _TokenCreator = null;
        private static SecurityToken _SecurityInfo = null;

        public static SecurityToken Token
        {
            get
            {
                SecurityToken result;
                if (Identity._SecurityInfo != null)
                {
                    result = Identity._SecurityInfo;
                }
                else
                {
                    if (Identity._TokenCreator == null)
                    {
                        Identity._TokenCreator = Identity.GetTokenCreator();
                    }
                    result = Identity._TokenCreator.NewToken();
                }
                return result;
            }
        }

        public static void SetToken(string userid, string password)
        {
            Identity._SecurityInfo = new SecurityToken(userid, password);
        }

        private static Type GetTokenCreatorType()
        {
            string text = ConfigurationManager.AppSettings["TokenCreator"];
            
            Type result;
            if (string.IsNullOrEmpty(text))
            {
                result = null;
            }
            else
            {
                result = Type.GetType(text);
            }
            return result;
        }
        private static ITokenCreator GetTokenCreator()
        {
            Type tokenCreatorType = Identity.GetTokenCreatorType();
            return (tokenCreatorType == null) ? new DefaultTokenCreator() : ((ITokenCreator)Activator.CreateInstance(tokenCreatorType, null));
        }
    }
}
