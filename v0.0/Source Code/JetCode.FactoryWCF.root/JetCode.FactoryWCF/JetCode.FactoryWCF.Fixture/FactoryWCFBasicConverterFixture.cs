using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryWCFBasicConverterFixture : FixtureBase
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
            get { return string.Format(@"{0}\WCF.BasicConverter", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryWCFBasicConverter detail = new FactoryWCFBasicConverter(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\BasicConverter.cs", this.SrcDirectory);

            FactoryWCFBasicConverter entity = new FactoryWCFBasicConverter(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}