using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Configuration.Base;

namespace BitsBlog.Configuration
{
    public sealed class CMSAppSystemConfiguration : ConfigurationBase
    {
        public CMSAppSystemConfiguration() : base(){}

        /// <summary>
        /// Gets configuraiton data from appSettings.config
        /// </summary>
        protected override void Init()
        {
            //TODO implement getting configurations items from web config
            throw new NotImplementedException();
        }
    }
}
