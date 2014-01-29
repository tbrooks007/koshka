using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Configuration.Base;

namespace BitsBlog.Configuration
{
    public sealed class ClientAppSystemConfiguration : ConfigurationBase
    {
        /// <summary>
        /// Used by read only apps to identify the blog they want data for.
        /// </summary>
        public Guid PublicUniqueId { get; private set; }

        public ClientAppSystemConfiguration() : base(){}


        /// <summary>
        /// Gets configuraiton data from appSettings.config
        /// </summary>
        protected override void Init()
        {
            //TODO implement getting configurations items from appSettings.config

            throw new NotImplementedException();
        }
    }
}
