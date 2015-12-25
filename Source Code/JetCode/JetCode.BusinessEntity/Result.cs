using System;
using System.Collections;
using System.Text;

namespace JetCode.BusinessEntity
{
    [Serializable]
    public class Result
    {
        private readonly Hashtable _errors = new Hashtable();
        private readonly Hashtable _rowversions = new Hashtable();
        private bool _ok = false;
        private object _tag = null;

        public Result()
        {
        }

        public Result(bool ok)
        {
            this._ok = ok;
        }

        public Result(Exception ex)
        {
            this._errors.Add(Guid.NewGuid(), ex);
        }

        public Result(string error)
        {
            this._errors.Add(Guid.NewGuid(), new ApplicationException(error));
        }

        public Result(BusinessBase entity, Exception ex)
        {
            Guid key = Guid.NewGuid();
            if (entity != null)
            {
                key = entity.ObjectID;
            }
            this._errors.Add(key, ex);
        }

        public Result(BusinessBase entity, string error)
        {
            Guid key = Guid.NewGuid();
            if (entity != null)
            {
                key = entity.ObjectID;
            }
            this._errors.Add(key, new ApplicationException(error));
        }

        public Result(Guid objID, byte[] rowVersion)
        {
            this._ok = true;
            this.Add(objID, rowVersion);
        }

        public void Add(Result r)
        {
            if (r == null)
                return;

            IDictionaryEnumerator enumerator = r.Errors.GetEnumerator();
            while (enumerator.MoveNext())
            {
                this.Errors.Add(enumerator.Key, enumerator.Value);
                this.OK = false;
            }

            enumerator = r.RowVersions.GetEnumerator();
            while (enumerator.MoveNext())
            {
                this.RowVersions.Add(enumerator.Key, enumerator.Value);
            }
        }

        public void Add(Guid objID, Exception ex)
        {
            this.OK = false;
            this.Errors.Add(objID, ex);
        }

        public void Add(Guid objID, string error)
        {
            this.OK = false;
            this.Errors.Add(objID, new ApplicationException(error));
        }

        public void Add(Guid objID, byte[] rowVersion)
        {
            this.RowVersions.Add(objID, rowVersion);
        }

        public byte[] GetRowVersion(Guid objID)
        {
            return (byte[]) this.RowVersions[objID];
        }

        public Hashtable Errors
        {
            get { return this._errors; }
        }

        public Hashtable RowVersions
        {
            get { return this._rowversions; }
        }

        public bool OK
        {
            get { return this._ok; }
            set { this._ok = value; }
        }

        public object Tag
        {
            get { return this._tag; }
            set { this._tag = value; }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            IDictionaryEnumerator enumerator = this.Errors.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Exception ex = (Exception)enumerator.Value;
                if(ex == null)
                    continue;

                builder.AppendLine(this.GetErrorMessage(ex));
            }
            return builder.ToString();
        }

        private string GetErrorMessage(Exception ex)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(ex.Message);
            
            bool flag = false;
            for (Exception exception = ex.InnerException; exception != null; exception = exception.InnerException)
            {
                if(!flag)
                {
                    builder.AppendLine();
                    flag = true;
                }
                
                builder.AppendLine(exception.Message);
            }

            return builder.ToString();
        }


    }
}