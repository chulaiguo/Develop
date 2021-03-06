using System;
using System.IO;
using System.Text;

namespace JetCode.Factory
{
    public static class AssemblyInfoCode
    {
        public static string AssemblyTitle = string.Empty;
        public static string AssemblyDescription = string.Empty;
        public static string AssemblyConfiguration = string.Empty;
        public static string AssemblyCompany = string.Empty;
        public static string AssemblyProduct = string.Empty;
        public static string AssemblyCopyright = "Copyright © 2007";
        public static string AssemblyTrademark = string.Empty;
        public static string AssemblyCulture = string.Empty;

        public static string AssemblyVersion = "1.0.0.0";
        public static string AssemblyFileVersion = "1.0.0.0";

        public static bool ComVisible = false;
        public static string ProjectID = Guid.NewGuid().ToString();

        public static string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            WriteUsing(writer);
            WriteContent(writer);

            return writer.ToString();
        }

        private static void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System.Reflection;");
            writer.WriteLine("using System.Runtime.CompilerServices;");
            writer.WriteLine("using System.Runtime.InteropServices;");

            writer.WriteLine();
        }

        private static void WriteContent(StringWriter writer)
        {
            writer.WriteLine("// General Information about an assembly is controlled through the following");
            writer.WriteLine("// set of attributes. Change these attribute values to modify the information");
            writer.WriteLine("// associated with an assembly.");
            writer.WriteLine("[assembly: AssemblyTitle(\"{0}\")]", AssemblyTitle);
            writer.WriteLine("[assembly: AssemblyDescription(\"{0}\")]", AssemblyDescription);
            writer.WriteLine("[assembly: AssemblyConfiguration(\"{0}\")]", AssemblyConfiguration);
            writer.WriteLine("[assembly: AssemblyCompany(\"{0}\")]", AssemblyCompany);
            writer.WriteLine("[assembly: AssemblyProduct(\"{0}\")]", AssemblyProduct);
            writer.WriteLine("[assembly: AssemblyCopyright(\"{0}\")]", AssemblyCopyright);
            writer.WriteLine("[assembly: AssemblyTrademark(\"{0}\")]", AssemblyTrademark);
            writer.WriteLine("[assembly: AssemblyCulture(\"{0}\")]", AssemblyCulture);
            writer.WriteLine();

            writer.WriteLine("// Setting ComVisible to false makes the types in this assembly not visible");
            writer.WriteLine("// to COM components.  If you need to access a type in this assembly from ");
            writer.WriteLine("// COM, set the ComVisible attribute to true on that type.");
            if (ComVisible)
            {
                writer.WriteLine("[assembly: ComVisible(true)]");
            }
            else
            {
                writer.WriteLine("[assembly: ComVisible(false)]");
            }
            writer.WriteLine();

            writer.WriteLine("// The following GUID is for the ID of the typelib if this project is exposed to COM");
            writer.WriteLine("[assembly: Guid(\"{0}\")]", ProjectID);
            writer.WriteLine();

            writer.WriteLine("// Version information for an assembly consists of the following four values:");
            writer.WriteLine("//");
            writer.WriteLine("//      Major Version");
            writer.WriteLine("//      Minor Version");
            writer.WriteLine("//      Build Number");
            writer.WriteLine("//      Revisionv");
            writer.WriteLine("//");
            writer.WriteLine("// You can specify all the values or you can default the Revision and Build Numbers");
            writer.WriteLine("// by using the '*' as shown below:");
            writer.WriteLine("[assembly: AssemblyVersion(\"{0}\")]", AssemblyVersion);
            writer.WriteLine("[assembly: AssemblyFileVersion(\"{0}\")]", AssemblyFileVersion);
        }
    }
}