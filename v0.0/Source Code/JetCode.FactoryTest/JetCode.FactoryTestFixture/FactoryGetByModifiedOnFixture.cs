using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactoryGetByModifiedOnFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.GetByModifiedOn", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryGetByModifiedOn detail = new FactoryGetByModifiedOn(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\GetByModifiedOn.cs", this.SrcDirectory);
            FactoryGetByModifiedOn detail = new FactoryGetByModifiedOn(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
