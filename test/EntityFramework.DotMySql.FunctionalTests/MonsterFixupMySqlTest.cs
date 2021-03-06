// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using EntityFramework.DotMySql.Extensions;
using Microsoft.Data.Entity.ChangeTracking.Internal;
using Microsoft.Data.Entity.FunctionalTests;
using Microsoft.Data.Entity.FunctionalTests.TestModels;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class MonsterFixupMySqlTest : MonsterFixupTestBase
    {
        private static readonly HashSet<string> _createdDatabases = new HashSet<string>();

        private static readonly ConcurrentDictionary<string, object> _creationLocks
            = new ConcurrentDictionary<string, object>();

        protected override IServiceProvider CreateServiceProvider(bool throwingStateManager = false)
        {
            var serviceCollection = new ServiceCollection()
                .AddEntityFramework()
                .AddMySql()
                .ServiceCollection();

            if (throwingStateManager)
            {
                serviceCollection.AddScoped<IStateManager, ThrowingMonsterStateManager>();
            }

            return serviceCollection.BuildServiceProvider();
        }

        protected override DbContextOptions CreateOptions(string databaseName)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseMySql(CreateConnectionString(databaseName));

            return optionsBuilder.Options;
        }

        private static string CreateConnectionString(string name)
        {
            var connStrBuilder = new MySqlConnectionStringBuilder
            {
                Database = name
            };
            return connStrBuilder.ApplyConfiguration().ConnectionString;
        }

        protected override void CreateAndSeedDatabase(string databaseName, Func<MonsterContext> createContext)
        {
            var creationLock = _creationLocks.GetOrAdd(databaseName, n => new object());
            lock (creationLock)
            {
                if (!_createdDatabases.Contains(databaseName))
                {
                    using (var context = createContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        context.SeedUsingFKs();
                    }

                    _createdDatabases.Add(databaseName);
                }
            }
        }

        public override void OnModelCreating<TMessage, TProductPhoto, TProductReview>(ModelBuilder builder)
        {
            base.OnModelCreating<TMessage, TProductPhoto, TProductReview>(builder);

            /*builder.Entity<TMessage>().Property(e => e.MessageId).UseMySqlIdentityColumn();
            builder.Entity<TProductPhoto>().Property(e => e.PhotoId).UseMySqlIdentityColumn();
            builder.Entity<TProductReview>().Property(e => e.ReviewId).UseMySqlIdentityColumn();*/
            builder.Entity<TMessage>().HasKey(e => e.MessageId);
            builder.Entity<TProductReview>().HasKey(e => e.ReviewId);
            builder.Entity<TProductPhoto>().HasKey(e => e.PhotoId);
        }
    }
}
