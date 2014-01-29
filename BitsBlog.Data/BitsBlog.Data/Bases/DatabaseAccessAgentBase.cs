using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Data.Interfaces;
using System.Data;

namespace BitsBlog.Data.Bases
{
    public abstract class DatabaseAccessAgentBase : IDatabaseAccessAgent
    {
        private string dbConnection = null;

        /// <summary>
        /// Gets the database connection string.
        /// </summary>
        public string DatabaseConnection
        {
            get { return dbConnection; }
        }

        private DatabaseAccessAgentBase() { }

        public DatabaseAccessAgentBase(string connection)
        {
            this.dbConnection = connection;
        }

        public abstract Dictionary<string, object> ExecuteReader(string storedProcedure, Dictionary<string, object> parameters);

        public abstract Dictionary<string, object> ExecuteReader(string storedProcedure);

        public abstract bool ExecuteNonQuery(string storedProcedure, Dictionary<string, object> parameters);

        public abstract bool ExecuteNonQuery(string storedProcedure);

        public abstract object ExecuteScalar(string storedProcedure, Dictionary<string, object> parameters);

        public abstract object ExecuteScalar(string storedProcedure);
    }
}
