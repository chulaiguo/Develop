using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryData;
using NUnit.Framework;

namespace JetCode.FactoryDataFixture
{
    [TestFixture]
    public class FactoryDataViewCollectionFixture : FixtureBase
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
            get { return string.Format(@"{0}\Data_DataViewList", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDataViewCollection design = new FactoryDataViewCollection(base.Schema);
            base.WriteToScreen(design);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

           
            string viewCollectionFileName = string.Format(@"{0}\ViewCollection.cs", this.SrcDirectory);

            FactoryDataViewCollection viewCollection = new FactoryDataViewCollection(base.Schema);
            base.WriteToFile(viewCollectionFileName, viewCollection);
        }
    }
}
