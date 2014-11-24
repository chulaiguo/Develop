using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactoryMicroIServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.MicroIService", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryMicroIService detail = new FactoryMicroIService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\IMicroService.cs", this.SrcDirectory);
            FactoryMicroIService detail = new FactoryMicroIService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
