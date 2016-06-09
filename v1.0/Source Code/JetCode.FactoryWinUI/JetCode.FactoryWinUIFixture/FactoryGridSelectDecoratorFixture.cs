using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryGridSelectDecoratorFixture : FixtureBase
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
            get { return string.Format(@"{0}\WinUI.GridSelect", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                if (item.GetPKList().Count == 2)
                    continue;

                FactoryGridSelectDecorator2 factory = new FactoryGridSelectDecorator2(base.Schema, item);
                base.WriteToScreen(factory);
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

                string fileName = string.Format(@"{0}\GridSelect{1}Decorator.cs", this.SrcDirectory, item.Alias);

                FactoryGridSelectDecorator2 factory = new FactoryGridSelectDecorator2(base.Schema, item);
                base.WriteToFile(fileName, factory);
            }
        }
    }
}
