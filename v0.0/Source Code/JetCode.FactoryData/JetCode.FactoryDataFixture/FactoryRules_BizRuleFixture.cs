using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryData;
using NUnit.Framework;

namespace JetCode.FactoryDataFixture
{
    [TestFixture]
    public class FactoryRules_BizRuleFixture : FixtureBase
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
            get { return string.Format(@"{0}\Rules.BizRule", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                FactoryRules_BizRule detail = new FactoryRules_BizRule(base.Schema, item);
                base.WriteToScreen(detail);
            }
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            foreach (ObjectSchema item in base.Schema.Objects)
            {
                string detailFileName = string.Format(@"{0}\{1}Rule.cs", this.SrcDirectory, item.Alias);

                FactoryRules_BizRule detail = new FactoryRules_BizRule(base.Schema, item);
                base.WriteToFile(detailFileName, detail);
            }
        }
    }
}
