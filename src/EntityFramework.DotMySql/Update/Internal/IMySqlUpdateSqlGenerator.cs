using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Microsoft.Data.Entity.Update.Internal
{
    public interface IMySqlUpdateSqlGenerator : IUpdateSqlGenerator
    {
        ResultSetMapping AppendBulkInsertOperation(
            [NotNull] StringBuilder commandStringBuilder,
            [NotNull] IReadOnlyList<ModificationCommand> modificationCommands,
            int commandPosition);
    }
   
}
