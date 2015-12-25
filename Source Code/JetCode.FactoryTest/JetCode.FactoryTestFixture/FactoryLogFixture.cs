using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactoryLogFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.Log", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryLog detail = new FactoryLog(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\Log.cs", this.SrcDirectory);

            FactoryLog detail = new FactoryLog(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
