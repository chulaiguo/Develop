using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryWCFFacadeConverterFixture : FixtureBase
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
            get { return string.Format(@"{0}\WCF.FacadeConverter", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryWCFFacadeConverter detail = new FactoryWCFFacadeConverter(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\FacadeConverter.cs", this.SrcDirectory);

            FactoryWCFFacadeConverter entity = new FactoryWCFFacadeConverter(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}