using System;

namespace JetCode
{
    [Serializable]
    public class SortField
    {
        private readonly bool _asc = false;
        private readonly string _fieldAlias = string.Empty;

        public SortField(string fieldAlias, bool asc)
        {
            this._fieldAlias = fieldAlias;
            this._asc = asc;
        }

        public bool ASC
        {
            get { return this._asc; }
        }

        public string FieldAlias
        {
            get { return this._fieldAlias; }
        }
    }
}