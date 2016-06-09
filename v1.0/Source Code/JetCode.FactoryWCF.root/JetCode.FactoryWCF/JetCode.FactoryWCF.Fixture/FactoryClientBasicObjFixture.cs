using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryClientBasicObjFixture : FixtureBase
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
            get { return string.Format(@"{0}\Sliverlight.BasicObj", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryClientBasicObj detail = new FactoryClientBasicObj(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\ViewObj.cs", this.SrcDirectory);

            FactoryClientBasicObj entity = new FactoryClientBasicObj(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}