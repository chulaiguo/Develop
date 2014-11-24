using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace JetCode.DataAccess
{
    public class MSSQLWhereCollection : WhereCollectionBase
    {
        private List<SqlParameter> _parameters = null;

        public MSSQLWhereCollection(string tableAlias)
            : base(tableAlias)
        {
            this._parameters = new List<SqlParameter>();
        }

        public SqlParameter[] SqlParameters
        {
            get
            {
                SqlParameter[] paras = new SqlParameter[this._parameters.Count];

                int i = 0;
                foreach (SqlParameter parameter in this._parameters)
                {
                    paras[i] = parameter;
                    i++;
                }

                return paras;
            }
        }

        private void AddParameter(string fieldName, SqlDbType fieldType, object fieldvalue)
        {
            SqlParameter para = new SqlParameter();
            para.ParameterName = string.Format("@{0}", fieldName);
            para.SqlDbType = fieldType;
            para.SqlValue = fieldvalue;

            this._parameters.Add(para);
        }

        public MSSQLWhereCollection AddEqualWhere(string fieldName, SqlDbType fieldType, object fieldvalue)
        {
            base.AddWhere(string.Format(" [{0}].[{1}]=@{1} ", this.TableAlias, fieldName));
            this.AddParameter(fieldName, fieldType, fieldvalue);
            return this;
        }

        public MSSQLWhereCollection AddNotEqualWhere(string fieldName, SqlDbType fieldType, object fieldvalue)
        {
            base.AddWhere(string.Format(" [{0}].[{1}]!=@{1} ", this.TableAlias, fieldName));
            this.AddParameter(fieldName, fieldType, fieldvalue);
            return this;
        }

        public MSSQLWhereCollection AddGreaterThanWhere(string fieldName, SqlDbType fieldType, object fieldvalue)
        {
            base.AddWhere(string.Format(" [{0}].[{1}]>@{1} ", this.TableAlias, fieldName));
            this.AddParameter(fieldName, fieldType, fieldvalue);
            return this;
        }

        public MSSQLWhereCollection AddLessThanWhere(string fieldName, SqlDbType fieldType, object fieldvalue)
        {
            base.AddWhere(string.Format(" [{0}].[{1}]<@{1} ", this.TableAlias, fieldName));
            this.AddParameter(fieldName, fieldType, fieldvalue);
            return this;
        }

        public MSSQLWhereCollection AddGreaterThanOrEqualWhere(string fieldName, SqlDbType fieldType, object fieldvalue)
        {
            base.AddWhere(string.Format(" [{0}].[{1}]>=@{1} ", this.TableAlias, fieldName));
            this.AddParameter(fieldName, fieldType, fieldvalue);
            return this;
        }

        public MSSQLWhereCollection AddLessThanOrEqualWhere(string fieldName, SqlDbType fieldType, object fieldvalue)
        {
            base.AddWhere(string.Format(" [{0}].[{1}]<=@{1} ", this.TableAlias, fieldName));
            this.AddParameter(fieldName, fieldType, fieldvalue);
            return this;
        }

        public MSSQLWhereCollection AddLikeWhere(string fieldName, SqlDbType fieldType, object fieldvalue)
        {
            base.AddWhere(string.Format(" [{0}].[{1}] LIKE @{1} ", this.TableAlias, fieldName));
            this.AddParameter(fieldName, fieldType, fieldvalue);
            return this;
        }

        public MSSQLWhereCollection AddNotLikeWhere(string fieldName, SqlDbType fieldType, object fieldvalue)
        {
            base.AddWhere(string.Format(" [{0}].[{1}] NOT LIKE @{1} ", this.TableAlias, fieldName));
            this.AddParameter(fieldName, fieldType, fieldvalue);
            return this;
        }

        public MSSQLWhereCollection AddBetweenAndWhere(string fieldName, SqlDbType fieldType, object minValue,
                                                       object maxValue)
        {
            string minValueParameterName = string.Format("Min{0}", fieldName);
            string maxValueParameterName = string.Format("Max{0}", fieldName);
            base.AddWhere(string.Format(" [{0}].[{1}] BETWEEN @{2} AND @{3}", this.TableAlias, fieldName,
                                         minValueParameterName, maxValueParameterName));

            this.AddParameter(minValueParameterName, fieldType, minValue);
            this.AddParameter(maxValueParameterName, fieldType, maxValue);
            return this;
        }

        public MSSQLWhereCollection AddInClause(string fieldName, string inFieldName, string inTableName,
                                                MSSQLWhereCollection where)
        {
            base.AddWhere(string.Format(" [{0}].[{1}] IN ", this.TableAlias, fieldName));
            base.AddWhere(string.Format(" (SELECT [{0}].[{1}] FROM [{2}] AS [{0}] {3}) ", where.TableAlias,
                                         inFieldName,
                                         inTableName, where.SqlWhere));

            this._parameters.AddRange(where.SqlParameters);
            return this;
        }

        public MSSQLWhereCollection AddInClause(string fieldName, string inFieldName, string inTableName)
        {
            base.AddWhere(string.Format(" [{0}].[{1}] IN ", this.TableAlias, fieldName));
            base.AddWhere(string.Format(" (SELECT [{0}] FROM [{1}]) ", inFieldName, inTableName));

            return this;
        }

        public MSSQLWhereCollection AddNotInClause(string fieldName, string inFieldName, string inTableName,
                                                   MSSQLWhereCollection where)
        {
            base.AddWhere(string.Format(" [{0}].[{1}] NOT IN ", this.TableAlias, fieldName));
            base.AddWhere(string.Format(" (SELECT [{0}].[{1}] FROM [{2}] AS [{0}] {3}) ", where.TableAlias,
                                         inFieldName,
                                         inTableName, where.SqlWhere));

            this._parameters.AddRange(where.SqlParameters);
            return this;
        }

        public MSSQLWhereCollection AddNotInClause(string fieldName, string inFieldName, string inTableName)
        {
            base.AddWhere(string.Format(" [{0}].[{1}] NOT IN ", this.TableAlias, fieldName));
            base.AddWhere(string.Format(" (SELECT [{0}] FROM [{1}]) ", inFieldName, inTableName));

            return this;
        }
    }
}