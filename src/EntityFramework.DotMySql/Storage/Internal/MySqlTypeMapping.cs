using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Storage;
using MySql.Data.MySqlClient;


namespace Microsoft.Data.Entity.Storage.Internal
{
    public class MySqlTypeMapping : RelationalTypeMapping
    {
        public new MySqlDbType? StoreType { get; }

        internal MySqlTypeMapping([NotNull] string defaultTypeName, [NotNull] Type clrType, MySqlDbType storeType)
            : base(defaultTypeName, clrType)
        {
            StoreType = storeType;
        }

        internal MySqlTypeMapping([NotNull] string defaultTypeName, [NotNull] Type clrType)
            : base(defaultTypeName, clrType)
        { }

        protected override void ConfigureParameter([NotNull] DbParameter parameter)
        {
            if (StoreType.HasValue)
            {
                ((MySqlParameter) parameter).MySqlDbType = StoreType.Value;
            }
        }
    }
}
