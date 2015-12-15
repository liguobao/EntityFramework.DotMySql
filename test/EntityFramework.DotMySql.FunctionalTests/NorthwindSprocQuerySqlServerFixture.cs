// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Data.Entity.ChangeTracking;
using Microsoft.Data.Entity.FunctionalTests;
using Microsoft.Data.Entity.FunctionalTests.TestModels.Northwind;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.SqlServer.FunctionalTests.TestModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class NorthwindSprocQuerySqlServerFixture : NorthwindSprocQueryRelationalFixture, IDisposable
    {
        /*private readonly IServiceProvider _serviceProvider;
        private readonly DbContextOptions _options;
        private readonly MySqlTestStore _testStore;

        public NorthwindSprocQuerySqlServerFixture()
        {
            _testStore = MySqlNorthwindContext.GetSharedStore();

            _serviceProvider = new ServiceCollection()
                .AddEntityFramework()
                .AddMySql()
                .ServiceCollection()
                .AddSingleton(TestMySqlModelSource.GetFactory(OnModelCreating))
                .AddSingleton<ILoggerFactory>(new TestSqlLoggerFactory())
                .BuildServiceProvider();

            var optionsBuilder = new DbContextOptionsBuilder();

            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseMySql(_testStore.Connection.ConnectionString);

            _options = optionsBuilder.Options;

            _serviceProvider.GetRequiredService<ILoggerFactory>();
        }

        public override NorthwindContext CreateContext()
        {
            var context = new MySqlNorthwindContext(_serviceProvider, _options);

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return context;
        }

        public void Dispose() => _testStore.Dispose();*/
        public override NorthwindContext CreateContext()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
