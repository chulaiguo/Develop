using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace JetCode.BusinessEntity
{
    [Serializable]
    public abstract class BusinessBase : ICloneable
    {
        private Guid _objectID = Guid.NewGuid();

        private bool _isNew = true;
        private bool _isDeleted = false;
        private bool _isDirty = true;

        [NonSerialized]
        private string _brokenRules = string.Empty;

        public Guid ObjectID
        {
            get { return this._objectID; }
            set { this._objectID = value; }
        }

        public bool IsNew
        {
            get { return this._isNew; }
        }

        public bool IsDeleted
        {
            get { return this._isDeleted; }
        }

        public bool IsSelfDirty
        {
            get { return this._isDirty; }
        }

        protected void MarkNew()
        {
            this._isNew = true;
            this._isDeleted = false;
            this.MarkDirty();
        }
        
        protected void MarkDeleted()
        {
            this._isDeleted = true;
            this.MarkDirty();
        }

        public virtual void MarkDirty()
        {
            this._isDirty = true;
        }

        public virtual void MarkClean()
        {
            this._isNew = false;
            this._isDeleted = false;
            this._isDirty = false;
        }

        public void Delete()
        {
            this.MarkDeleted();
        }

        public void AcceptSelfChanges()
        {
            this.MarkClean();
        }

        public void AcceptChanges()
        {
            this.AcceptSelfChanges();

            List<PropertyInfo> childList = this.GetChildrenInfoList();
            foreach (PropertyInfo info in childList)
            {
                BusinessCollectionBase collectionValue = info.GetValue(this, null) as BusinessCollectionBase;
                if (collectionValue == null)
                    continue;

                collectionValue.AcceptChanges();
            }
        }


        public bool IsDirty
        {
            get
            {
                if(this._isDirty)
                    return true;

                List<PropertyInfo> childList = this.GetChildrenInfoList();
                foreach (PropertyInfo info in childList)
                {
                    BusinessCollectionBase collectionValue = info.GetValue(this, null) as BusinessCollectionBase;
                    if (collectionValue == null)
                        continue;

                    if(collectionValue.IsDirty)
                        return true;
                }

                return false;
            }
        }


        public bool IsValid
        {
            get
            {
                this._brokenRules = string.Empty;
                if(this.IsDeleted)
                    return true;

                if (this.IsSelfDirty)
                {
                    this._brokenRules = this.CheckRules();
                    if(this._brokenRules.Length > 0)
                        return false;
                }

                List<PropertyInfo> childList = this.GetChildrenInfoList();
                foreach (PropertyInfo info in childList)
                {
                    BusinessCollectionBase collectionValue = info.GetValue(this, null) as BusinessCollectionBase;
                    if (collectionValue == null)
                        continue;

                    if (!collectionValue.IsValid)
                        return false;
                }

                return true;
            }
        }

        protected virtual string CheckRules()
        {
            return string.Empty;
        }

        public string GetBrokenRules()
        {
            StringBuilder builder = new StringBuilder();
            if(this._brokenRules.Length > 0)
            {
                builder.AppendLine(string.Format("{0}:", this.TableName));
                builder.AppendLine(this._brokenRules);
            }

            List<PropertyInfo> childList = this.GetChildrenInfoList();
            foreach (PropertyInfo info in childList)
            {
                BusinessCollectionBase collectionValue = info.GetValue(this, null) as BusinessCollectionBase;
                if (collectionValue == null)
                    continue;

                string error = collectionValue.GetBrokenRules();
                if(error.Length == 0)
                    continue;

                builder.AppendLine(error);
            }

            return builder.ToString();
        }


        public virtual bool IsActive
        {
            get { return true; }
        }

        public virtual bool IsLogicDeleted
        {
            get { return false; }
        }

        public virtual void ToLogicDeleted()
        {
        }

        public virtual byte[] RowVersion
        {
            get { return null; }
            set { }
        }

        public virtual string CreatedBy
        {
            get { return string.Empty; }
            set {}
        }

        public virtual DateTime CreatedOn
        {
            get { return DateTime.Now; }
            set { }
        }

        public virtual string ModifiedBy
        {
            get { return string.Empty; }
            set { }
        }

        public virtual DateTime ModifiedOn
        {
            get { return DateTime.Now; }
            set { }
        }

        public virtual string TableName
        {
            get { return string.Empty; }
        }

        public object Clone()
        {
            BusinessBase entity = base.MemberwiseClone() as BusinessBase;
            if(entity == null)
                return null;

            entity.CloneChildren(this);
            return entity;
        }

        private void CloneChildren(BusinessBase srcEntity)
        {
            List<PropertyInfo> childList = this.GetChildrenInfoList();
            foreach (PropertyInfo info in childList)
            {
                BusinessCollectionBase collectionValue = info.GetValue(srcEntity, null) as BusinessCollectionBase;
                if (collectionValue == null)
                    continue;

                info.SetValue(this, collectionValue.Clone(), null);
            }
        }

        public void GetChanges()
        {
            if (!this.IsDirty)
                return;

            List<PropertyInfo> childList = this.GetChildrenInfoList();
            foreach (PropertyInfo info in childList)
            {
                BusinessCollectionBase collectionValue = info.GetValue(this, null) as BusinessCollectionBase;
                if (collectionValue == null)
                    continue;

                if (this.IsDeleted)
                {
                    info.SetValue(this, null, null);
                }
                else
                {
                    collectionValue.GetChanges();
                }
            }
        }

        private List<PropertyInfo> GetChildrenInfoList()
        {
            List<PropertyInfo> retList = new List<PropertyInfo>();

            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public
                                                                     | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo info in properties)
            {
                if (!info.PropertyType.IsSubclassOf(typeof(BusinessCollectionBase)))
                    continue;

                retList.Add(info);
            }

            return retList;
        }

        public virtual void CopyFrom(BusinessBase entity, bool all)
        {
        }

        public void CopyParent(BusinessBase parent)
        {
            SortedList<string, PropertyInfo> parentIndex = this.GetParentInfoIndex(parent);

            PropertyInfo[] properties = this.GetType().GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo info in properties)
            {
                if(info.PropertyType == parent.GetType())
                {
                    info.SetValue(this, parent, null);
                    continue;
                }

                if(!parentIndex.ContainsKey(info.Name))
                    continue;

                PropertyInfo parentInfo = parentIndex[info.Name];
                if(parentInfo.PropertyType != info.PropertyType)
                    continue;

                object parentValue = parentInfo.GetValue(parent, null);
                if(parentValue == null)
                    continue;

                info.SetValue(this, parentValue, null);
            }
        }

        private SortedList<string, PropertyInfo> GetParentInfoIndex(object parent)
        {
            SortedList<string, PropertyInfo> retIndex = new SortedList<string, PropertyInfo>();

            PropertyInfo[] properties = parent.GetType().GetProperties(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo info in properties)
            {
                if (!info.PropertyType.IsSubclassOf(typeof(BusinessBase)))
                    continue;

                if(retIndex.ContainsKey(info.Name))
                    continue;

                retIndex.Add(info.Name, info);
            }

            return retIndex;
        }

        public byte[] Serialize()
        {
            Hashtable hashtable = new Hashtable();
            SortedList<string, FieldInfo> fieldInfoList = this.GetFieldInfoList();
            foreach (KeyValuePair<string, FieldInfo> pair in fieldInfoList)
            {
                object obj = pair.Value.GetValue(this);
                if (obj != null)
                {
                    hashtable.Add(pair.Key, obj);
                }
            }

            MemoryStream stream = new MemoryStream();
            new BinaryFormatter().Serialize(stream, hashtable);
            return stream.ToArray();
        }


        public void Deserialize(byte[] data)
        {
            if (data == null || data.Length == 0)
                return;

            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();
            Hashtable hashtable = formatter.Deserialize(stream) as Hashtable;
            if (hashtable != null)
            {
                SortedList<string, FieldInfo> fieldInfoList = this.GetFieldInfoList();
                foreach (DictionaryEntry entry in hashtable)
                {
                    if (fieldInfoList.ContainsKey(entry.Key.ToString()))
                    {
                        fieldInfoList[entry.Key.ToString()].SetValue(this, entry.Value);
                    }
                }
            }
        }

        private SortedList<string, FieldInfo> GetFieldInfoList()
        {
            SortedList<string, FieldInfo> list = new SortedList<string, FieldInfo>();
            this.GetFieldInfoList(list, this.GetType());
            return list;
        }

        private void GetFieldInfoList(SortedList<string, FieldInfo> list, Type type)
        {
            if (type == typeof(BusinessBase))
                return;

            FieldInfo[] fields = type.GetFields(BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (FieldInfo info in fields)
            {
                if (list.ContainsKey(info.Name))
                    continue;

                if (info.FieldType.IsValueType || info.FieldType == typeof(string) || info.FieldType == typeof(byte[]))
                {
                    list.Add(info.Name, info);
                }
            }

            this.GetFieldInfoList(list, type.BaseType);
        }
    }
}