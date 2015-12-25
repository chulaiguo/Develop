using JetCode.BizSchema;
using JetCode.Factory;
using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryWorkMapListFixture : FixtureBase
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
            get { return string.Format(@"{0}\WinUI.FormWorkMapList", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                if (!FactoryBase.IsMapTable(item))
                    continue;

                FactoryWorkMapList detail = new FactoryWorkMapList(base.Schema, item);
                base.WriteToScreen(detail);

                FactoryWorkMapListDesign design = new FactoryWorkMapListDesign(base.Schema, item);
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

                string detailFileName = string.Format(@"{0}\FormWork{1}List.cs", this.SrcDirectory, item.Alias);
                string designFileName = string.Format(@"{0}\FormWork{1}List.Designer.cs", this.SrcDirectory, item.Alias);

                FactoryWorkMapList detail = new FactoryWorkMapList(base.Schema, item);
                base.WriteToFile(detailFileName, detail);

                FactoryWorkMapListDesign design = new FactoryWorkMapListDesign(base.Schema, item);
                base.WriteToFile(designFileName, design);
            }
        }
    }
}
