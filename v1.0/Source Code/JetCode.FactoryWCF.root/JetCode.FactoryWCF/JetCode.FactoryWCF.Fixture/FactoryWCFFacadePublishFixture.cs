using System.Collections.Generic;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryWCFFacadePublishFixture : FixtureBase
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
            get { return string.Format(@"{0}\WCF.FacadePublish", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryWCFFacadePublish detail = new FactoryWCFFacadePublish(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            FactoryWCFFacadePublish entity = new FactoryWCFFacadePublish(base.Schema);
            entity.GenCode();
            foreach (KeyValuePair<string, string> pair in entity.List)
            {
                string fileName = string.Format(@"{0}\{1}.svc", this.SrcDirectory, pair.Key);
                base.WriteToFile(fileName, pair.Value);
            }
        }
    }
}