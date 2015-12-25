using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryGetByMapParentFixture : FixtureBase
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
            get { return string.Format(@"{0}\DataService.GetByMapParentCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryGetByMapParent detail = new FactoryGetByMapParent(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\GetByMapParentMethods.cs", this.SrcDirectory);
            FactoryGetByMapParent detail = new FactoryGetByMapParent(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
