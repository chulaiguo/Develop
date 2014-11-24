using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.IO;
using JetCode.BizSchema;
using JetCode.BizSchema.Factory;
using JetCode.Factory;
using Microsoft.CSharp;
using NUnit.Framework;

namespace JetCode.FactoryFixture
{
    public class FixtureBase
    {
        protected const string BasePath = @"F:\Tools\Factory";
        private MappingSchema _mappingSchema = null;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            string xmlFile = string.Format(@"{0}\Schema.xml", BasePath);
            this._mappingSchema = SchemaFactory.GetMappingSchema(xmlFile);

            FactoryBase.DatabaseType = this.GetDatabaseType();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
        }

        private DatabaseTypeCode GetDatabaseType()
        {
            string dbType = ConfigurationManager.AppSettings["DBType"];
            if (string.IsNullOrEmpty(dbType))
                return DatabaseTypeCode.Default;

            if (dbType == DatabaseTypeCode.Oracle.ToString())
                return DatabaseTypeCode.Oracle;

            if (dbType == DatabaseTypeCode.MSSqlServer.ToString())
                return DatabaseTypeCode.MSSqlServer;

            return DatabaseTypeCode.Default;
        }

        protected MappingSchema Schema
        {
            get { return _mappingSchema; }
        }

        protected virtual string SrcDirectory
        {
            get { return BasePath; }
        }

        protected string TestDocDirectory
        {
            get { return string.Format(@"{0}\TestDoc", BasePath); }
        }

        protected string OutputDirectory
        {
            get { return string.Format(@"{0}\Bin", BasePath); }
        }

        protected virtual string AssemblyInfo
        {
            get { return AssemblyInfoCode.GenCode(); }
        }

        protected void ClearSrcDirectory()
        {
            if (Directory.Exists(this.SrcDirectory))
            {
                Directory.Delete(this.SrcDirectory, true);
            }

            Directory.CreateDirectory(this.SrcDirectory);
        }

        protected void WriteToFile(string fileName, string srcCode)
        {
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fs.SetLength(0);
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(srcCode);
            }
        }

        protected void WriteToFile(string fileName, FactoryBase factory)
        {
            this.WriteToFile(fileName, factory.GenCode());
        }

        protected void WriteToScreen(FactoryBase factory)
        {
            Console.WriteLine(factory.GenCode());
        }

        protected void WriteToScreen(string srcCode)
        {
            Console.WriteLine(srcCode);
        }

        protected void Compile(string dllName)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters cp = new CompilerParameters();

            //Add Referenced Assemblies
            string[] files = Directory.GetFiles(this.OutputDirectory);
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Data.dll");
            cp.ReferencedAssemblies.Add("System.Configuration.dll");
            foreach (string item in files)
            {
                if (item.EndsWith(dllName))
                    continue;

                if (!item.ToLower().EndsWith("dll"))
                    continue;

                cp.ReferencedAssemblies.Add(item);
            }

            // Generate an a class library
            if (dllName.ToLower().EndsWith("exe"))
            {
                cp.GenerateExecutable = true;
            }
            else
            {
                cp.GenerateExecutable = false;
            }

            // Specify the assembly file name to generate.
            cp.OutputAssembly = string.Format(@"{0}\{1}", this.OutputDirectory, dllName);

            // Save the assembly as a physical file.
            cp.GenerateInMemory = false;

            // Set whether to treat all warnings as errors.
            cp.TreatWarningsAsErrors = false;

            //AssemblyInfo
            this.WriteToFile(string.Format(@"{0}\AssemblyInfo.cs", this.SrcDirectory), AssemblyInfo);

            string[] srcfiles = Directory.GetFiles(this.SrcDirectory, "*.cs");
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, srcfiles);
            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                Console.WriteLine("Errors building into {0}", cr.PathToAssembly);
                foreach (CompilerError ce in cr.Errors)
                {
                    Console.WriteLine("{0}", ce);
                    Console.WriteLine();
                }
            }
            Assert.IsFalse(cr.Errors.Count > 0);
        }
    }
}