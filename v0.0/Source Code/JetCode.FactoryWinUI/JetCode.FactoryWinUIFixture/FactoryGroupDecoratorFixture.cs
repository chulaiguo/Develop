using JetCode.BizSchema;
using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryGroupDecoratorFixture : FixtureBase
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
            get { return string.Format(@"{0}\WinUI.GroupDecorator", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            foreach (ObjectSchema item in base.Schema.Objects)
            {
                if(item.Parents.Count != 2)
                    continue;

                if(!item.IsMultiPK)
                    continue;

                ObjectSchema parent1 = base.Schema.Objects.Find(item.Parents[0].Name);
                ObjectSchema parent2 = base.Schema.Objects.Find(item.Parents[1].Name);
                if(parent1 == null || parent2 == null)
                    continue;

                FactoryGroupDecorator factory1 = new FactoryGroupDecorator(base.Schema, item, parent1, parent2);
                base.WriteToScreen(factory1);

                FactoryGroupDecorator factory2 = new FactoryGroupDecorator(base.Schema, item, parent2, parent1);
                base.WriteToScreen(factory2);
            }
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            foreach (ObjectSchema item in base.Schema.Objects)
            {
                if (item.Parents.Count != 2)
                    continue;

                if (!item.IsMultiPK)
                    continue;

                ObjectSchema parent1 = base.Schema.Objects.Find(item.Parents[0].Name);
                ObjectSchema parent2 = base.Schema.Objects.Find(item.Parents[1].Name);
                if (parent1 == null || parent2 == null)
                    continue;

                string fileName1 = string.Format(@"{0}\Group{1}{2}Decorator.cs", this.SrcDirectory, parent1.Alias, parent2.Alias);
                FactoryGroupDecorator factory1 = new FactoryGroupDecorator(base.Schema, item, parent1, parent2);
                base.WriteToFile(fileName1, factory1);

                string fileName2 = string.Format(@"{0}\Group{1}{2}Decorator.cs", this.SrcDirectory, parent2.Alias, parent1.Alias);
                FactoryGroupDecorator factory2 = new FactoryGroupDecorator(base.Schema, item, parent2, parent1);
                base.WriteToFile(fileName2, factory2);
            }
        }
    }
}
