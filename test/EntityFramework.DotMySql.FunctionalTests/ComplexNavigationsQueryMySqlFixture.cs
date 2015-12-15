// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Data.Entity.ChangeTracking;
using Microsoft.Data.Entity.FunctionalTests;
using Microsoft.Data.Entity.FunctionalTests.TestModels.ComplexNavigationsModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class ComplexNavigationsQueryMySqlFixture 
        : ComplexNavigationsQueryRelationalFixture<MySqlTestStore>
    {
        public static readonly string DatabaseName = "ComplexNavigations";

        private readonly IServiceProvider _serviceProvider;

        private readonly string _connectionString 
            = MySqlTestStore.CreateConnectionString(DatabaseName);

        public ComplexNavigationsQueryMySqlFixture()
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

                    using (var context = new ComplexNavigationsContext(_serviceProvider, optionsBuilder.Options))
                    {
                        // TODO: Delete DB if model changed
                        context.Database.EnsureDeleted();

                        if (context.Database.EnsureCreated())
                        {
                            ComplexNavigationsModelInitializer.Seed(context);
                        }

                        TestSqlLoggerFactory.SqlStatements.Clear();
                    }
                });
        }

        public override ComplexNavigationsContext CreateContext(MySqlTestStore testStore)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseMySql(testStore.Connection);

            var context = new ComplexNavigationsContext(_serviceProvider, optionsBuilder.Options);

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            context.Database.UseTransaction(testStore.Transaction);

            return context;
        }
    }
}
