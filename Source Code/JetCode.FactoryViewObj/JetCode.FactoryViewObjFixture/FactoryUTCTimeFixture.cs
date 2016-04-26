using JetCode.FactoryFixture;
using JetCode.FactoryViewObj;
using NUnit.Framework;

namespace JetCode.FactoryViewObjFixture
{
    [TestFixture]
    public class FactoryUTCTimeFixture : FixtureBase
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
            get { return string.Format(@"{0}\ViewObj.UTCTime", BasePath); }
        }

        [Test]
        public void WriteCodeToScreen()
        {
            FactoryUTCTime detail = new FactoryUTCTime(base.Schema);
            base.WriteToScreen(detail);
        }

        [Test]
        public void WriteCodeToFile()
        {
            base.ClearSrcDirectory();

            string fileName = string.Format(@"{0}\UTCTime.cs", this.SrcDirectory);
            FactoryUTCTime detail = new FactoryUTCTime(base.Schema);
            base.WriteToFile(fileName, detail);
        }
    }
}
