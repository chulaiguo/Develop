using JetCode.FactoryFixture;
using JetCode.FactoryData;
using NUnit.Framework;

namespace JetCode.FactoryDataFixture
{
    [TestFixture]
    public class FactoryFullNameFixture : FixtureBase
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
            get { return string.Format(@"{0}\Data_FullName", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryFullName detail = new FactoryFullName(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\FullName.cs", this.SrcDirectory);

            FactoryFullName detail = new FactoryFullName(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
