using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Configuration.Enums;

namespace BitsBlog.Configuration.Interfaces
{
    public interface ISystemConfiguration
    {
        #region Properties

        PrimaryDataSourceType PrimaryDataSourceType { get;}
        SupportedDatabaseSystem DatabaseSystem { get;}
        string RestfulServiceURL { get; }

        //TODO add restful service crediental properites, will need to research
        // what is likley to be needed. Add when we support this.

        #endregion
    }
}
