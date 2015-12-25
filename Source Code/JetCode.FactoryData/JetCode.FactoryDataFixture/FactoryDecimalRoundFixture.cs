using JetCode.FactoryData;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryDataFixture
{
    [TestFixture]
    public class FactoryDecimalRoundFixture : FixtureBase
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
            get
            {
                return string.Format(@"{0}\Data_DecimalRound", BasePath);
            }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDecimalRound detail = new FactoryDecimalRound(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\DecimalRound.cs", this.SrcDirectory);

            FactoryDecimalRound detail = new FactoryDecimalRound(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
