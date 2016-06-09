using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryDetailEditorFixture : FixtureBase
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
            get { return string.Format(@"{0}\WinUI.FormDetailEditor_Old", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                FactoryDetailEditor detail = new FactoryDetailEditor(base.Schema, item);
                base.WriteToScreen(detail);

                FactoryDetailEditorDesign design = new FactoryDetailEditorDesign(base.Schema, item);
                base.WriteToScreen(design);
            }
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            foreach (ObjectSchema item in base.Schema.Objects)
            {
                string detailFileName = string.Format(@"{0}\FormDetail{1}.cs", this.SrcDirectory, item.Alias);
                string designFileName = string.Format(@"{0}\FormDetail{1}.Designer.cs", this.SrcDirectory, item.Alias);

                FactoryDetailEditor detail = new FactoryDetailEditor(base.Schema, item);
                base.WriteToFile(detailFileName,detail);

                FactoryDetailEditorDesign design = new FactoryDetailEditorDesign(base.Schema, item);
                base.WriteToFile(designFileName, design);
            }
        }
    }
}
