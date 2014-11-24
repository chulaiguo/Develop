using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryClientBasicObjConverterFixture : FixtureBase
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
            get { return string.Format(@"{0}\Sliverlight.BasicObjConverter", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryClientBasicObjConverter detail = new FactoryClientBasicObjConverter(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\ViewObjConverter.cs", this.SrcDirectory);

            FactoryClientBasicObjConverter entity = new FactoryClientBasicObjConverter(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}