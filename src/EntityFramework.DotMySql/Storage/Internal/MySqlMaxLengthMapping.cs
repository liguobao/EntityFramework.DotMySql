using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity.Storage;
using JetBrains.Annotations;

namespace EntityFramework.DotMySql.Storage.Internal
{
    public class MySqlMaxLengthMapping : RelationalTypeMapping
    {

        private readonly int _maxSpecificSize;

        public MySqlMaxLengthMapping([NotNull] string defaultTypeName, [NotNull] Type clrType, DbType? storeType = null)
            : base(defaultTypeName, clrType, storeType)
        {

            if (clrType == typeof (char))
            {
                _maxSpecificSize = 256;
            }
            else
            {
                _maxSpecificSize =
                    (storeType == DbType.AnsiString)
                    || (storeType == DbType.AnsiStringFixedLength)
                    || (storeType == DbType.Binary)
                        ? 8000
                        : 4000;
            }
        }

        protected override void ConfigureParameter(DbParameter parameter)
        {
            // For strings and byte arrays, set the max length to 8000 bytes if the data will
            // fit so as to avoid query cache fragmentation by setting lots of differet Size
            // values otherwise always set to -1 (unbounded) to avoid SQL client size inference.

            var length = (parameter.Value as string)?.Length ?? (parameter.Value as byte[])?.Length;

            parameter.Size = (length != null) && (length <= _maxSpecificSize)
                ? _maxSpecificSize
                : -1;
        }
    }
}
