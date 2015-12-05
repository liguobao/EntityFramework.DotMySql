// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.Data.Entity.FunctionalTests;
using Microsoft.Data.Entity.FunctionalTests.TestModels.Northwind;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.SqlServer.FunctionalTests.TestModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class NorthwindQueryMySqlFixture : NorthwindQueryRelationalFixture, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DbContextOptions _options;

        private readonly MySqlTestStore _testStore = MySqlNorthwindContext.GetSharedStore();
        private readonly TestSqlLoggerFactory _testSqlLoggerFactory = new TestSqlLoggerFactory();

        public NorthwindQueryMySqlFixture()
        {
            _serviceProvider
                = new ServiceCollection()
                    .AddEntityFramework()
                    .AddMySql()
                    .ServiceCollection()
                    .AddSingleton(TestMySqlModelSource.GetFactory(OnModelCreating))
                    .AddSingleton<ILoggerFactory>(_testSqlLoggerFactory)
                    .BuildServiceProvider();

            _options = BuildOptions();
        }

        protected DbContextOptions BuildOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder();

            var mySqlDbContextOptionsBuilder
                = optionsBuilder
                    .EnableSensitiveDataLogging()
                    .UseMySql(_testStore.Connection.ConnectionString);

            ConfigureOptions(mySqlDbContextOptionsBuilder);

            mySqlDbContextOptionsBuilder.ApplyConfiguration();

            return optionsBuilder.Options;
        }

        protected virtual void ConfigureOptions(MySqlDbContextOptionsBuilder MySqlDbContextOptionsBuilder)
        {
        }

        public override NorthwindContext CreateContext() 
            => new MySqlNorthwindContext(_serviceProvider, _options);

        public void Dispose() => _testStore.Dispose();

        public override CancellationToken CancelQuery() => _testSqlLoggerFactory.CancelQuery();
    }
}
