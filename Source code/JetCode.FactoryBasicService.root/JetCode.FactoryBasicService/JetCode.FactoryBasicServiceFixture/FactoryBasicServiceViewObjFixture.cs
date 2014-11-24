using JetCode.FactoryBasicService;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryBasicServiceFixture
{
    [TestFixture]
    public class FactoryBasicServiceViewObjFixture : FixtureBase
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
            get { return string.Format(@"{0}\BasicService.ViewMethods", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryBasicServiceViewObj detail = new FactoryBasicServiceViewObj(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\ViewMethods.cs", this.SrcDirectory);
            FactoryBasicServiceViewObj detail = new FactoryBasicServiceViewObj(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
