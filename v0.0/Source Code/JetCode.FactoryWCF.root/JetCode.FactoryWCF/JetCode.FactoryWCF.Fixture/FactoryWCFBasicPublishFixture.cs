using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryWCFBasicPublishFixture : FixtureBase
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
            get { return string.Format(@"{0}\WCF.BasicPublish", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                FactoryWCFBasicPublish detail = new FactoryWCFBasicPublish(base.Schema, item);
                base.WriteToScreen(detail);
            }
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            foreach (ObjectSchema item in base.Schema.Objects)
            {
                string fileName = string.Format(@"{0}\{1}.svc", this.SrcDirectory, item.Alias);

                FactoryWCFBasicPublish entity = new FactoryWCFBasicPublish(base.Schema, item);
                base.WriteToFile(fileName, entity);
            }
        }
    }
}