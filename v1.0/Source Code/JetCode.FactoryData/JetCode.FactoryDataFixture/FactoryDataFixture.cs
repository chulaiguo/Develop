using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryDataFixture
{
    [TestFixture]
    public class FactoryDataFixture : FixtureBase
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
            get { return string.Format(@"{0}\Data_DataObj", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryData.FactoryData detail = new FactoryData.FactoryData(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\DataObj.cs", this.SrcDirectory);

            FactoryData.FactoryData detail = new FactoryData.FactoryData(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
