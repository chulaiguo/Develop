using JetCode.FactoryFixture;
using JetCode.FactoryWinUI;
using NUnit.Framework;

namespace JetCode.FactoryWinUIFixture
{
    [TestFixture]
    public class FactoryViewObjFunsFixture : FixtureBase
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
            get { return string.Format(@"{0}\Misc.ViewObj_SpecialFuns", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryViewObjFuns detail = new FactoryViewObjFuns(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\SpecialFuns.cs", this.SrcDirectory);
            FactoryViewObjFuns detail = new FactoryViewObjFuns(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
