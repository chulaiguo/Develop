using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactoryMicroServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.MicroService", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryMicroService detail = new FactoryMicroService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\MicroService.cs", this.SrcDirectory);
            FactoryMicroService detail = new FactoryMicroService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
