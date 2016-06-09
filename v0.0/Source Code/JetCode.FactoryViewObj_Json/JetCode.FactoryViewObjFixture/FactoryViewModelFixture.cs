using JetCode.FactoryFixture;
using JetCode.FactoryViewObj;
using NUnit.Framework;

namespace JetCode.FactoryViewObjFixture
{
    [TestFixture]
    public class FactoryViewModelFixture : FixtureBase
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
            get { return string.Format(@"{0}\ViewObj_JSON.DataObj", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryViewModel detail = new FactoryViewModel(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\DataObj.cs", this.SrcDirectory);

            FactoryViewModel detail = new FactoryViewModel(base.Schema);
            base.WriteToFile(detailFileName,detail);
        }
    }
}
