using System;
using System.Data;
using System.Transactions;
using JetCode.DataSecurity;

namespace JetCode.DataServiceBase
{
    public abstract class DataServiceBase : MarshalByRefObject
    {
        private string _connectionString = string.Empty;
        private string _userid = string.Empty;
        private string _password = string.Empty;
        private bool _isAuthorized = false;

        private IDBLog _dbLog = null;
        private IPermission _permission = null;

        public DataServiceBase(string connectionString, string userid, string password)
        {
            this._userid = userid;
            this._password = password;
            this._connectionString = connectionString;

            this._dbLog = PluginDBLog.GetDBLog();
            this._permission = PluginPermission.GetPermission();
        }

        protected virtual bool Selectable(BusinessBase entity)
        {
            return true;
        }

        protected virtual bool Insertable(BusinessBase entity)
        {
            return true;
        }

        protected virtual bool Deletable(BusinessBase entity)
        {
            return true;
        }

        protected virtual bool Editable(BusinessBase entity)
        {
            return true;
        }

        private bool IsAuthorizedUser()
        {
            if (!this._isAuthorized && this._permission.IsAuthorized(this._userid, this._password))
            {
                this._isAuthorized = true;
            }
            return this._isAuthorized;
        }

        private bool HasSelectPermission()
        {
            if (!this.IsAuthorizedUser())
            {
                return false;
            }
            if (!this._permission.Selectable(this._userid, this.TableName))
            {
                return false;
            }
            return true;
        }

        protected BusinessBase Filter(BusinessBase entity)
        {
            return ((this.HasSelectPermission() && this.Selectable(entity)) ? entity : null);
        }

        protected BusinessCollectionBase Filter(BusinessCollectionBase list)
        {
            if (!this.HasSelectPermission())
            {
                list.Clear();
            }
            else
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    BusinessBase entity = list[i] as BusinessBase;
                    if (!this.Selectable(entity))
                    {
                        list.RemoveAt(i);
                    }
                }
            }
            list.AcceptChanges();
            return list;
        }

        protected Result Save(BusinessBase entity)
        {
            if (!this.IsAuthorizedUser())
            {
                return new Result(string.Format("The user {0} is not authorized, access denied.", this._userid));
            }

            Result rowVersion = new Result(true);
            if (entity == null || !entity.IsDirty)
                return rowVersion;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    this.SaveChild(entity);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    return new Result(ex);
                } 
            }

            return this.GetRowVersion(entity);
        }

        protected Result Save(BusinessCollectionBase list)
        {
            if (!this.IsAuthorizedUser())
            {
                return new Result(string.Format("The user {0} is not authorized, access denied.", this._userid));
            }

            if (list == null || !list.IsDirty)
                return new Result(true);

            Result result = new Result(true);
            list.GetChanges();
            foreach (BusinessBase item in list)
            {
                if (!item.IsDirty)
                    continue;

                Result r = this.Save(item);
                result.Add(r);
            }

            return result;
        }

        private void SaveChild(BusinessBase entity)
        {
            if (entity == null || !entity.IsDirty)
                return;

            if (!entity.IsDeleted && !entity.IsValid)
            {
                throw new InvalidOperationException("Object is not valid (broken rules) and cannot be saved\r\n" +
                                                    entity.BrokenRules);
            }

            if (entity.IsDeleted)
            {
                if (!this._permission.Deletable(this._userid, this.TableName) || !this.Deletable(entity))
                {
                    throw new InvalidOperationException(
                        string.Format("The user {0} doesn't have the delete permission for table {1}, access denied.",
                                      this._userid, entity.TableName));
                }

                int affectedRows = this.DeleteEntity(entity);
                this.WriteDeleteDBLog(entity, affectedRows);
            }
            else
            {
                if (entity.IsNew)
                {
                    if (!this._permission.Insertable(this._userid, this.TableName) || !this.Insertable(entity))
                    {
                        throw new InvalidOperationException(string.Format(
                                                                "The user {0} doesn't have the insert permission for table {1}, access denied.",
                                                                this._userid, entity.TableName));
                    }

                    int affectedRows = this.InsertEntity(entity);
                    this.WriteInsertDBLog(entity, affectedRows);
                }
                else if (entity.IsSelfDirty)
                {
                    if (!this._permission.Editable(this._userid, this.TableName) || !this.Editable(entity))
                    {
                        throw new InvalidOperationException(string.Format(
                                                                "The user {0} doesn't have the edit permission for table {1}, access denied.",
                                                                this._userid, entity.TableName));
                    }

                    int affectedRows = this.UpdateEntity(entity);
                    this.WriteUpdateDBLog(entity, affectedRows);
                }

                this.UpdateChildren(entity);
            }
        }

        protected abstract void UpdateChildren(BusinessBase entity);
        protected void UpdateChildren(BusinessCollectionBase list)
        {
            if (list == null || !list.IsDirty)
                return;

            list.GetChanges();
            foreach (BusinessBase item in list)
            {
                if (!item.IsDirty)
                    continue;

                this.SaveChild(item);
            }
        }

        protected abstract Result GetRowVersion(BusinessBase entity);
        protected Result GetRowVersion(BusinessCollectionBase list)
        {
            Result result = new Result(true);
            foreach (BusinessBase item in list)
            {
                result.Add(this.GetRowVersion(item));
            }

            return result;
        }

        protected abstract int InsertEntity(BusinessBase entity);
        protected abstract int DeleteEntity(BusinessBase entity);
        protected abstract int UpdateEntity(BusinessBase entity);


        protected string Password
        {
            get { return this._password; }
        }

        protected string UserID
        {
            get { return this._userid; }
        }

        protected string ConnectionString
        {
            get { return this._connectionString; }
        }

        protected virtual string TableName
        {
            get { return string.Empty; }
        }

        #region DBLog

        private void WriteDeleteDBLog(BusinessBase entity, int affectedRows)
        {
            if (affectedRows != 1)
            {
                throw new DBConcurrencyException(
                    string.Format("Delete entity failed. {0} rows have been affected.", affectedRows));
            }

            this._dbLog.LogDelete(entity);
        }

        private void WriteInsertDBLog(BusinessBase entity, int affectedRows)
        {
            if (affectedRows != 1)
            {
                throw new DBConcurrencyException(
                    string.Format("Insert entity failed. {0} rows have been affected.", affectedRows));
            }

            this._dbLog.LogDelete(entity);
        }

        private void WriteUpdateDBLog(BusinessBase entity, int affectedRows)
        {
            if (affectedRows != 1)
            {
                throw new DBConcurrencyException(
                    string.Format("Update entity failed. {0} rows have been affected.", affectedRows));
            }

            this._dbLog.LogDelete(entity);
        }

        #endregion
    }
}