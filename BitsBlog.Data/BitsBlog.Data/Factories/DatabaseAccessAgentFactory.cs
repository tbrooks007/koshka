using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Data.Configurations;
using BitsBlog.Data.Interfaces;

namespace BitsBlog.Data.Factories
{
    internal class DatabaseAccessAgentFactory
    {
        public static IDatabaseAccessAgent CreateAccessAgent(string connectionString)
        {
            IDatabaseAccessAgent agent = null;

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                string providerName = DatabaseConnectionFactory.GetProviderName(connectionString);
                agent = GetDatabaseAccessAgent(providerName, connectionString);
            }
            else
            {
                throw new ArgumentException("connectionString","The connectionString parameter can not be null or blank.");
            }

            return agent;
        }

        private static IDatabaseAccessAgent GetDatabaseAccessAgent(string providerName, string connectionString)
        {
            //TODO add param checking

            IDatabaseAccessAgent agent = null;

            switch (providerName.ToLower())
            {
                case "system.data.sqlclient":
                    agent = new SQLServerDataAccessAgent(connectionString);
                    break;
            }

            return agent;
        }
    }
}
