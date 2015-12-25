using System;
using System.Data;
using System.Transactions;
using JetCode.BusinessEntity;
using JetCode.Logger;

namespace JetCode.DataServiceBase
{
    public abstract class ServiceBase : MarshalByRefObject
    {
        private readonly string _connectionString = string.Empty;
        private readonly SecurityToken _securityToken = null;
        private readonly SecurityToken _trustedToken = null;

        protected static readonly IDBEditLog _DBLog = DBEditLogBuilder.GetDBEditLog();
        protected static readonly ApplicationLog _SysLog = new ApplicationLog();

        protected static readonly IPermission _Permission = PermissionBuilder.GetPermission();
        private TablePermission _tablePermission = null;

        protected ServiceBase(string connectionString, SecurityToken securityToken)
        {
            this._connectionString = connectionString;
            this._securityToken = securityToken;
            this._trustedToken = new SecurityToken(securityToken.UserId, string.Empty);
            this._trustedToken.SetAsTrusted("HelloDataServiceEx");
        }

        protected string ConnectionString
        {
            get { return this._connectionString; }
        }

        protected SecurityToken SecurityToken
        {
            get { return this._securityToken; }
        }

        protected SecurityToken TrustedToken
        {
            get { return this._trustedToken; }
        }

        protected string UserID
        {
            get { return this._securityToken.UserId; }
        }

        protected string Password
        {
            get { return this._securityToken.Password; }
        }

        protected bool IsTrusted
        {
            get { return this._securityToken.IsTrusted; }
        }

        protected bool IsAuthorizedSelect
        {
            get { return this.TablePermission.Selectable; }
        }

        protected virtual string TableName
        {
            get { return string.Empty; }
        }

        private TablePermission TablePermission
        {
            get
            {
                if (this._tablePermission == null)
                {
                    this._tablePermission = _Permission.GetPermission(this.SecurityToken, this.TableName);
                }
                return this._tablePermission;
            }
        }


        protected virtual void AfterDelete(BusinessBase entity)
        {
        }

        protected virtual void AfterInsert(BusinessBase entity)
        {
        }

        protected virtual void AfterUpdate(BusinessBase entity)
        {
        }

        protected virtual void BeforeDelete(BusinessBase entity)
        {
        }

        protected virtual void BeforeInsert(BusinessBase entity)
        {
        }

        protected virtual void BeforeUpdate(BusinessBase entity)
        {
        }


        protected bool IsAuthorizedDelete(BusinessBase entity)
        {
            if (this.IsTrusted)
            {
                return true;
            }
            if (!this.TablePermission.Deletable)
            {
                return false;
            }
            return this.Deletable(entity);
        }

        protected bool IsAuthorizedInsert(BusinessBase entity)
        {
            if (this.IsTrusted)
            {
                return true;
            }
            if (!this.TablePermission.Insertable)
            {
                return false;
            }
            return this.Insertable(entity);
        }

        protected bool IsAuthorizedUpdate(BusinessBase entity)
        {
            if (this.IsTrusted)
            {
                return true;
            }
            if (!this.TablePermission.Updatable)
            {
                return false;
            }
            return this.Editable(entity);
        }

        protected virtual bool Deletable(BusinessBase entity)
        {
            return true;
        }

        protected virtual bool Editable(BusinessBase entity)
        {
            return true;
        }

        protected virtual bool Insertable(BusinessBase entity)
        {
            return true;
        }

        protected virtual bool Selectable(BusinessBase entity)
        {
            return true;
        }


        protected virtual BusinessBase Filter(BusinessBase entity)
        {
            if (entity == null)
                return null;

            if (!entity.IsActive)
                return null;

            if (!this.Selectable(entity))
                return null;

            return entity;
        }

        protected virtual BusinessCollectionBase Filter(BusinessCollectionBase list)
        {
            if (list == null)
                return null;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (this.Filter(list[i]) == null)
                {
                    list.RemoveAt(i);
                }
            }

            list.AcceptChanges();
            return list;
        }

        public Result GetRowVersion(BusinessCollectionBase list)
        {
            Result result = new Result(true);
            foreach (BusinessBase item in list)
            {
                result.Add(this.GetRowVersion(item));
            }
            return result;
        }

        protected abstract int DeleteEntity(BusinessBase entity);
        protected abstract int InsertEntity(BusinessBase entity);
        protected abstract int UpdateEntity(BusinessBase entity);
        protected abstract void UpdateChildren(BusinessBase entity);
        protected abstract Result GetRowVersion(BusinessBase entity);


