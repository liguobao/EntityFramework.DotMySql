// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Data.Entity.FunctionalTests;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class RowNumberPagingTest : QueryTestBase<NorthwindRowNumberPagingQueryMySqlFixture>, IDisposable
    {
        public RowNumberPagingTest(NorthwindRowNumberPagingQueryMySqlFixture fixture, ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            //TestSqlLoggerFactory.CaptureOutput(testOutputHelper);
        }

        public void Dispose()
        {
            //Assert for all tests that OFFSET or FETCH is never used
            Assert.DoesNotContain("OFFSET ", Sql);
            Assert.DoesNotContain("FETCH ", Sql);
        }

        public override void Skip()
        {
            base.Skip();

            Assert.Equal(
               @"@__p_0: 5

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
ORDER BY `c`.`CustomerID`
LIMIT 18446744073709551610 OFFSET @__p_0",
               Sql);
        }

        public override void Skip_no_orderby()
        {
            base.Skip_no_orderby();

            Assert.EndsWith(
                @"@__p_0: 5

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
LIMIT 18446744073709551610 OFFSET @__p_0",
                Sql);
        }

        public override void Skip_Take()
        {
            base.Skip_Take();

            Assert.Equal(
                @"@__p_1: 10
@__p_0: 5

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
ORDER BY `c`.`ContactName`
LIMIT @__p_1 OFFSET @__p_0",
                Sql);
        }

        public override void Join_Customers_Orders_Skip_Take()
        {
            base.Join_Customers_Orders_Skip_Take();
            Assert.Equal(
                @"@__p_1: 5
@__p_0: 10

SELECT `c`.`ContactName`, `o`.`OrderID`
FROM `Customers` AS `c`
INNER JOIN `Orders` AS `o` ON `c`.`CustomerID` = `o`.`CustomerID`
ORDER BY `o`.`OrderID`
LIMIT @__p_1 OFFSET @__p_0",
                Sql);
        }

        public override void Join_Customers_Orders_Projection_With_String_Concat_Skip_Take()
        {
            base.Join_Customers_Orders_Projection_With_String_Concat_Skip_Take();
            Assert.Equal(
                @"@__p_1: 5
@__p_0: 10

SELECT CONCAT(CONCAT(`c`.`ContactName`,' '),`c`.`ContactTitle`), `o`.`OrderID`
FROM `Customers` AS `c`
INNER JOIN `Orders` AS `o` ON `c`.`CustomerID` = `o`.`CustomerID`
ORDER BY `o`.`OrderID`
LIMIT @__p_1 OFFSET @__p_0",
                Sql);
        }

        public override void Take_Skip()
        {
            base.Take_Skip();

            Assert.Equal(@"@__p_0: 10
@__p_1: 5

SELECT `t0`.*
FROM (
    SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
    FROM `Customers` AS `c`
    ORDER BY `c`.`ContactName`
    LIMIT @__p_0
) AS `t0`
ORDER BY `t0`.`ContactName`
LIMIT 18446744073709551610 OFFSET @__p_1",
                Sql);
        }

        public override void Take_Skip_Distinct()
        {
            base.Take_Skip_Distinct();

            Assert.Equal(
                @"@__p_0: 10
@__p_1: 5

SELECT DISTINCT `t1`.*
FROM (
    SELECT `t0`.*
    FROM (
        SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
        FROM `Customers` AS `c`
        ORDER BY `c`.`ContactName`
        LIMIT @__p_0
    ) AS `t0`
    ORDER BY `t0`.`ContactName`
    LIMIT 18446744073709551610 OFFSET @__p_1
) AS `t1`",
                Sql);
        }

        public override void Take_skip_null_coalesce_operator()
        {
            base.Take_skip_null_coalesce_operator();

            Assert.Equal(@"@__p_0: 10
@__p_1: 5

SELECT DISTINCT `t1`.*
FROM (
    SELECT `t0`.*
    FROM (
        SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
        FROM `Customers` AS `c`
        ORDER BY COALESCE(`c`.`Region`, 'ZZ')
        LIMIT @__p_0
    ) AS `t0`
    ORDER BY COALESCE(`t0`.`Region`, 'ZZ')
    LIMIT 18446744073709551610 OFFSET @__p_1
) AS `t1`", Sql);
        }

        public override void Select_take_skip_null_coalesce_operator()
        {
            base.Select_take_skip_null_coalesce_operator();

            Assert.Equal(@"@__p_0: 10
@__p_1: 5

SELECT `t0`.*
FROM (
    SELECT `c`.`CustomerID`, `c`.`CompanyName`, COALESCE(`c`.`Region`, 'ZZ') AS `Coalesce`
    FROM `Customers` AS `c`
    ORDER BY `Coalesce`
    LIMIT @__p_0
) AS `t0`
ORDER BY `Coalesce`
LIMIT 18446744073709551610 OFFSET @__p_1", Sql);
        }

        public override void String_Contains_Literal()
        {
            // skip. This is covered in QueryMySqlTest
            // base.String_Contains_Literal()
        }


        private static string FileLineEnding = @"
";

        private static string Sql => TestSqlLoggerFactory.Sql.Replace(Environment.NewLine, FileLineEnding);
    }
}
