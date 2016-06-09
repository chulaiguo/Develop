using JetCode.FactoryFixture;
using JetCode.FactoryWebAPI;
using NUnit.Framework;

namespace JetCode.FactoryWebAPIFixture
{
    [TestFixture]
    public class FactoryDTOConvertFixture : FixtureBase
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
            get { return string.Format(@"{0}\Publish_WebAPI_JSON.DTOConvert", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDTOConvert detail = new FactoryDTOConvert(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\JsonDTOConvert.cs", this.SrcDirectory);
            FactoryDTOConvert detail = new FactoryDTOConvert(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
