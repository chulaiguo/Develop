using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryClientBasicObjMethodsFixture : FixtureBase
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
            get { return string.Format(@"{0}\Sliverlight.BasicObjMethods", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryClientBasicObjMethods detail = new FactoryClientBasicObjMethods(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\ViewObjMethods.cs", this.SrcDirectory);

            FactoryClientBasicObjMethods entity = new FactoryClientBasicObjMethods(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}