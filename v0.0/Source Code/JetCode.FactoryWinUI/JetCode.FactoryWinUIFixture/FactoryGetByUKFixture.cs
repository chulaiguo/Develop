using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryGetByUKFixture : FixtureBase
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
            get { return string.Format(@"{0}\DataService.GetByUKCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryGetByUK2 detail = new FactoryGetByUK2(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\GetByUKMethods.cs", this.SrcDirectory);
            FactoryGetByUK2 detail = new FactoryGetByUK2(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
