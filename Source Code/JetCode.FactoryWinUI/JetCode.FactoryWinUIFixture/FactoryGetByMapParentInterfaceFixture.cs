using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryGetByMapParentInterfaceFixture : FixtureBase
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
            get { return string.Format(@"{0}\DataService.GetByMapParentCodeInterface", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryGetByMapParentInterface detail = new FactoryGetByMapParentInterface(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\IGetByMapParentMethods.cs", this.SrcDirectory);
            FactoryGetByMapParentInterface detail = new FactoryGetByMapParentInterface(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
