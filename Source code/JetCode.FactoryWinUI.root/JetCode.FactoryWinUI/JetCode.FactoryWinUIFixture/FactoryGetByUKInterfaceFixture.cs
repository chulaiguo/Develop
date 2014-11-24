using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryGetByUKInterfaceFixture : FixtureBase
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
            get { return string.Format(@"{0}\DataService.GetByUKInterfaceCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryGetByUKInterface2 detail = new FactoryGetByUKInterface2(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\IGetByUKMethods.cs", this.SrcDirectory);
            FactoryGetByUKInterface2 detail = new FactoryGetByUKInterface2(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
