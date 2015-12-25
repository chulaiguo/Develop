using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryFormMainFixture : FixtureBase
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
            get { return string.Format(@"{0}\WinUI.FormMainCode", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryFormMain detail = new FactoryFormMain(base.Schema);
            base.WriteToScreen(detail);

            FactoryFormMainDesign design = new FactoryFormMainDesign(base.Schema);
            base.WriteToScreen(design);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string detailFileName = string.Format(@"{0}\FormMain.cs", this.SrcDirectory);
            string designFileName = string.Format(@"{0}\FormMain.Designer.cs", this.SrcDirectory);

            FactoryFormMain detail = new FactoryFormMain(base.Schema);
            base.WriteToFile(detailFileName,detail);

            FactoryFormMainDesign design = new FactoryFormMainDesign(base.Schema);
            base.WriteToFile(designFileName, design);
        }
    }
}
