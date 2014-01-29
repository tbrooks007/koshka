using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitsBlog.Authentication.Structs
{
    public struct SymmetricKey
    {
        public string AccessKey, SecreteKey;

        public SymmetricKey(string accessKey, string secretKey)
        {
            this.AccessKey = accessKey;
            this.SecreteKey = secretKey;
        }
    }
}
