using JetCode.FactoryData;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryDataFixture
{
    [TestFixture]
    public class FactoryUpperCaseFixture : FixtureBase
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
                return string.Format(@"{0}\Data_Uppercase", BasePath);
            }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryUpperCase detail = new FactoryUpperCase(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\Uppercase.cs", this.SrcDirectory);

            FactoryUpperCase detail = new FactoryUpperCase(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
