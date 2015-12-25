using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryWorkSearchFixture : FixtureBase
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
            get { return string.Format(@"{0}\WinUI.FormWorkSearch", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                FactoryWorkSearch detail = new FactoryWorkSearch(base.Schema, item);
                base.WriteToScreen(detail);

                FactoryWorkSearchDesign design = new FactoryWorkSearchDesign(base.Schema, item);
                base.WriteToScreen(design);
            }
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            foreach (ObjectSchema item in base.Schema.Objects)
            {
                string detailFileName = string.Format(@"{0}\FormWork{1}Search.cs", this.SrcDirectory, item.Alias);
                string designFileName = string.Format(@"{0}\FormWork{1}Search.Designer.cs", this.SrcDirectory, item.Alias);

                FactoryWorkSearch detail = new FactoryWorkSearch(base.Schema, item);
                base.WriteToFile(detailFileName, detail);

                FactoryWorkSearchDesign design = new FactoryWorkSearchDesign(base.Schema, item);
                base.WriteToFile(designFileName, design);
            }
        }
    }
}
