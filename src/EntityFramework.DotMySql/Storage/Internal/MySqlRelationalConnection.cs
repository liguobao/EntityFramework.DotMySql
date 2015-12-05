// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Utilities;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

// ReSharper disable once CheckNamespace
namespace Microsoft.Data.Entity.Storage.Internal
{
    public class MySqlRelationalConnection : RelationalConnection
    {
        public MySqlRelationalConnection(
            [NotNull] IDbContextOptions options,
            // ReSharper disable once SuggestBaseTypeForParameter
            [NotNull] ILogger<MySqlConnection> logger)
            : base(options, logger)
        {
        }

        private MySqlRelationalConnection(
            [NotNull] IDbContextOptions options, [NotNull] ILogger logger)
            : base(options, logger)
        {
        }

        // TODO: Consider using DbProviderFactory to create connection instance
        // Issue #774
        protected override DbConnection CreateDbConnection() => new MySqlConnection(ConnectionString);

        public MySqlRelationalConnection CreateMasterConnection()
        {
            var csb = new MySqlConnectionStringBuilder(ConnectionString) {
                Database = "mysql",
                Pooling = false
            };
            
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseMySql(csb.GetConnectionString(true));
            return new MySqlRelationalConnection(optionsBuilder.Options, Logger);
        }
    }
}
