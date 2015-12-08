// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Data.Entity.FunctionalTests.TestModels.GearsOfWarModel;
using Microsoft.Data.Entity.SqlServer.FunctionalTests;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.Data.Entity.FunctionalTests
{
    public class GearsOfWarFromSqlQueryMySqlTest : GearsOfWarFromSqlQueryTestBase<MySqlTestStore, GearsOfWarQueryMySqlFixture>
    {
        [Fact]
        public virtual void From_sql_queryable_simple_columns_out_of_order()
        {
            using (var context = CreateContext())
            {
                var actual = context.Set<Weapon>()
                    .FromSql(@"SELECT `Id`, `Name`, `AmmunitionType`, `OwnerFullName`, `SynergyWithId` FROM `Weapon` ORDER BY `Name`")
                    .ToArray();

                Assert.Equal(10, actual.Length);

                var first = actual.First();

                Assert.Equal(AmmunitionType.Shell, first.AmmunitionType);
                Assert.Equal("Baird's Gnasher", first.Name);
            }
        }

        public GearsOfWarFromSqlQueryMySqlTest(GearsOfWarQueryMySqlFixture fixture, ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
        }

        protected override void ClearLog() => TestSqlLoggerFactory.Reset();

        private static string Sql => TestSqlLoggerFactory.Sql;
    }
}
