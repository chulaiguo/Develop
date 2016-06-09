using JetCode.FactoryFixture;
using JetCode.FactoryTest;
using NUnit.Framework;

namespace JetCode.FactoryTestFixture
{
    [TestFixture]
    public class FactorySettingCategoryFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.SettingCategory", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactorySettingCategory detail = new FactorySettingCategory(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\SettingCategory.cs", this.SrcDirectory);
            FactorySettingCategory detail = new FactorySettingCategory(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
