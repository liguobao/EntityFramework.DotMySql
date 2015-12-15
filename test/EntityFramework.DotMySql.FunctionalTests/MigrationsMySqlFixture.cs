// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Data.Entity.FunctionalTests;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class MigrationsMySqlFixture : MigrationsFixtureBase
    {
        private readonly DbContextOptions _options;
        private readonly IServiceProvider _serviceProvider;

        public MigrationsMySqlFixture()
        {
            _serviceProvider = new ServiceCollection()
                .AddEntityFramework()
                .AddMySql()
                .ServiceCollection()
                .BuildServiceProvider();

            var connectionStringBuilder = new MySqlConnectionStringBuilder
            {
                Database = nameof(MigrationsMySqlTest)
            };
            connectionStringBuilder.ApplyConfiguration();

            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseMySql(connectionStringBuilder.GetConnectionString(true));
            _options = optionsBuilder.Options;
        }

        public override MigrationsContext CreateContext() => new MigrationsContext(_serviceProvider, _options);
    }
}
