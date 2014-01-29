using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace BitsBlog.Security.Utilities
{
    public static class SecureStringHelper
    {
        public unsafe static SecureString Secure(this string source)
        {
            if (source == null)
                return null;
            if (source.Length == 0)
                return new SecureString();

            fixed (char* pChars = source.ToCharArray())
            {
                SecureString secured = new SecureString(pChars, source.Length);
                return secured;
            }
        }

        public static string Unsecure(this SecureString source)
        {
            if (source == null)
                return null;

            IntPtr bstr = Marshal.SecureStringToBSTR(source);
            try
            {
                return Marshal.PtrToStringUni(bstr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(bstr);
            }
        }
    }
}
