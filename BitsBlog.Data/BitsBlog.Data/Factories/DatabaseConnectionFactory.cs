using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Configuration;

namespace BitsBlog.Data.Factories
{
    internal class DatabaseConnectionFactory
    {
        public DatabaseConnectionFactory() { }

        public static DbConnection GetConnection(string connectionString)
        {
            string providerName = null;
            var csb = new DbConnectionStringBuilder { ConnectionString = connectionString };

            if (csb.ContainsKey("provider"))
            {
                providerName = csb["provider"].ToString();
            }
            else
            {
                var css = ConfigurationManager
                                  .ConnectionStrings
                                  .Cast<ConnectionStringSettings>()
                                  .FirstOrDefault(x => x.ConnectionString == connectionString);

                if (css != null) 
                    providerName = css.ProviderName;
            }

            if (providerName != null)
            {
                var providerExists = DbProviderFactories
                                            .GetFactoryClasses()
                                            .Rows.Cast<DataRow>()
                                            .Any(r => r[2].Equals(providerName));
                if (providerExists)
                {
                    var factory = DbProviderFactories.GetFactory(providerName);
                    return factory.CreateConnection();
                }
            }

            return null;
        }

        public static string GetProviderName(string connectionString)
        {
            string providerName = null;
            var csb = new DbConnectionStringBuilder { ConnectionString = connectionString };

            if (csb.ContainsKey("provider"))
            {
                providerName = csb["provider"].ToString();
            }
            else
            {
                var css = ConfigurationManager
                                  .ConnectionStrings
                                  .Cast<ConnectionStringSettings>()
                                  .FirstOrDefault(x => x.ConnectionString == connectionString);

                if (css != null)
                    providerName = css.ProviderName;
            }

            if (providerName != null)
            {
                var providerExists = DbProviderFactories
                                            .GetFactoryClasses()
                                            .Rows.Cast<DataRow>()
                                            .Any(r => r[2].Equals(providerName));
                if (providerExists)
                    return providerName;
            }

            return null;
        }
    }
}
