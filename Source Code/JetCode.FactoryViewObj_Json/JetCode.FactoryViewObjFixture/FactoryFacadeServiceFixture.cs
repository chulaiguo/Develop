using JetCode.FactoryFixture;
using JetCode.FactoryViewObj;
using NUnit.Framework;

namespace JetCode.FactoryViewObjFixture
{
    [TestFixture]
    public class FactoryFacadeServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\ViewObj_JSON.FacadeMethods", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryFacadeService detail = new FactoryFacadeService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\FacadeMethods.cs", this.SrcDirectory);
            FactoryFacadeService detail = new FactoryFacadeService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
