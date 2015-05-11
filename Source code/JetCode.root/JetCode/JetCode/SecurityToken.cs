using System;
using System.Collections;
using System.Globalization;

namespace JetCode
{
	[Serializable]
	public class SecurityToken
	{
		private readonly string _userid = "";
		private readonly string _password = "";
		private readonly string _ticks = "";
		private bool _isTrusted = false;
		private bool _isAnonymous = false;

		private Guid _tokenID = Guid.NewGuid();
		private int _blockSize = 1000;
		private int _blockIndex = -1;

		private object _userAccount = null;
		private object _filter = null;
		private Hashtable _permissions = null;

		private string _orderBy = string.Empty;
		private string[] _setting = null;

		private string _parameterNames = string.Empty;
		private object[] _parameters = null;

		public string UserId
		{
			get { return this._userid; }
		}
		public string Password
		{
			get { return this._password; }
		}
		public string Ticks
		{
			get { return this._ticks; }
		}
		public bool IsTrusted
		{
			get { return this._isTrusted; }
		}
		public bool IsAnonymous
		{
			get { return this._isAnonymous; }
		}
		public Guid TokenID
		{
			get { return this._tokenID; }
		}
		public int BlockSize
		{
			get { return this._blockSize; }
		}
		public int BlockIndex
		{
			get { return this._blockIndex; }
		}

		public object UserAccount
		{
			get { return this._userAccount; }
			set { this._userAccount = value; }
		}
		public object Filter
		{
			get { return this._filter; }
			set { this._filter = value; }
		}
		public Hashtable Permissions
		{
			get { return this._permissions; }
			set { this._permissions = value; }
		}

		public string ParameterNames
		{
			get { return this._parameterNames; }
		}

		public string GetOrderBySQL()
		{
		    if (string.IsNullOrEmpty(this._orderBy))
		        return string.Empty;

            return string.Format("ORDER BY {0}", this._orderBy);
		}

		public bool IsSettingExist(string key)
		{
			if (this._setting == null)
			{
				return false;
			}

		    foreach (string item in this._setting)
		    {
		        if (String.Compare(item, key, StringComparison.OrdinalIgnoreCase) == 0)
		        {
		            return true;
		        }
		    }

			return false;
		}
		public object GetParameter(int index)
		{
			object result;
			if (this._parameters == null || index < 0 || index >= this._parameters.Length)
			{
				result = null;
			}
			else
			{
				result = this._parameters[index];
			}
			return result;
		}

		private SecurityToken(SecurityToken token)
		{
			this._userid = token.UserId;
			this._password = token.Password;
			this._ticks = token.Ticks;
			this._userAccount = token.UserAccount;
			this._filter = token.Filter;
			this._permissions = token.Permissions;
			this._isTrusted = token.IsTrusted;
			this._isAnonymous = token.IsAnonymous;
			this._setting = token._setting;
		}
		public SecurityToken(string userid, string password)
		{
			this.SetAsAnonymous(userid, password);
			this._userid = userid;
			this._ticks = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
			this._password = this.HashValue(password);
		}
		public SecurityToken(string userid, string password, string secret)
		{
			this.SetAsAnonymous(userid, password);
			this.SetAsTrusted(secret);
			this._userid = userid;
			this._ticks = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
			this._password = this.HashValue(password);
		}
		public SecurityToken(string userid, string password, string secret, string ticks)
		{
			this.SetAsAnonymous(userid, password);
			this.SetAsTrusted(secret);
			this._userid = userid;
			this._ticks = ticks;
			this._password = this.HashValue(password);
		}

		public static SecurityToken CreateDuplicateToken(SecurityToken originalToken, bool all)
		{
			if (originalToken == null)
			{
				return null;
			}
			
			if (all)
			{
				return CreateApplicationToken(originalToken, originalToken._blockIndex, originalToken._blockSize, originalToken._tokenID, originalToken._orderBy, originalToken._setting);
			}
			
            return new SecurityToken(originalToken);
		}

