using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Utilities;

namespace EntityFramework.DotMySql.Extensions
{
    public static class MySqlModelBuilderExtension
    {
        public static ModelBuilder ForMySqlUseIdentityColumns(
            [NotNull] this ModelBuilder modelBuilder)
        {
            Check.NotNull(modelBuilder, nameof(modelBuilder));

            var property = modelBuilder.Model;

            /*property.MySql().ValueGenerationStrategy = SqlServerValueGenerationStrategy.IdentityColumn;
            property.SqlServer().HiLoSequenceName = null;
            property.SqlServer().HiLoSequenceSchema = null;*/

            return modelBuilder;
        }
    }
}
