using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryWorkEditorFixture : FixtureBase
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
            get { return string.Format(@"{0}\WinUI.FormWorkEditor", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                FactoryWorkEditor detail = new FactoryWorkEditor(base.Schema, item);
                base.WriteToScreen(detail);

                FactoryWorkEditorDesign design = new FactoryWorkEditorDesign(base.Schema, item);
                base.WriteToScreen(design);
            }
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            foreach (ObjectSchema item in base.Schema.Objects)
            {
                string detailFileName = string.Format(@"{0}\FormWork{1}.cs", this.SrcDirectory, item.Alias);
                string designFileName = string.Format(@"{0}\FormWork{1}.Designer.cs", this.SrcDirectory, item.Alias);

                FactoryWorkEditor detail = new FactoryWorkEditor(base.Schema, item);
                base.WriteToFile(detailFileName, detail);

                FactoryWorkEditorDesign design = new FactoryWorkEditorDesign(base.Schema, item);
                base.WriteToFile(designFileName, design);
            }
        }
    }
}