        public Result Save(BusinessBase entity)
        {
            Result result = new Result(true);

            if (entity == null || !entity.IsDirty)
                return result;

            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    this.SaveUnderTransaction(entity);
                    scope.Complete();
                    flag = true;
                }
                catch (Exception exception)
                {
                    result = new Result(entity, exception);
                    _SysLog.WriteFatal("Exception occured in saving entity.", exception);
                }
            }

            if (flag)
            {
                _SysLog.WriteInfo("Commited transaction successfully.");
                result = this.GetRowVersion(entity);
            }

            return result;
        }

        protected Result Save(BusinessCollectionBase list)
        {
            Result result = new Result(true);

            if (list == null || !list.IsDirty)
                return result;

            list.GetChanges();
            foreach (BusinessBase item in list)
            {
                if (item.IsDirty)
                {
                    Result r = this.Save(item);
                    result.Add(r);
                }
            }

            return result;
        }

        public void SaveUnderTransaction(BusinessBase entity)
        {
            if (entity == null || !entity.IsDirty)
                return;

            if (entity.IsLogicDeleted)
            {
                entity.ToLogicDeleted();
            }

            if (!entity.IsDeleted && !entity.IsValid)
            {
                throw new InvalidOperationException("Object is not valid (broken rules) and cannot be saved\r\n" 
                    + entity.GetBrokenRules());
            }

            try
            {
                if (entity.IsDeleted)
                {
                    if (!this.IsAuthorizedDelete(entity))
                    {
                        throw new InvalidOperationException(
                            string.Format("The user {0} doesn't have the delete permission, access denied.",
                                          this.UserID));
                    }

                    this.BeforeDelete(entity);
                    int num = this.DeleteEntity(entity);
                    if (num != 1)
                    {
                        throw new DBConcurrencyException(
                            string.Format("Too many or zero rows have been affected for deletion. (rows={0}, user='{1}')",
                                num, this.UserID));
                    }
                    this.AfterDelete(entity);
                }
                else
                {
                    if (entity.IsNew)
                    {
                        if (!this.IsAuthorizedInsert(entity))
                        {
                            throw new InvalidOperationException(
                                string.Format("The user {0} doesn't have the insert permission, access denied.",
                                    this.UserID));
                        }

                        this.BeforeInsert(entity);
                        entity.CreatedOn = DateTime.Now;
                        entity.CreatedBy = this.UserID;
                        entity.ModifiedOn = DateTime.Now;
                        entity.ModifiedBy = this.UserID;

                        int num = this.InsertEntity(entity);
                        if (num != 1)
                        {
                            throw new DBConcurrencyException(
                                string.Format("Too many or zero rows have been affected for insertion. (rows={0}, user='{1}')",
                                    num, this.UserID));
                        }

                        num = _DBLog.LogInsert(entity, this.UserID);
                        if (num != 1)
                        {
                            throw new DBConcurrencyException(
                                string.Format("Too many or zero rows have been affected for DBLogInsert insertion. (rows={0}, user='{1}')",
                                    num, this.UserID));
                        }
                        this.AfterInsert(entity);
                    }
                    else if (entity.IsSelfDirty)
                    {
                        if (!this.IsAuthorizedUpdate(entity))
                        {
                            throw new InvalidOperationException(
                                string.Format("The user {0} doesn't have the edit permission, access denied.",
                                              this.UserID));
                        }

                        this.BeforeUpdate(entity);
                        entity.ModifiedOn = DateTime.Now;
                        entity.ModifiedBy = this.UserID;

                        int num = this.UpdateEntity(entity);
                        if (num != 1)
                        {
                            throw new DBConcurrencyException(
                                string.Format("Too many or zero rows have been affected for updating. (rows={0}, user='{1}')",
                                    num, this.UserID));
                        }

                        num = _DBLog.LogUpdate(entity, this.UserID);
                        if (num != 1)
                        {
                            throw new DBConcurrencyException(
                                string.Format("Too many or zero rows have been affected for DBLogUpdate insertion. (rows={0}, user='{1}')",
                                    num, this.UserID));
                        }
                        this.AfterUpdate(entity);
                    }

                    this.UpdateChildren(entity);
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException(entity.ToString(), exception);
            }
        }

        public void SaveUnderTransaction(BusinessCollectionBase list)
        {
            if (list == null || !list.IsDirty)
                return;

            list.GetChanges();
            foreach (BusinessBase item in list)
            {
                if (item.IsDirty)
                {
                    this.SaveUnderTransaction(item);
                }
            }
        }
    }
}