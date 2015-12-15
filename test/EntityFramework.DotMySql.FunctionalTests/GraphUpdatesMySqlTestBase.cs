// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Data.Entity.FunctionalTests;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public abstract class GraphUpdatesMySqlTestBase<TFixture> : GraphUpdatesTestBase<MySqlTestStore, TFixture>
        where TFixture : GraphUpdatesMySqlTestBase<TFixture>.GraphUpdatesMySqlFixtureBase, new()
    {
        protected GraphUpdatesMySqlTestBase(TFixture fixture)
            : base(fixture)
        {
        }

        public abstract class GraphUpdatesMySqlFixtureBase : GraphUpdatesFixtureBase
        {
            private readonly IServiceProvider _serviceProvider;

            protected GraphUpdatesMySqlFixtureBase()
            {
                _serviceProvider = new ServiceCollection()
                    .AddEntityFramework()
                    .AddMySql()
                    .ServiceCollection()
                    .AddSingleton(TestMySqlModelSource.GetFactory(OnModelCreating))
                    .BuildServiceProvider();
            }

            protected abstract string DatabaseName { get; }

            public override MySqlTestStore CreateTestStore()
            {
                return MySqlTestStore.GetOrCreateShared(DatabaseName, () =>
                    {
                        var optionsBuilder = new DbContextOptionsBuilder();
                        optionsBuilder.UseMySql(MySqlTestStore.CreateConnectionString(DatabaseName));

                        using (var context = new GraphUpdatesContext(_serviceProvider, optionsBuilder.Options))
                        {
                            context.Database.EnsureDeleted();
                            if (context.Database.EnsureCreated())
                            {
                                Seed(context);
                            }
                        }
                    });
            }

            public override DbContext CreateContext(MySqlTestStore testStore)
            {
                var optionsBuilder = new DbContextOptionsBuilder();
                optionsBuilder.UseMySql(testStore.Connection);

                var context = new GraphUpdatesContext(_serviceProvider, optionsBuilder.Options);
                context.Database.UseTransaction(testStore.Transaction);
                return context;
            }
        }
    }
}
