﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Data.Entity.SqlServer.FunctionalTests;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.Data.Entity.FunctionalTests
{
    public class GearsOfWarFromSqlQuerySqlServerTest : GearsOfWarFromSqlQueryTestBase<MySqlTestStore, GearsOfWarQuerySqlServerFixture>
    {
        public override void From_sql_queryable_simple_columns_out_of_order()
        {
            base.From_sql_queryable_simple_columns_out_of_order();

            Assert.Equal(
                @"SELECT ""Id"", ""Name"", ""AmmunitionType"", ""OwnerFullName"", ""SynergyWithId"" FROM ""Weapon"" ORDER BY ""Name""",
                Sql);
        }

        public GearsOfWarFromSqlQuerySqlServerTest(GearsOfWarQuerySqlServerFixture fixture, ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
        }

        protected override void ClearLog() => TestSqlLoggerFactory.Reset();

        private static string Sql => TestSqlLoggerFactory.Sql;
    }
}
