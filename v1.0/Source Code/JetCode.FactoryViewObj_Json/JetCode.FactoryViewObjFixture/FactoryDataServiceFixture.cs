using JetCode.FactoryFixture;
using JetCode.FactoryViewObj;
using NUnit.Framework;

namespace JetCode.FactoryViewObjFixture
{
    [TestFixture]
    public class FactoryDataServiceFixture : FixtureBase
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
            get { return string.Format(@"{0}\ViewObj_JSON.DataMethods", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryDataService detail = new FactoryDataService(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\DataMethods.cs", this.SrcDirectory);
            FactoryDataService detail = new FactoryDataService(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
