using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Configuration.Interfaces;

namespace BitsBlog.Configuration.Base
{
    public abstract class ConfigurationBase : ISystemConfiguration
    {
        #region Properties

        public Enums.PrimaryDataSourceType PrimaryDataSourceType { get; private set; }

        public Enums.SupportedDatabaseSystem DatabaseSystem { get; private set; }

        public string RestfulServiceURL { get; private set; }

        #endregion

        public ConfigurationBase()
        {
            Init();
        }

        #region Methods

        /// <summary>
        /// Gets configuraiton data from web.config or app.config
        /// </summary>
        protected abstract void Init();

        #endregion
    }
}
