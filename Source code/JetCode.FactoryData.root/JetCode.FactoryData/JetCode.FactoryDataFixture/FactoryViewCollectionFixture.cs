using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryData;
using NUnit.Framework;

namespace JetCode.FactoryDataFixture
{
    [TestFixture]
    public class FactoryViewCollectionFixture : FixtureBase
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
            get { return string.Format(@"{0}\Data.ViewCollectionCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryViewCollection design = new FactoryViewCollection(base.Schema);
            base.WriteToScreen(design);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

           
            string viewCollectionFileName = string.Format(@"{0}\ViewCollection.cs", this.SrcDirectory);

            FactoryViewCollection viewCollection = new FactoryViewCollection(base.Schema);
            base.WriteToFile(viewCollectionFileName, viewCollection);
        }
    }
}
