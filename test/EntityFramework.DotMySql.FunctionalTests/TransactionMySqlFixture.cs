// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Data.Common;
using Microsoft.Data.Entity.FunctionalTests;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class TransactionMySqlFixture : TransactionFixtureBase<MySqlTestStore>
    {
        private readonly IServiceProvider _serviceProvider;

        public TransactionMySqlFixture()
        {
            _serviceProvider = new ServiceCollection()
                .AddEntityFramework()
                .AddMySql()
                .ServiceCollection()
                .AddSingleton(TestMySqlModelSource.GetFactory(OnModelCreating))
                .BuildServiceProvider();
        }

        public override MySqlTestStore CreateTestStore()
        {
            var db = MySqlTestStore.CreateScratch();

            using (var command = db.Connection.CreateCommand())
            {
                command.CommandText = "ALTER DATABASE [" + db.Connection.Database + "] SET ALLOW_SNAPSHOT_ISOLATION ON";
                command.ExecuteNonQuery();
            }

            using (var command = db.Connection.CreateCommand())
            {
                command.CommandText = "ALTER DATABASE [" + db.Connection.Database + "] SET READ_COMMITTED_SNAPSHOT ON";
                command.ExecuteNonQuery();
            }

            using (var context = CreateContext(db))
            {
                Seed(context);
            }

            return db;
        }

        public override DbContext CreateContext(MySqlTestStore testStore)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseMySql(testStore.Connection.ConnectionString);

            return new DbContext(_serviceProvider, optionsBuilder.Options);
        }

        public override DbContext CreateContext(DbConnection connection)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseMySql(connection);

            return new DbContext(_serviceProvider, optionsBuilder.Options);
        }
    }
}
