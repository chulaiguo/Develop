using System;
using System.Collections;
using System.Text;

namespace JetCode
{
    [Serializable]
    public class SecurityToken
    {
        private readonly string _userid = string.Empty;
        private readonly string _ticks = string.Empty;
        private string _password = string.Empty;
        private string _secret = string.Empty;
        private string _projectName = string.Empty;
        private string _clientName = string.Empty;
        private string _clientIPAddress = string.Empty;

        private Guid _tokenID = Guid.NewGuid();
        private int _blockIndex = -1;
        private int _blockSize = 1000;

        private bool _includeInactive = false;
        private readonly SortFieldCollection _sortFields = new SortFieldCollection();

        private object _userAccount = null;
        private Hashtable _permissions = null;
        private bool _isAnonymous = false;
        private bool _isTrusted = false;

        public SecurityToken(string userid, string password)
        {
            this._userid = userid;
            this._ticks = DateTime.Now.Ticks.ToString();
            this._password = this.HashValue(password);
            this._secret = string.Empty;
        }

        public SecurityToken(string userid, string password, string secret)
        {
            this._userid = userid;
            this._ticks = DateTime.Now.Ticks.ToString();
            this._password = this.HashValue(password);
            this._secret = this.HashValue(secret);
        }

        public SecurityToken(string userid, string password, string secret, string ticks)
        {
            this._userid = userid;
            this._ticks = ticks;
            this._password = this.HashValue(password);
            this._secret = this.HashValue(secret);
        }

        public SecurityToken(string userid, string password, string secret, int index, int size)
        {
            this._userid = userid;
            this._ticks = DateTime.Now.Ticks.ToString();
            this._password = this.HashValue(password);
            this._secret = this.HashValue(secret);
            this._blockIndex = index;
            this._blockSize = size;
        }

        public SecurityToken(string userid, string password, string secret, string ticks, int index, int size)
        {
            this._userid = userid;
            this._ticks = ticks;
            this._password = this.HashValue(password);
            this._secret = this.HashValue(secret);
            this._blockIndex = index;
            this._blockSize = size;
        }

        public override bool Equals(object obj)
        {
            SecurityToken token = obj as SecurityToken;
            if (token == null)
                return false;

            if (string.Compare(this.UserId, token.UserId, true) != 0 
                || this.Password != token.Password 
                || this.Secret != token.Secret)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}{1}{2}", this.UserId, this.Password, this.Secret).GetHashCode();
        }

        private string HashValue(string value)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
                int num;
                if (i < this.Ticks.Length)
                {
                    num = ((byte) value[i]) + ((byte) this.Ticks[i]);
                }
                else
                {
                    num = ((byte) value[i]) + 18;
                }

                builder.AppendFormat("{0} ", num);
            }

            return builder.ToString();
        }

        public void ResetQueryParas()
        {
            this._blockIndex = -1;
            this._blockSize = 1000;
            this._sortFields.Clear();
        }

        public int BlockIndex
        {
            get { return this._blockIndex; }
            set { this._blockIndex = value; }
        }

        public int BlockSize
        {
            get { return this._blockSize; }
            set { this._blockSize = value; }
        }

        public bool IncludeInactive
        {
            get { return this._includeInactive; }
            set { this._includeInactive = value; }
        }

        public bool IsAnonymous
        {
            get { return this._isAnonymous; }
            set { this._isAnonymous = value; }
        }

        public bool IsTrusted
        {
            get { return this._isTrusted; }
            set { this._isTrusted = value; }
        }

        public string Password
        {
            get { return this._password; }
            set { this._password = value; }
        }

        public Hashtable Permissions
        {
            get { return this._permissions; }
            set { this._permissions = value; }
        }

        public string Secret
        {
            get { return this._secret; }
            set { this._secret = value; }
        }

        public SortFieldCollection SortFields
        {
            get { return this._sortFields; }
        }

        public string Ticks
        {
            get { return this._ticks; }
        }

        public Guid TokenID
        {
            get { return this._tokenID; }
            set { this._tokenID = value; }
        }

        public object UserAccount
        {
            get { return this._userAccount; }
            set { this._userAccount = value; }
        }

        public string UserId
        {
            get { return this._userid; }
        }

        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        public string ClientName
        {
            get { return _clientName; }
            set { _clientName = value; }
        }

        public string ClientIpAddress
        {
            get { return _clientIPAddress; }
            set { _clientIPAddress = value; }
        }
    }
}