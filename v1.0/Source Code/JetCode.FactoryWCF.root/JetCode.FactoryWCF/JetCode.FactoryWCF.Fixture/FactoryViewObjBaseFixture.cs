using JetCode.BizSchema;
using JetCode.FactoryFixture;
using NUnit.Framework;

namespace JetCode.FactoryWCF.Fixture
{
    [TestFixture]
    public class FactoryViewObjBaseFixture : FixtureBase
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
            get { return string.Format(@"{0}\Sliverlight.ViewObjBase", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryViewObjBase detail = new FactoryViewObjBase(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\ViewObjBase.cs", this.SrcDirectory);

            FactoryViewObjBase entity = new FactoryViewObjBase(base.Schema);
            base.WriteToFile(fileName, entity);
        }
    }
}