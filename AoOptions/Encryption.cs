using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

// requires reference to System.Security

namespace AdamOneilSoftware
{
    internal static class Encryption
    {
        internal static string Encrypt(string clearText, DataProtectionScope scope = DataProtectionScope.CurrentUser)
        {
            byte[] clearBytes = Encoding.ASCII.GetBytes(clearText);
            byte[] encryptedBytes = ProtectedData.Protect(clearBytes, null, scope);
            return Convert.ToBase64String(encryptedBytes);
        }

		internal static string Decrypt(string encryptedText, DataProtectionScope scope = DataProtectionScope.CurrentUser)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] clearBytes = ProtectedData.Unprotect(encryptedBytes, null, scope);
            return Encoding.ASCII.GetString(clearBytes);
        }
    }
}
