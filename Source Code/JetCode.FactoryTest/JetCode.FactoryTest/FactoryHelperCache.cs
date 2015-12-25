using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;
using System;

namespace JetCode.FactoryTest
{
    public class FactoryHelperCache : FactoryBase
    {
        public FactoryHelperCache(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using System.IO;");
            writer.WriteLine("using System.Reflection;");
            writer.WriteLine("using System.Runtime.Serialization.Formatters.Binary;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using E3000.Data;");
            writer.WriteLine("using E3000.BasicServiceWrapper;");

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.FacadeService.Utils", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\tpublic class HelperCache");
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tprivate SecurityToken _token = null;");
            writer.WriteLine(); 

            writer.WriteLine("\t\tpublic HelperCache(SecurityToken token)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._token = token;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprivate static void WriteError(string error)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tServiceBase.WriteError(error);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprivate Result RecordEntityToCache(object data, Guid recordPK, string action)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn RecordEntityToCache(data, recordPK, action, this._token);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tpublic static Result RecordEntityToCache(object data, Guid recordPK, string action, SecurityToken token)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\ttry");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tstring tableName = data.GetType().Name;");
            writer.WriteLine("\t\t\t\ttableName = tableName.Substring(0, tableName.Length - 4);");
            writer.WriteLine("\t\t\t\tif (string.Compare(action, CacheOfflineData._DeleteAction, StringComparison.OrdinalIgnoreCase) == 0)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tCacheOfflineData offline = new CacheOfflineData();");
            writer.WriteLine("\t\t\t\t\toffline.DateTime = DateTime.Now;");
            writer.WriteLine("\t\t\t\t\toffline.DataType = tableName;");
            writer.WriteLine("\t\t\t\t\toffline.RecordPK = recordPK;");
            writer.WriteLine("\t\t\t\t\toffline.Action = action;");
            writer.WriteLine("\t\t\t\t\tResult result = CacheOfflineWrapper.Save(offline, token);");
            writer.WriteLine("\t\t\t\t\tif (!result.OK)");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\treturn result;");
            writer.WriteLine("\t\t\t\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\t\treturn new Result(true);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\tHashtable table = new Hashtable();");
            writer.WriteLine("\t\t\t\tPropertyInfo[] properties = data.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);");
            writer.WriteLine("\t\t\t\tforeach (PropertyInfo info in properties)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tif (!info.CanRead || !info.CanWrite)");
            writer.WriteLine("\t\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\tif (info.Name == \"ObjectID\" || info.Name == \"RowVersion\")");
            writer.WriteLine("\t\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\tif (info.PropertyType.IsValueType || info.PropertyType == typeof(string) || info.PropertyType == typeof(byte[]))");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\tif (table.ContainsKey(info.Name))");
            writer.WriteLine("\t\t\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\t\tobject obj = info.GetValue(data, null);");
            writer.WriteLine("\t\t\t\t\t\tif (obj == null)");
            writer.WriteLine("\t\t\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\t\ttable.Add(info.Name, obj);");
            writer.WriteLine("\t\t\t\t\t}");           
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\tusing (MemoryStream fs = new MemoryStream())");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tBinaryFormatter formatter = new BinaryFormatter();");
            writer.WriteLine("\t\t\t\t\tformatter.Serialize(fs, table);");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\t\tCacheOfflineData offline = new CacheOfflineData();");
            writer.WriteLine("\t\t\t\t\toffline.DateTime = DateTime.Now;");
            writer.WriteLine("\t\t\t\t\toffline.ObjectStream = fs.ToArray();");
            writer.WriteLine("\t\t\t\t\toffline.DataType = tableName;");
            writer.WriteLine("\t\t\t\t\toffline.RecordPK = recordPK;");
            writer.WriteLine("\t\t\t\t\toffline.Action = action;");
            writer.WriteLine("\t\t\t\t\tResult result = CacheOfflineWrapper.Save(offline, token);");
            writer.WriteLine("\t\t\t\t\tif (!result.OK)");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\treturn result;");
            writer.WriteLine("\t\t\t\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\t\treturn new Result(true);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tcatch (Exception ex)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tWriteError(ex.Message);");
            writer.WriteLine("\t\t\t\treturn new Result(ex.Message);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tpublic static void CopyDataTo(object src, object dst)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tSortedList<string, PropertyInfo> index = new SortedList<string, PropertyInfo>();");
            writer.WriteLine("\t\t\tPropertyInfo[] properties = src.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);");
            writer.WriteLine("\t\t\tforeach (PropertyInfo info in properties)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (!info.CanRead || !info.CanWrite)");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\tif (info.Name == \"ObjectID\" || info.Name == \"RowVersion\")");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\tif (index.ContainsKey(info.Name))");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\tif (info.PropertyType.IsValueType || info.PropertyType == typeof(string) || info.PropertyType == typeof(byte[]))");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tindex.Add(info.Name, info);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\t\tproperties = dst.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);");
            writer.WriteLine("\t\t\tforeach (PropertyInfo dstInfo in properties)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (!dstInfo.CanRead || !dstInfo.CanWrite)");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\tif (!index.ContainsKey(dstInfo.Name))");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\tPropertyInfo srcInfo = index[dstInfo.Name];");
            writer.WriteLine("\t\t\t\tif (srcInfo.PropertyType != dstInfo.PropertyType)");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\tobject value = srcInfo.GetValue(src, null);");
            writer.WriteLine("\t\t\t\tif (value == null)");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\tdstInfo.SetValue(dst, value, null);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            SortedList<string, ObjectSchema> index = new SortedList<string, ObjectSchema>();
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (item.Alias.StartsWith("Log"))
                    continue;

                if (item.Alias == "CacheOffline" || item.Alias == "ACDownloadTask"
                    || item.Alias == "BDSiteCommand" || item.Alias == "BDSiteResponse" 
                    || item.Alias == "BDPanelCommand")
                    continue;

                if(index.ContainsKey(item.Alias))
                    continue;

                index.Add(item.Alias, item);
            }

            foreach (KeyValuePair<string, ObjectSchema> pair in index)
            {
                writer.WriteLine("\t\t#region {0}", pair.Key);
                writer.WriteLine("\t\tpublic Result Save{0}({0}Data data, bool sync)", pair.Key);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\ttry");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tif (!data.IsSelfDirty)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\treturn new Result(true);");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("");
                writer.WriteLine("\t\t\t\t//local");
                writer.WriteLine("\t\t\t\tResult result = {0}Wrapper.Save(data, this._token);", pair.Key);
                writer.WriteLine("\t\t\t\tif (!result.OK)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tWriteError(result.ToString());");
                writer.WriteLine("\t\t\t\t\treturn result;");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("");
                writer.WriteLine("\t\t\t\tif(data.IsNew)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tthis.InsertRemote{0}(data, sync);", pair.Key);
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t\telse if(data.IsDeleted)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tthis.DeleteRemote{0}(data, sync);", pair.Key);
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t\telse");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tthis.UpdateRemote{0}(data, sync);", pair.Key);
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("");
                writer.WriteLine("\t\t\t\treturn result;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\tcatch (Exception ex)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tWriteError(ex.Message);");
                writer.WriteLine("\t\t\t\treturn new Result(ex.Message);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tprivate void Save{0}ToCache({0}Data data, string action)", pair.Key);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tResult result = this.RecordEntityToCache(data, data.{0}PK, action);", pair.Key);
                writer.WriteLine("\t\t\tif (!result.OK)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tWriteError(result.ToString());");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tprivate void InsertRemote{0}({0}Data data, bool sync)", pair.Key);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tstring action = CacheOfflineData._InsertAction;");
                writer.WriteLine("\t\t\tif (!sync)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis.Save{0}ToCache(data, action);", pair.Key);
                writer.WriteLine("\t\t\t\treturn;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\ttry");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tD3000.Data.{0}Data remote = new D3000.Data.{0}Data();", pair.Key);
                writer.WriteLine("\t\t\t\tCopyDataTo(data, remote);");
                writer.WriteLine("\t\t\t\tremote.{0}PK = data.{0}PK;", pair.Key);
                writer.WriteLine("\t\t\t\tremote.CreatedBy = string.Empty;");
                writer.WriteLine("\t\t\t\tremote.CreatedOn = System.DateTime.Now;");
                writer.WriteLine("\t\t\t\tremote.ModifiedBy = string.Empty;");
                writer.WriteLine("\t\t\t\tremote.ModifiedOn = System.DateTime.Now;");
                writer.WriteLine("\t\t\t\tResult r = D3000.BasicServiceWrapper.{0}Wrapper.Save(remote, this._token);", pair.Key);
                writer.WriteLine("\t\t\t\tif (!r.OK)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tWriteError(r.ToString());");
                writer.WriteLine("\t\t\t\t\tthis.Save{0}ToCache(data, action);", pair.Key);
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\tcatch (Exception ex)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tWriteError(ex.Message);");
                writer.WriteLine("\t\t\t\tthis.Save{0}ToCache(data, action);", pair.Key);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine("");
                writer.WriteLine("\t\tprivate void UpdateRemote{0}({0}Data data, bool sync)", pair.Key);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tstring action = CacheOfflineData._UpdateAction;");
                writer.WriteLine("\t\t\tif (!sync)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis.Save{0}ToCache(data, action);", pair.Key);
                writer.WriteLine("\t\t\t\treturn;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\ttry");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tD3000.Data.{0}Data remote = D3000.BasicServiceWrapper.{0}Wrapper.GetByPK(data.{0}PK, this._token);", pair.Key);
                writer.WriteLine("\t\t\t\tif (remote == null)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tremote = new D3000.Data.{0}Data();", pair.Key);
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("");
                writer.WriteLine("\t\t\t\tCopyDataTo(data, remote);");
                writer.WriteLine("\t\t\t\tremote.{0}PK = data.{0}PK;", pair.Key);
                if (pair.Key == "ACHistory")
                {
                    writer.WriteLine("\t\t\t\tremote.Address = data.Address1;");
                }
                writer.WriteLine("\t\t\t\tif (remote.IsNew)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tremote.CreatedBy = string.Empty;");
                writer.WriteLine("\t\t\t\t\tremote.CreatedOn = System.DateTime.Now;");
                writer.WriteLine("\t\t\t\t\tremote.ModifiedBy = string.Empty;");
                writer.WriteLine("\t\t\t\t\tremote.ModifiedOn = System.DateTime.Now;");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t\telse");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tremote.ModifiedBy = string.Empty;");
                writer.WriteLine("\t\t\t\t\tremote.ModifiedOn = System.DateTime.Now;");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t\tResult r = D3000.BasicServiceWrapper.{0}Wrapper.Save(remote, this._token);", pair.Key);
                writer.WriteLine("\t\t\t\tif (!r.OK)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tWriteError(r.ToString());");
                writer.WriteLine("\t\t\t\t\tthis.Save{0}ToCache(data, action);", pair.Key);
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\tcatch (Exception ex)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tWriteError(ex.Message);");
                writer.WriteLine("\t\t\t\tthis.Save{0}ToCache(data, action);", pair.Key);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine("");
                writer.WriteLine("\t\tprivate void DeleteRemote{0}({0}Data data, bool sync)", pair.Key);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tstring action = CacheOfflineData._DeleteAction;");
                writer.WriteLine("\t\t\tif (!sync)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis.Save{0}ToCache(data, action);", pair.Key);
                writer.WriteLine("\t\t\t\treturn;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\ttry");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tD3000.Data.{0}Data remote = D3000.BasicServiceWrapper.{0}Wrapper.GetByPK(data.{0}PK, this._token);", pair.Key);
                writer.WriteLine("\t\t\t\tif (remote == null)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\treturn;");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("");
                writer.WriteLine("\t\t\t\tremote.Delete();");
                writer.WriteLine("\t\t\t\tResult r = D3000.BasicServiceWrapper.{0}Wrapper.Save(remote, this._token);", pair.Key);
                writer.WriteLine("\t\t\t\tif (!r.OK)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tWriteError(r.ToString());");
                writer.WriteLine("\t\t\t\t\tthis.Save{0}ToCache(data, action);", pair.Key);
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\tcatch (Exception ex)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tWriteError(ex.Message);");
                writer.WriteLine("\t\t\t\tthis.Save{0}ToCache(data, action);", pair.Key);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine("\t\t#endregion");
                writer.WriteLine();
            }
            
            writer.WriteLine("\t}");
            writer.WriteLine();
        }
    }
}
