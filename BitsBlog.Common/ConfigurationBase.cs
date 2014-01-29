using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitsBlog.Common
{
    public abstract class ConfigurationBase
    {
        #region Properties & Fields

        protected static volatile ConfigurationBase instance;
        protected static object syncRoot = new Object();
        public static ConfigurationBase Instance { get; private set; }

        #endregion

        #region Constructors

        protected ConfigurationBase()
        {
            //set all app setting related properties
            SetAllProperties();
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Set configuration properties.
        /// </summary>
        protected abstract void SetAllProperties();

        #endregion
    }
}
