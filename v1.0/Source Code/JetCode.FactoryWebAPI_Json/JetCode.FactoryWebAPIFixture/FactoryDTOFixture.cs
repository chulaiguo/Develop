using JetCode.FactoryFixture;
using JetCode.FactoryWebAPI;
using NUnit.Framework;

namespace JetCode.FactoryWebAPIFixture
{
    [TestFixture]
    public class FactoryDTOFixture : FixtureBase
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
            get { return string.Format(@"{0}\Publish_WebAPI_JSON.DTO", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDTO detail = new FactoryDTO(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\JsonDTO.cs", this.SrcDirectory);
            FactoryDTO detail = new FactoryDTO(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
