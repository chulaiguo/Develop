using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryFixDecimalFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.FixDecimalCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryFixDecimal detail = new FactoryFixDecimal(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\FixDecimals.cs", this.SrcDirectory);
            FactoryFixDecimal detail = new FactoryFixDecimal(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
