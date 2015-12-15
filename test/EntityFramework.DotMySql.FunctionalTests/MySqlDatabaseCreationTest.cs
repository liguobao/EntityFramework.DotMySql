// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Storage;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class MySqlDatabaseCreationTest
    {
        [Fact]
        public async Task Exists_returns_false_when_database_doesnt_exist()
        {
            await Exists_returns_false_when_database_doesnt_exist_test(async: false);
        }

        [Fact]
        public async Task ExistsAsync_returns_false_when_database_doesnt_exist()
        {
            await Exists_returns_false_when_database_doesnt_exist_test(async: true);
        }

        private static async Task Exists_returns_false_when_database_doesnt_exist_test(bool async)
        {
            using (var testDatabase = await MySqlTestStore.CreateScratchAsync(createDatabase: false))
            {
                using (var context = new BloggingContext(testDatabase))
                {
                    var creator = context.GetService<IRelationalDatabaseCreator>();

                    Assert.False(async ? await creator.ExistsAsync() : creator.Exists());

                    Assert.Equal(ConnectionState.Closed, context.Database.GetDbConnection().State);
                }
            }
        }

        [Fact]
        public async Task Exists_returns_true_when_database_exists()
        {
            await Exists_returns_true_when_database_exists_test(async: false);
        }

        [Fact]
        public async Task ExistsAsync_returns_true_when_database_exists()
        {
            await Exists_returns_true_when_database_exists_test(async: true);
        }

        private static async Task Exists_returns_true_when_database_exists_test(bool async)
        {
            using (var testDatabase = await MySqlTestStore.CreateScratchAsync(createDatabase: true))
            {
                using (var context = new BloggingContext(testDatabase))
                {
                    var creator = context.GetService<IRelationalDatabaseCreator>();

                    Assert.True(async ? await creator.ExistsAsync() : creator.Exists());

                    Assert.Equal(ConnectionState.Closed, context.Database.GetDbConnection().State);

                    await creator.DeleteAsync();
                }

                
            }
        }

        [Fact]
        public async Task EnsureDeleted_will_delete_database()
        {
            await EnsureDeleted_will_delete_database_test(async: false, openConnection: false);
        }

        [Fact]
        public async Task EnsureDeletedAsync_will_delete_database()
        {
            await EnsureDeleted_will_delete_database_test(async: true, openConnection: false);
        }

        [Fact]
        public async Task EnsureDeleted_will_delete_database_with_opened_connections()
        {
            await EnsureDeleted_will_delete_database_test(async: false, openConnection: true);
        }

        [Fact]
        public async Task EnsureDeletedAsync_will_delete_database_with_opened_connections()
        {
            await EnsureDeleted_will_delete_database_test(async: true, openConnection: true);
        }

        private static async Task EnsureDeleted_will_delete_database_test(bool async, bool openConnection)
        {
            using (var testDatabase = await MySqlTestStore.CreateScratchAsync(createDatabase: true))
            {
                if (!openConnection)
                {
                    testDatabase.Connection.Close();
                }

                using (var context = new BloggingContext(testDatabase))
                {
                    var creator = context.GetService<IRelationalDatabaseCreator>();

                    Assert.True(async ? await creator.ExistsAsync() : creator.Exists());

                    if (async)
                    {
                        Assert.True(await context.Database.EnsureDeletedAsync());
                    }
                    else
                    {
                        Assert.True(context.Database.EnsureDeleted());
                    }

                    Assert.Equal(ConnectionState.Closed, context.Database.GetDbConnection().State);

                    Assert.False(async ? await creator.ExistsAsync() : creator.Exists());

                    Assert.Equal(ConnectionState.Closed, context.Database.GetDbConnection().State);
                }
            }
        }

        [Fact]
        public async Task EnsuredDeleted_noop_when_database_doesnt_exist()
        {
            await EnsuredDeleted_noop_when_database_doesnt_exist_test(async: false);
        }

        [Fact]
        public async Task EnsuredDeletedAsync_noop_when_database_doesnt_exist()
        {
            await EnsuredDeleted_noop_when_database_doesnt_exist_test(async: true);
        }

        private static async Task EnsuredDeleted_noop_when_database_doesnt_exist_test(bool async)
        {
            using (var testDatabase = await MySqlTestStore.CreateScratchAsync(createDatabase: false))
            {
                using (var context = new BloggingContext(testDatabase))
                {
                    var creator = context.GetService<IRelationalDatabaseCreator>();

                    Assert.False(async ? await creator.ExistsAsync() : creator.Exists());

                    if (async)
                    {
                        Assert.False(await creator.EnsureDeletedAsync());
                    }
                    else
                    {
                        Assert.False(creator.EnsureDeleted());
                    }

                    Assert.Equal(ConnectionState.Closed, context.Database.GetDbConnection().State);

                    Assert.False(async ? await creator.ExistsAsync() : creator.Exists());

                    Assert.Equal(ConnectionState.Closed, context.Database.GetDbConnection().State);
                }
            }
        }

        [Fact]
        public async Task EnsureCreated_can_create_schema_in_existing_database()
        {
            await EnsureCreated_can_create_schema_in_existing_database_test(async: false);
        }

        [Fact]
        public async Task EnsureCreatedAsync_can_create_schema_in_existing_database()
        {
            await EnsureCreated_can_create_schema_in_existing_database_test(async: true);
        }

        private static async Task EnsureCreated_can_create_schema_in_existing_database_test(bool async)
        {
            using (var testDatabase = await MySqlTestStore.CreateScratchAsync())
            {
                await RunDatabaseCreationTest(testDatabase, async);
            }
        }

        [Fact]
        public async Task EnsureCreated_can_create_physical_database_and_schema()
        {
            await EnsureCreated_can_create_physical_database_and_schema_test(async: false);
        }

        [Fact]
        public async Task EnsureCreatedAsync_can_create_physical_database_and_schema()
        {
            await EnsureCreated_can_create_physical_database_and_schema_test(async: true);
        }

        private static async Task EnsureCreated_can_create_physical_database_and_schema_test(bool async)
        {
            using (var testDatabase = await MySqlTestStore.CreateScratchAsync(createDatabase: false))
            {
                await RunDatabaseCreationTest(testDatabase, async);
            }
        }

        private static async Task RunDatabaseCreationTest(MySqlTestStore testStore, bool async)
        {
            using (var context = new BloggingContext(testStore))
            {
                var creator = context.GetService<IRelationalDatabaseCreator>();

                Assert.Equal(ConnectionState.Closed, context.Database.GetDbConnection().State);

                if (async)
                {
                    Assert.True(await creator.EnsureCreatedAsync());
                }
                else
                {
                    Assert.True(creator.EnsureCreated());
                }

                Assert.Equal(ConnectionState.Closed, context.Database.GetDbConnection().State);

                if (testStore.Connection.State != ConnectionState.Open)
                {
                    await testStore.Connection.OpenAsync();
                }

                var tables = await testStore.QueryAsync<string>("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='" + testStore.Connection.Database + "'");
                Assert.Equal(1, tables.Count());
                Assert.Equal("blog", tables.Single());

                var columns = (await testStore.QueryAsync<string>(
                    "SELECT CONCAT(TABLE_NAME, '.', COLUMN_NAME, ' (', COLUMN_TYPE, ')') FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_SCHEMA='" + testStore.Connection.Database + "' AND TABLE_NAME = 'Blog' ORDER BY TABLE_NAME, COLUMN_NAME")).ToArray();
                Assert.Equal(19, columns.Length);

                Assert.Equal(
                    new[]
                    {
                        "blog.AndChew (varbinary(8000))",
                        "blog.AndRow (timestamp)",
                        "blog.Cheese (longtext)",
                        "blog.CupOfChar (int(11))",
                        "blog.ErMilan (int(11))",
                        "blog.Fuse (smallint(6))",
                        "blog.George (bit(1))",
                        "blog.Key1 (varchar(450))",
                        "blog.Key2 (varbinary(450))",
                        "blog.NotFigTime (datetime)",
                        "blog.NotToEat (tinyint(4))",
                        "blog.On (float)",
                        "blog.OrNothing (double)",
                        "blog.OrULong (int(11))",
                        "blog.OrUShort (double(20,0))",
                        "blog.OrUSkint (bigint(20))",
                        "blog.TheGu (char(38))",
                        "blog.ToEat (tinyint(3) unsigned)",
                        "blog.WayRound (bigint(20))"
                    },
                    columns);
            }
        }

        [Fact]
        public async Task EnsuredCreated_is_noop_when_database_exists_and_has_schema()
        {
            await EnsuredCreated_is_noop_when_database_exists_and_has_schema_test(async: false);
        }

        [Fact]
        public async Task EnsuredCreatedAsync_is_noop_when_database_exists_and_has_schema()
        {
            await EnsuredCreated_is_noop_when_database_exists_and_has_schema_test(async: true);
        }

        private static async Task EnsuredCreated_is_noop_when_database_exists_and_has_schema_test(bool async)
        {
            using (var testDatabase = await MySqlTestStore.CreateScratchAsync(createDatabase: false))
            {
                using (var context = new BloggingContext(testDatabase))
                {
                    context.Database.EnsureCreated();

                    if (async)
                    {
                        Assert.False(await context.Database.EnsureCreatedAsync());
                    }
                    else
                    {
                        Assert.False(context.Database.EnsureCreated());
                    }

                    Assert.Equal(ConnectionState.Closed, context.Database.GetDbConnection().State);

                    context.Database.EnsureDeleted();
                }
            }
        }

        private static IServiceProvider CreateServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddEntityFramework()
                .AddMySql();

            return serviceCollection.BuildServiceProvider();
        }

        private class BloggingContext : DbContext
        {
            private readonly MySqlTestStore _testStore;

            public BloggingContext(MySqlTestStore testStore)
                : base(CreateServiceProvider())
            {
                _testStore = testStore;
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseMySql(_testStore.Connection.ConnectionString);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Blog>(b =>
                    {
                        b.HasKey(e => new { e.Key1, e.Key2 });
                        b.Property(e => e.AndRow).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
                    });
            }

            public DbSet<Blog> Blogs { get; set; }
        }

        public class Blog
        {
            public string Key1 { get; set; }
            public byte[] Key2 { get; set; }
            public string Cheese { get; set; }
            public int ErMilan { get; set; }
            public bool George { get; set; }
            public Guid TheGu { get; set; }
            public DateTime NotFigTime { get; set; }
            public byte ToEat { get; set; }
            public char CupOfChar { get; set; }
            public double OrNothing { get; set; }
            public short Fuse { get; set; }
            public long WayRound { get; set; }
            public sbyte NotToEat { get; set; }
            public float On { get; set; }
            public ushort OrULong { get; set; }
            public uint OrUSkint { get; set; }
            public ulong OrUShort { get; set; }
            public byte[] AndChew { get; set; }
            public byte[] AndRow { get; set; }
        }
    }
}
