using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryData;
using NUnit.Framework;

namespace JetCode.FactoryDataFixture
{
    [TestFixture]
    public class FactoryDataViewFixture : FixtureBase
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
            get { return string.Format(@"{0}\Data_DataView", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                FactoryDataView detail = new FactoryDataView(base.Schema, item);
                base.WriteToScreen(detail);
            }
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            foreach (ObjectSchema item in base.Schema.Objects)
            {
                string viewFileName = string.Format(@"{0}\{1}View.cs", this.SrcDirectory, item.Alias);

                FactoryDataView view = new FactoryDataView(base.Schema, item);
                base.WriteToFile(viewFileName, view);
            }
        }
    }
}
