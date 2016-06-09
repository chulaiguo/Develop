using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryClientBasicObjCollectionFixture : FixtureBase
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
            get { return string.Format(@"{0}\Sliverlight.BasicObjCollection", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryClientBasicObjCollection detail = new FactoryClientBasicObjCollection(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\ViewObjCollection.cs", this.SrcDirectory);

            FactoryClientBasicObjCollection entity = new FactoryClientBasicObjCollection(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}