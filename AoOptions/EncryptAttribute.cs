using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class EncryptAttribute : Attribute
	{
		private readonly DataProtectionScope _scope;

		public EncryptAttribute(DataProtectionScope scope = DataProtectionScope.CurrentUser)
		{
			_scope = scope;
		}

		public DataProtectionScope Scope
		{
			get { return _scope; }
		}
	}
}
