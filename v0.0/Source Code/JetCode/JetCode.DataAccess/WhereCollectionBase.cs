using System.Text;

namespace JetCode.DataAccess
{
    public abstract class WhereCollectionBase
    {
        private string _tableAlias = string.Empty;
        private StringBuilder _sqlBuilder = null;

        public WhereCollectionBase(string tableAlias)
        {
            this._tableAlias = tableAlias;
        }

        public string SqlWhere
        {
            get
            {
                if (this._sqlBuilder == null)
                    return string.Empty;

                return string.Format(" WHERE {0}", this._sqlBuilder);
            }
        }

        protected string TableAlias
        {
            get { return _tableAlias; }
        }

        protected void AddWhere(string where)
        {
            if (this._sqlBuilder == null)
            {
                this._sqlBuilder = new StringBuilder();
            }
            else
            {
                this._sqlBuilder.Append(" AND ");
            }

            this._sqlBuilder.Append(where);
        }
    }
}