using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactoryMicroDataFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.MicroData", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryMicroData detail = new FactoryMicroData(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\MicroData.cs", this.SrcDirectory);
            FactoryMicroData detail = new FactoryMicroData(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
