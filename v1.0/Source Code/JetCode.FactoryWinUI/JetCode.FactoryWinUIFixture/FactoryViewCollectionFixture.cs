using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
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
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                FactoryViewCollection design = new FactoryViewCollection(base.Schema, item);
                base.WriteToScreen(design);
            }
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            foreach (ObjectSchema item in base.Schema.Objects)
            {
                string viewCollectionFileName = string.Format(@"{0}\{1}ViewCollection.cs", this.SrcDirectory, item.Alias);

                FactoryViewCollection viewCollection = new FactoryViewCollection(base.Schema, item);
                base.WriteToFile(viewCollectionFileName, viewCollection);
            }
        }
    }
}
