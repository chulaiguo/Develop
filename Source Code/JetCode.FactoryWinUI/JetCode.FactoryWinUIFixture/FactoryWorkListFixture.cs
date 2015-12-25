using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryWorkListFixture : FixtureBase
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
            get { return string.Format(@"{0}\WinUI.FormWorkList", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                FactoryWorkList detail = new FactoryWorkList(base.Schema, item);
                base.WriteToScreen(detail);

                FactoryWorkListDesign design = new FactoryWorkListDesign(base.Schema, item);
                base.WriteToScreen(design);
            }
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            foreach (ObjectSchema item in base.Schema.Objects)
            {
                string detailFileName = string.Format(@"{0}\FormWork{1}List.cs", this.SrcDirectory, item.Alias);
                string designFileName = string.Format(@"{0}\FormWork{1}List.Designer.cs", this.SrcDirectory, item.Alias);

                FactoryWorkList detail = new FactoryWorkList(base.Schema, item);
                base.WriteToFile(detailFileName, detail);

                FactoryWorkListDesign design = new FactoryWorkListDesign(base.Schema, item);
                base.WriteToFile(designFileName, design);
            }
        }
    }
}
