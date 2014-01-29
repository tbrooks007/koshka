using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Data.Repositories.Bases;
using BitsBlog.Data.Repositories.Interfaces;
using BitsBlog.Data.Factories;
using BitsBlog.Data.Configurations;
using System.Data.Common;
using BitsBlog.Data.Interfaces;
using BitsBlog.Common.Enums;
using BitsBlog.Common;
using BitsBlog.Core.Interfaces;

namespace BitsBlog.Data.Repositories.Factories
{
    public class RepsitoryFactory
    {
        private static IDatabaseAccessAgent agent = null;
        
        /// <summary>
        /// Gets the AccessAgent, if the agent is null a new one is created.
        /// </summary>
        private static IDatabaseAccessAgent AccessAgent
        {
            get
            {
                if (agent == null)
                {
                    //get the connection string from the configuration, get the proper data access agent
                    //based on the connection string's provider
                    if (!string.IsNullOrWhiteSpace(DatabaseConfiguration.Instance.ConnectionString))
                        agent = DatabaseAccessAgentFactory.CreateAccessAgent(DatabaseConfiguration.Instance.ConnectionString);

                    //TODO log if the connection string isn't there...means something is wrong
                }

                return agent;
            }
        }

        public RepsitoryFactory()
        {
            //RepsitoryFactory rf = new RepsitoryFactory();
            //var blah = rf.CreateRepository<IUserRepository>();
            //var blah2 = rf.CreateRepository<IBlogRepository>();
            //var blah2 = rf.CreateRepository<BlogPostRepository>();
        }

        public T CreateRepository<T>() where T : class
        {
            return (T)CreateRepository<T>(typeof(T));
        }

        private T CreateRepository<T>(Type type) where T : class
        {
            if (type == typeof(IUserRepository))
                 return (T)CreateUserRepository();
            else if (type == typeof(IBlogPostRepository))
                 return (T)CreateBlogPostRepository();
             else if (type == typeof(IBlogRepository))
                 return (T)CreateBlogRepository();
             else
                return null;
        }

        private IBlogPostRepository CreateBlogPostRepository()
        {
            return new BlogPostRepository(AccessAgent);
        }

        private IBlogRepository CreateBlogRepository()
        {
            return new BlogRepository(AccessAgent);
        }

        private IUserRepository CreateUserRepository()
        {
            return new UserRepository (AccessAgent);
        }
    }
}
