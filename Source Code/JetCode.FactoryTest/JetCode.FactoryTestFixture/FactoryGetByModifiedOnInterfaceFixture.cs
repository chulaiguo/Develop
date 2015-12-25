using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactoryGetByModifiedOnInterfaceFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.IGetByModifiedOn", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryGetByModifiedOnInterface detail = new FactoryGetByModifiedOnInterface(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\IGetByModifiedOn.cs", this.SrcDirectory);
            FactoryGetByModifiedOnInterface detail = new FactoryGetByModifiedOnInterface(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
