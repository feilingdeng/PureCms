using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Core.Data
{
    public class DbSession : PetaPoco.Database
    {
        public DbSession(IDbConnection connection) : base(connection)
        {
        }

        public DbSession(string connectionString, string providerName) : base(connectionString, providerName)
        {
        }

        public DbSession(string connectionString, DbProviderFactory provider) : base(connectionString, provider)
        {
        }

        public DbSession(string connectionStringName) : base(connectionStringName)
        { }

        public override void OnException(Exception e)
        {
            base.AbortTransaction();
            throw new PureCmsException(e.Message, e.InnerException);
            //base.OnException(e);
        }
    }
}
