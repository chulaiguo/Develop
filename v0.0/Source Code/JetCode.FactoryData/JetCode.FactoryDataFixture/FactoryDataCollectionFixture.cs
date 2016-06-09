using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryDataFixture
{
    [TestFixture]
    public class FactoryDataCollectionFixture : FixtureBase
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
            get { return string.Format(@"{0}\Data_DataListObj", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryData.FactoryDataCollection detail = new FactoryData.FactoryDataCollection(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\DataList.cs", this.SrcDirectory);

            FactoryData.FactoryDataCollection detail = new FactoryData.FactoryDataCollection(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
