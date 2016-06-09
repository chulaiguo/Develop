using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryDataFixture
{
    [TestFixture]
    public class FactoryViewObjFixture : FixtureBase
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
            get { return string.Format(@"{0}\Data_ViewObj", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryData.FactoryViewObj detail = new FactoryData.FactoryViewObj(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\ViewObj.cs", this.SrcDirectory);

            FactoryData.FactoryViewObj detail = new FactoryData.FactoryViewObj(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
