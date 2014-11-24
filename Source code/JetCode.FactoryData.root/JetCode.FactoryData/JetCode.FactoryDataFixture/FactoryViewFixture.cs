using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryData;
using NUnit.Framework;

namespace JetCode.FactoryDataFixture
{
    [TestFixture]
    public class FactoryViewFixture : FixtureBase
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
            get { return string.Format(@"{0}\Data.ViewCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                FactoryView detail = new FactoryView(base.Schema, item);
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

                FactoryView view = new FactoryView(base.Schema, item);
                base.WriteToFile(viewFileName, view);
            }
        }
    }
}
