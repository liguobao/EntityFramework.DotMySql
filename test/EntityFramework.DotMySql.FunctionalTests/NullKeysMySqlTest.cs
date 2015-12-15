// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Data.Entity.FunctionalTests;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class NullKeysMySqlTest : NullKeysTestBase<NullKeysMySqlTest.NullKeysSqlServerFixture>
    {
        public NullKeysMySqlTest(NullKeysSqlServerFixture fixture)
            : base(fixture)
        {
        }

        public class NullKeysSqlServerFixture : NullKeysFixtureBase
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly DbContextOptions _options;

            public NullKeysSqlServerFixture()
            {
                _serviceProvider = new ServiceCollection()
                    .AddEntityFramework()
                    .AddMySql()
                    .ServiceCollection()
                    .AddSingleton(TestMySqlModelSource.GetFactory(OnModelCreating))
                    .BuildServiceProvider();

                var optionsBuilder = new DbContextOptionsBuilder();
                optionsBuilder.UseMySql(MySqlTestStore.CreateConnectionString("StringsContext"));
                _options = optionsBuilder.Options;

                EnsureCreated();
            }

            public override DbContext CreateContext()
            {
                return new DbContext(_serviceProvider, _options);
            }
        }
    }
}