		public static SecurityToken CreateSettingToken(SecurityToken originalToken, string[] setting)
		{
			if (originalToken == null || setting == null)
			{
			    return null;
			}

			return CreateApplicationToken(originalToken, originalToken._blockIndex, originalToken._blockSize, originalToken._tokenID, originalToken._orderBy, setting);
		}
		public static SecurityToken CreatePagesToken(SecurityToken originalToken, int blockIndex, int blockSize, Guid tokenID)
		{
			if (originalToken == null || blockIndex < 0 || blockSize <= 0 || tokenID == Guid.Empty)
			{
			    return null;
			}
			
			return CreateApplicationToken(originalToken, blockIndex, blockSize, tokenID, originalToken._orderBy, originalToken._setting);
		}
		public static SecurityToken CreateSortFieldsToken(SecurityToken originalToken, string orderBy)
		{
            if (originalToken == null || orderBy == null)
            {
                return null;
            }
			
            return CreateApplicationToken(originalToken, originalToken._blockIndex, originalToken._blockSize, originalToken._tokenID, orderBy, originalToken._setting);
		}

		public static SecurityToken CreateSortFieldsToken(SecurityToken originalToken, string fieldName, bool asc)
		{
			if (originalToken == null)
			{
			    return null;
			}

		    string orderBy = string.Format("{0} {1}", fieldName, asc ? "ASC" : "DESC");
            return CreateApplicationToken(originalToken, originalToken._blockIndex, originalToken._blockSize, originalToken._tokenID, orderBy, originalToken._setting);
		}

        public static SecurityToken CreateSortFieldsToken(SecurityToken originalToken, string fieldName1, bool asc1, string fieldName2, bool asc2)
        {
            if (originalToken == null)
            {
                return null;
            }

            string orderBy = string.Format("{0} {1}, {2} {3}", fieldName1, asc1 ? "ASC" : "DESC", fieldName2, asc2 ? "ASC" : "DESC");
            return CreateApplicationToken(originalToken, originalToken._blockIndex, originalToken._blockSize, originalToken._tokenID, orderBy, originalToken._setting);
        }


		public static SecurityToken CreateApplicationToken(SecurityToken originalToken, int blockIndex, int blockSize, Guid tokenID, string orderBy, string[] setting)
		{
			SecurityToken securityToken = new SecurityToken(originalToken);
			if (blockIndex >= 0)
			{
				securityToken._blockIndex = blockIndex;
			}
			if (blockSize > 0)
			{
				securityToken._blockSize = blockSize;
			}
			if (tokenID != Guid.Empty)
			{
				securityToken._tokenID = tokenID;
			}
			if (orderBy != null)
			{
			    securityToken._orderBy = orderBy;
			}
			if (setting != null)
			{
				securityToken._setting = new string[setting.Length];
				for (int i = 0; i < setting.Length; i++)
				{
					securityToken._setting[i] = setting[i];
				}
			}
			return securityToken;
		}
		public static SecurityToken CreateFrameworkToken(SecurityToken originalToken, string[] paraNames, object[] paraValue)
		{
			SecurityToken securityToken = CreateApplicationToken(originalToken, originalToken._blockIndex, originalToken._blockSize, originalToken._tokenID, originalToken._orderBy, originalToken._setting);
			SecurityToken result;
			if (paraNames == null || paraValue == null || paraNames.Length != paraValue.Length)
			{
				result = securityToken;
			}
			else
			{
				securityToken._parameters = new object[paraValue.Length];
				for (int i = 0; i < paraValue.Length; i++)
				{
					if (i == 0)
					{
						securityToken._parameterNames = string.Format("{0}", paraNames[i]);
					}
					else
					{
						securityToken._parameterNames = string.Format("{0}|{1}", securityToken._parameterNames, paraNames[i]);
					}
					securityToken._parameters[i] = paraValue[i];
				}
				result = securityToken;
			}
			return result;
		}

		public override bool Equals(object obj)
		{
			SecurityToken securityToken = obj as SecurityToken;
			return securityToken != null && String.Compare(this.UserId, securityToken.UserId, StringComparison.OrdinalIgnoreCase) == 0 && !(this.Password != securityToken.Password);
		}

		public override int GetHashCode()
		{
			return string.Format("{0}{1}", this.UserId, this.Password).GetHashCode();
		}

		private string HashValue(string value)
		{
			string text = string.Empty;
			for (int i = 0; i < value.Length; i++)
			{
				int num;
				if (i < this.Ticks.Length)
				{
					num = (byte)value[i] + (byte)this.Ticks[i];
				}
				else
				{
					num = (byte)value[i] + 18;
				}
				text = text + num + " ";
			}
			return text;
		}
        public void SetAsTrusted(string secret)
        {
            this._isTrusted = (secret == "HelloDataServiceEx");
        }

		private void SetAsAnonymous(string userid, string password)
		{
			this._isAnonymous = (userid == "anonymous" && password == "anonymous");
		}
	}
}
