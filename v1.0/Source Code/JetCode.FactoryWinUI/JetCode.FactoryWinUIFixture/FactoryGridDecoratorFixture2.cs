using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryGridDecoratorFixture2 : FixtureBase
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
            get { return string.Format(@"{0}\WinUI.GridDecorator", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                FactoryGridDecorator2 factory = new FactoryGridDecorator2(base.Schema, item);
                base.WriteToScreen(factory);
            }
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            foreach (ObjectSchema item in base.Schema.Objects)
            {
                string fileName = string.Format(@"{0}\Grid{1}Decorator.cs", this.SrcDirectory, item.Alias);

                FactoryGridDecorator2 factory = new FactoryGridDecorator2(base.Schema, item);
                base.WriteToFile(fileName, factory);
            }
        }
    }
}
