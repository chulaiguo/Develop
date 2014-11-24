using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryUtilityFixture : FixtureBase
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        protected override string SrcDirectory
        {
            get { return string.Format(@"{0}\WinUI.UtilityCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryLookupBuilder builder = new FactoryLookupBuilder(base.Schema);
            base.WriteToScreen(builder);

            FactoryLoginService login = new FactoryLoginService(base.Schema);
            base.WriteToScreen(login);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            FactoryLookupBuilder builder = new FactoryLookupBuilder(base.Schema);
            string builderFileName = string.Format(@"{0}\LookUpEditBuilder.cs", this.SrcDirectory);
            base.WriteToFile(builderFileName, builder);

            FactoryLoginService login = new FactoryLoginService(base.Schema);
            string loginFileName = string.Format(@"{0}\LoginService.cs", this.SrcDirectory);
            base.WriteToFile(loginFileName, login);

        }
    }
}
