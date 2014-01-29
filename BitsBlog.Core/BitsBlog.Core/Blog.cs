using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Core.Interfaces;

namespace BitsBlog.Core
{
    public sealed class Blog : IBlog
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string UserUUID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public IList<Structs.Author> Authors { get; set; }
        public Guid BlogUUID { get; set; } //TODO use randomly generated key (may not encrypt it) - use RNGCryptoServiceProvider
        public bool AllowReaderComments { get; set; }
        public bool MustApproveReaderComments { get; set; }
        public bool MustBeUserToMakeComments { get; set; }
        public bool RequireNameField { get; set; }
        public bool RequireEmailField { get; set; }
        public bool RequireCAPTCHA { get; set; }

        #endregion
    }
}
