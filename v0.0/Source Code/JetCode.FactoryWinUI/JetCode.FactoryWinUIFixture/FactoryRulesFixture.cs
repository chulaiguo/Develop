using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryRulesFixture : FixtureBase
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
            get { return string.Format(@"{0}\RulesCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                if(item.IsMultiPK)
                    continue;

                FactoryRules detail = new FactoryRules(base.Schema, item);
                base.WriteToScreen(detail);
            }
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            foreach (ObjectSchema item in base.Schema.Objects)
            {
                if (item.IsMultiPK)
                    continue;

                string viewFileName = string.Format(@"{0}\{1}Data.cs", this.SrcDirectory, item.Alias);

                FactoryRules view = new FactoryRules(base.Schema, item);
                base.WriteToFile(viewFileName, view);
            }
        }
    }
}
