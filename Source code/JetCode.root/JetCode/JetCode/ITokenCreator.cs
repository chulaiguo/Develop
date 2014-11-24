using System.Configuration;

namespace JetCode
{
    public interface ITokenCreator
    {
        SecurityToken NewToken();
    }

    public class DefaultTokenCreator : ITokenCreator
    {
        public SecurityToken NewToken()
        {
            return new SecurityToken(ConfigurationManager.AppSettings["AppsUserId"], ConfigurationManager.AppSettings["AppsPassword"], ConfigurationManager.AppSettings["AppsToken"]);
        }
    }
}
