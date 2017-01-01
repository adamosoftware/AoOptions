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
        internal static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.ASCII.GetBytes(clearText);
            byte[] encryptedBytes = ProtectedData.Protect(clearBytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedBytes);
        }

		internal static string Decrypt(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] clearBytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
            return Encoding.ASCII.GetString(clearBytes);
        }
    }
}
