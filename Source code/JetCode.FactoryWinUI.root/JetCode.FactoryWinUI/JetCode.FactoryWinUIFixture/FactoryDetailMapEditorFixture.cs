using JetCode.BizSchema;
using JetCode.Factory;
using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryDetailMapEditorFixture : FixtureBase
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
            get { return string.Format(@"{0}\WinUI.FormDetailMap", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                if (!FactoryBase.IsMapTable(item))
                    continue;

                FactoryDetailMapEditor detail = new FactoryDetailMapEditor(base.Schema, item);
                base.WriteToScreen(detail);

                FactoryDetailMapEditorDesign design = new FactoryDetailMapEditorDesign(base.Schema, item);
                base.WriteToScreen(design);
            }
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            foreach (ObjectSchema item in base.Schema.Objects)
            {
                if (!FactoryBase.IsMapTable(item))
                    continue;

                string detailFileName = string.Format(@"{0}\FormDetail{1}.cs", this.SrcDirectory, item.Alias);
                string designFileName = string.Format(@"{0}\FormDetail{1}.Designer.cs", this.SrcDirectory, item.Alias);

                FactoryDetailMapEditor detail = new FactoryDetailMapEditor(base.Schema, item);
                base.WriteToFile(detailFileName,detail);

                FactoryDetailMapEditorDesign design = new FactoryDetailMapEditorDesign(base.Schema, item);
                base.WriteToFile(designFileName, design);
            }
        }
    }
}
