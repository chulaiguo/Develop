using JetCode.FactoryData;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryDataFixture
{
    [TestFixture]
    public class FactoryLogicDeleteFixture : FixtureBase
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
                return string.Format(@"{0}\Data_LogicDeleted", BasePath);
            }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryLogicDelete detail = new FactoryLogicDelete(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\LogicDeleted.cs", this.SrcDirectory);

            FactoryLogicDelete detail = new FactoryLogicDelete(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
