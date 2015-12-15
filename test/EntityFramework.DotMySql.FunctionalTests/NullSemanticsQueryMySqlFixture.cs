// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Data.Entity.ChangeTracking;
using Microsoft.Data.Entity.FunctionalTests;
using Microsoft.Data.Entity.FunctionalTests.TestModels.NullSemantics;
using Microsoft.Data.Entity.FunctionalTests.TestModels.NullSemanticsModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class NullSemanticsQueryMySqlFixture : NullSemanticsQueryRelationalFixture<MySqlTestStore>
    {
        public static readonly string DatabaseName = "NullSemanticsQueryTest";

        private readonly IServiceProvider _serviceProvider;

        private readonly string _connectionString = MySqlTestStore.CreateConnectionString(DatabaseName);

        public NullSemanticsQueryMySqlFixture()
        {
            _serviceProvider = new ServiceCollection()
                .AddEntityFramework()
                .AddMySql()
                .ServiceCollection()
                .AddSingleton(TestMySqlModelSource.GetFactory(OnModelCreating))
                .AddSingleton<ILoggerFactory>(new TestSqlLoggerFactory())
                .BuildServiceProvider();
        }

        public override MySqlTestStore CreateTestStore()
        {
            return MySqlTestStore.GetOrCreateShared(DatabaseName, () =>
            {
                var optionsBuilder = new DbContextOptionsBuilder();
                optionsBuilder.UseMySql(_connectionString);

                using (var context = new NullSemanticsContext(_serviceProvider, optionsBuilder.Options))
                {
                    // TODO: Delete DB if model changed

                    if (context.Database.EnsureCreated())
                    {
                        NullSemanticsModelInitializer.Seed(context);
                    }

                    TestSqlLoggerFactory.SqlStatements.Clear();
                }
            });
        }

        public override NullSemanticsContext CreateContext(MySqlTestStore testStore, bool useRelationalNulls)
        {
            var optionsBuilder = new DbContextOptionsBuilder();

            var sqlServerOptions
                = optionsBuilder
                    .EnableSensitiveDataLogging()
                    .UseMySql(testStore.Connection);

            if (useRelationalNulls)
            {
                sqlServerOptions.UseRelationalNulls();
            }

            var context = new NullSemanticsContext(_serviceProvider, optionsBuilder.Options);

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            context.Database.UseTransaction(testStore.Transaction);

            return context;
        }
    }
}