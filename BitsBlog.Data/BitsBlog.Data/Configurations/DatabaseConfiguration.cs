using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Common;
using System.Configuration;

namespace BitsBlog.Data.Configurations
{
    internal sealed class DatabaseConfiguration : ConfigurationBase
    {
        new protected static volatile DatabaseConfiguration instance;
        new protected static object syncRoot = new Object();
        public string ConnectionString { get; private set; }

        public DatabaseConfiguration()
        {
            SetAllProperties();
        }

        new public static DatabaseConfiguration Instance
        {
            // This approach allows us to have one istance for all threads.
            // This also allows us to only create the instance when this property is first accessed.
            get
            {
                if (instance == null)
                {
                    // allow only one thread to create the instance
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DatabaseConfiguration();
                    }
                }

                return instance;
            }
        }

        protected override void SetAllProperties()
        {
            try
            {
                this.ConnectionString = ConfigurationManager.ConnectionStrings["BitsBlogDB"].ConnectionString;
            }
            catch (Exception e)
            {
                //TODO log error
            }
        }
    }
}
