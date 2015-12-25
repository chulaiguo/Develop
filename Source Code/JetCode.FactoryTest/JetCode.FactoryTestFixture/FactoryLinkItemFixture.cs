using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactoryLinkItemFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.LinkItem", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                if (item.Alias == "ACUserEvent")
                    continue;

                if(item.Alias.EndsWith("Download"))
                    continue;

                FactoryLinkItem detail = new FactoryLinkItem(base.Schema, item);
                base.WriteToScreen(detail);
            }
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            foreach (ObjectSchema item in base.Schema.Objects)
            {
                if (item.Alias == "ACUserEvent" || item.Alias == "UsrAccount")
                    continue;

                if (item.Alias == "LogOnLineActivity" || item.Alias == "LogPhoneLineActivity")
                    continue;

                if (item.Alias == "ACTempModeZone")
                    continue;

                if (item.Alias.EndsWith("Download"))
                    continue;

                string fileName = string.Format(@"{0}\{1}.cs", this.SrcDirectory, item.Alias.Substring(2));

                FactoryLinkItem detail = new FactoryLinkItem(base.Schema, item);
                base.WriteToFile(fileName, detail);
            }
        }
    }
}
