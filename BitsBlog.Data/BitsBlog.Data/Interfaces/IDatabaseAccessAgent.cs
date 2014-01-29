using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitsBlog.Data.Interfaces
{
    /// <summary>
    /// Common interface that sets the basic contract for all database access agents supported by the Bits-Blog Engine.
    /// </summary>
    public interface IDatabaseAccessAgent
    {
        string DatabaseConnection { get; }

        Dictionary<string, object> ExecuteReader(string storedProcedure, Dictionary<string, object> parameters);
        Dictionary<string, object> ExecuteReader(string storedProcedure);

        bool ExecuteNonQuery(string storedProcedure, Dictionary<string, object> parameters);
        bool ExecuteNonQuery(string storedProcedure);

        object ExecuteScalar(string storedProcedure, Dictionary<string, object> parameters);
        object ExecuteScalar(string storedProcedure);
    }
}
