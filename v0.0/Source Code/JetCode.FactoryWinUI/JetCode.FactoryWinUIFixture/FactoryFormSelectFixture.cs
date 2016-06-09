using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryFormSelectFixture : FixtureBase
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
            get { return string.Format(@"{0}\WinUI.FormSelect", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                if (item.GetPKList().Count == 2)
                    continue;

                FactoryFormSelect detail = new FactoryFormSelect(base.Schema, item);
                base.WriteToScreen(detail);

                FactoryFormSelectDesign design = new FactoryFormSelectDesign(base.Schema, item);
                base.WriteToScreen(design);
            }
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            foreach (ObjectSchema item in base.Schema.Objects)
            {
                if (item.GetPKList().Count == 2)
                    continue;

                string detailFileName = string.Format(@"{0}\FormSelect{1}.cs", this.SrcDirectory, item.Alias);
                string designFileName = string.Format(@"{0}\FormSelect{1}.Designer.cs", this.SrcDirectory, item.Alias);

                FactoryFormSelect detail = new FactoryFormSelect(base.Schema, item);
                base.WriteToFile(detailFileName,detail);

                FactoryFormSelectDesign design = new FactoryFormSelectDesign(base.Schema, item);
                base.WriteToFile(designFileName, design);
            }
        }
    }
}
