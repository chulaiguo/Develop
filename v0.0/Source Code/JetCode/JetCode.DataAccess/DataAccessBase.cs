using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using JetCode.Logger;
using Microsoft.ApplicationBlocks.Data;
using System.Text;

namespace JetCode.DataAccess
{
    public abstract class DataAccessBase
    {
        protected static readonly ApplicationLog _SysLog = new ApplicationLog();

        private readonly string _connectionString = string.Empty;
        private readonly SortedList<string, bool> _sortByIndex = new SortedList<string, bool>();

        protected DataAccessBase(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public string ConnectionString
        {
            get { return this._connectionString; }
        }

        public void AddSortBy(string fieldAlias, bool asc)
        {
            fieldAlias = string.Format("[{0}]", fieldAlias);
            if (this._sortByIndex.ContainsKey(fieldAlias))
                return;

            this._sortByIndex.Add(fieldAlias, asc);
        }

        public void ClearSortBy()
        {
            this._sortByIndex.Clear();
        }

        protected SafeDataReader ExecuteDataReader(string where, SqlParameter[] paras)
        {
            string command = string.Format("{0} {1} {2}", this.SQLSelect, where, this.GetOrderByString());
            _SysLog.WriteDebug(command);
            _SysLog.WriteDebug(this.SqlParameterToString(paras));

            try
            {
                return
                    new SafeDataReader(SqlHelper.ExecuteReader(this.ConnectionString, CommandType.Text, command, paras));
            }
            catch (Exception ex)
            {
                _SysLog.WriteError("Exception occured during execute reader.", ex);
                throw new ApplicationException(
                    string.Format("Sql: {0}. Parameters: {1}", command, this.SqlParameterToString(paras)), ex);
            }
        }

        protected DataTable ExecuteDataTable(string sql, SqlParameter[] paras)
        {
            _SysLog.WriteDebug(sql);
            _SysLog.WriteDebug(this.SqlParameterToString(paras));
            try
            {
                return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, paras).Tables[0];
            }
            catch (Exception ex)
            {
                _SysLog.WriteError("Exception occured during execute data table.", ex);
                throw new ApplicationException(
                    string.Format("Sql: {0}. Parameters: {1}", sql, this.SqlParameterToString(paras)), ex);
            }
        }

        public int ExecuteNonQuery(string sql, SqlParameter[] paras)
        {
            _SysLog.WriteDebug(sql);
            _SysLog.WriteDebug(this.SqlParameterToString(paras));
            try
            {
                return SqlHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql, paras);
            }
            catch (Exception ex)
            {
                _SysLog.WriteError("Exception occured during execute non-query.", ex);
                throw new ApplicationException(
                    string.Format("Sql: {0}. Parameters: {1}", sql, this.SqlParameterToString(paras)), ex);
            }
        }

        public object ExecuteScalar(string sql, SqlParameter[] paras)
        {
            _SysLog.WriteDebug(sql);
            _SysLog.WriteDebug(this.SqlParameterToString(paras));
            try
            {
                return SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, paras);
            }
            catch (Exception ex)
            {
                _SysLog.WriteError("Exception occured during execute scalar.", ex);
                throw new ApplicationException(
                    string.Format("Sql: {0}. Parameters: {1}", sql, this.SqlParameterToString(paras)), ex);
            }
        }

        protected int GetDataReaderCount(string where, SqlParameter[] paras)
        {
            return (int) this.ExecuteScalar(this.SQLCount + " " + where, paras);
        }

        private string SqlParameterToString(SqlParameter[] paras)
        {
            if (paras == null || paras.Length == 0)
                return "None";

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < paras.Length; i++)
            {
                builder.AppendFormat("[{0}={1}]", paras[i].ParameterName, paras[i].Value);
            }

            return builder.ToString();
        }


        protected string GetOrderByString()
        {
            if (this._sortByIndex.Count == 0)
                return string.Empty;

            StringBuilder builder = new StringBuilder();
            builder.Append(" ORDER BY ");
            foreach (KeyValuePair<string, bool> pair in this._sortByIndex)
            {
                if (pair.Value)
                {
                    builder.AppendFormat("{0},", pair.Key);
                }
                else
                {
                    builder.AppendFormat("{0} DESC,", pair.Key);
                }
            }

            return builder.ToString().TrimEnd(',');
        }

        protected abstract string SQLCount { get; }

        protected abstract string SQLSelect { get; }
    }
}