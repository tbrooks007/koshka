using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Data.Bases;
using System.Data;
using System.Data.SqlClient;
using BitsBlog.Data.Interfaces;

namespace BitsBlog.Data
{
    internal sealed class SQLServerDataAccessAgent : DatabaseAccessAgentBase
    {
        public SQLServerDataAccessAgent(string connection) : base(connection) { }

        public override Dictionary<string, object> ExecuteReader(string storedProcedure, Dictionary<string, object> parameters)
        {
            if (String.IsNullOrWhiteSpace(storedProcedure)) throw new ArgumentNullException("storedProcedure", "The storedProcedure parameter can not be null or empty.");
            if (parameters == null && parameters.Count > 0) throw new ArgumentNullException("parameters", "The parameters parameter cant not be null or empty.");

            IDataReader dataReader = null;
            IDbCommand cmd = null;
            SqlParameter sqlParam = null;
            Dictionary<string, object> rtnValues = null;

            using (IDbConnection conn = new SqlConnection(DatabaseConnection))
            {
                try
                {
                    rtnValues = new Dictionary<string, object>();
                    cmd = new SqlCommand(storedProcedure);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;

                    foreach (KeyValuePair<string, object> param in parameters)
                    {
                        sqlParam = new SqlParameter(param.Key, param.Value);
                        cmd.Parameters.Add(sqlParam);
                    }

                    conn.Open();
                    dataReader = cmd.ExecuteReader();
                    rtnValues = this.GetResultValuesFromDataReader(dataReader);
                }
                catch (Exception e)
                {
                    //TODO throw or log error
                }
            }

            return rtnValues;
        }

        public override Dictionary<string, object> ExecuteReader(string storedProcedure)
        {
            if (String.IsNullOrWhiteSpace(storedProcedure)) throw new ArgumentNullException("storedProcedure", "The storedProcedure parameter can not be null or empty.");

            IDataReader dataReader = null;
            IDbCommand cmd = null;
            Dictionary<string, object> rtnValues = null;

            using (IDbConnection conn = new SqlConnection(DatabaseConnection))
            {
                try
                {
                    rtnValues = new Dictionary<string, object>();
                    cmd = new SqlCommand(storedProcedure);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;

                    conn.Open();
                    dataReader = cmd.ExecuteReader();
                    rtnValues = this.GetResultValuesFromDataReader(dataReader);
                }
                catch(Exception e)
                {
                    //TODO throw or log error
                }
            }

            return rtnValues;
        }

        public override bool ExecuteNonQuery(string storedProcedure, Dictionary<string, object> parameters)
        {
            if (String.IsNullOrWhiteSpace(storedProcedure)) throw new ArgumentNullException("storedProcedure", "The storedProcedure parameter can not be null or empty.");
            if (parameters == null && parameters.Count > 0) throw new ArgumentNullException("parameters", "The parameters parameter cant not be null or empty.");
            
            bool isSuccess = false;
            IDbCommand cmd = null;
            SqlParameter sqlParam = null;

            using (IDbConnection conn = new SqlConnection(DatabaseConnection))
            {
                try
                {
                    cmd = new SqlCommand(storedProcedure);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;

                    foreach (KeyValuePair<string, object> param in parameters)
                    {
                        sqlParam = new SqlParameter(param.Key, param.Value);
                        cmd.Parameters.Add(sqlParam);
                    }

                    conn.Open();
                    int numberOfRowsAffected = cmd.ExecuteNonQuery();
                    isSuccess = numberOfRowsAffected > 0 ? true : false;
                }
                catch (Exception e)
                {
                    //TODO throw or log error
                    Console.WriteLine(e.Message);
                }
            }

            return isSuccess;
        }

        #region Access Agent Utils

        private Dictionary<string, object> GetResultValuesFromDataReader(IDataReader dataReader)
        {
            if (dataReader == null) throw new ArgumentNullException("dataReader");

            Dictionary<string, object> rtnValues = new Dictionary<string, object>();

            while (dataReader.Read())
            {
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    string name = dataReader.GetName(i);
                    object value = dataReader.GetValue(i) as object;

                    if (value != null && value.ToString().Length > 0)
                        rtnValues.Add(name, value);
                }
            }

            return rtnValues;
        }

        #endregion

        #region unimplemented methods

        public override bool ExecuteNonQuery(string storedProcedure)
        {
            throw new NotImplementedException();
        }

        public override object ExecuteScalar(string storedProcedure, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public override object ExecuteScalar(string storedProcedure)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
