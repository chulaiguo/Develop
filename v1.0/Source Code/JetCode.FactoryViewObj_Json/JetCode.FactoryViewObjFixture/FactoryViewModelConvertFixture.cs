using JetCode.FactoryFixture;
using JetCode.FactoryViewObj;
using NUnit.Framework;

namespace JetCode.FactoryViewObjFixture
{
    [TestFixture]
    public class FactoryViewModelConvertFixture : FixtureBase
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
            get { return string.Format(@"{0}\ViewObj_JSON.DataObjConvert", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryViewModelConvert detail = new FactoryViewModelConvert(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\DataObjConvert.cs", this.SrcDirectory);

            FactoryViewModelConvert detail = new FactoryViewModelConvert(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
