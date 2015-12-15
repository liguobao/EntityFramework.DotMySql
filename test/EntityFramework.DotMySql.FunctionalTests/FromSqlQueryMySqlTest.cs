// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Data.Entity.FunctionalTests;
using System.Linq;
using Microsoft.Data.Entity.FunctionalTests.TestModels.Northwind;
using Xunit;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class FromSqlQueryMySqlTest : FromSqlQueryTestBase<NorthwindQueryMySqlFixture>
    {
        public override void From_sql_queryable_simple()
        {
            using (var context = CreateContext())
            {
                var actual = context.Set<Customer>()
                    .FromSql(@"SELECT * FROM `Customers` WHERE `ContactName` LIKE '%z%'")
                    .ToArray();

                Assert.Equal(14, actual.Length);
                Assert.Equal(14, context.ChangeTracker.Entries().Count());
            }


            Assert.Equal(
                @"SELECT * FROM `Customers` WHERE `ContactName` LIKE '%z%'",
                Sql);
        }

        public override void From_sql_queryable_simple_columns_out_of_order()
        {
            using (var context = CreateContext())
            {
                var actual = context.Set<Customer>()
                    .FromSql(@"SELECT `Region`, `PostalCode`, `Phone`, `Fax`, `CustomerID`, `Country`, `ContactTitle`, `ContactName`, `CompanyName`, `City`, `Address` FROM `Customers`")
                    .ToArray();

                Assert.Equal(91, actual.Length);
                Assert.Equal(91, context.ChangeTracker.Entries().Count());
            }

            Assert.Equal(
                @"SELECT `Region`, `PostalCode`, `Phone`, `Fax`, `CustomerID`, `Country`, `ContactTitle`, `ContactName`, `CompanyName`, `City`, `Address` FROM `Customers`",
                Sql);
        }

        public override void From_sql_queryable_simple_columns_out_of_order_and_extra_columns()
        {
            using (var context = CreateContext())
            {
                var actual = context.Set<Customer>()
                    .FromSql(@"SELECT `Region`, `PostalCode`, `PostalCode` AS `Foo`, `Phone`, `Fax`, `CustomerID`, `Country`, `ContactTitle`, `ContactName`, `CompanyName`, `City`, `Address` FROM `Customers`")
                    .ToArray();

                Assert.Equal(91, actual.Length);
                Assert.Equal(91, context.ChangeTracker.Entries().Count());
            }

            Assert.Equal(
                @"SELECT `Region`, `PostalCode`, `PostalCode` AS `Foo`, `Phone`, `Fax`, `CustomerID`, `Country`, `ContactTitle`, `ContactName`, `CompanyName`, `City`, `Address` FROM `Customers`",
                Sql);
        }

        public override void From_sql_queryable_composed()
        {
            using (var context = CreateContext())
            {
                var actual = context.Set<Customer>()
                    .FromSql(@"SELECT * FROM `Customers`")
                    .Where(c => c.ContactName.Contains("z"))
                    .ToArray();

                Assert.Equal(14, actual.Length);
            }

            Assert.Equal(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM (
    SELECT * FROM `Customers`
) AS `c`
WHERE `c`.`ContactName` LIKE CONCAT(CONCAT('%','z'),'%')",
                Sql);
        }

        public override void From_sql_queryable_multiple_composed()
        {
            using (var context = CreateContext())
            {
                var actual
                    = (from c in context.Set<Customer>().FromSql(@"SELECT * FROM `Customers`")
                       from o in context.Set<Order>().FromSql(@"SELECT * FROM `Orders`")
                       where c.CustomerID == o.CustomerID
                       select new { c, o })
                        .ToArray();

                Assert.Equal(830, actual.Length);
            }

            Assert.Equal(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`, `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM (
    SELECT * FROM `Customers`
) AS `c`
CROSS JOIN (
    SELECT * FROM `Orders`
) AS `o`
WHERE `c`.`CustomerID` = `o`.`CustomerID`",
                Sql);
        }

        public override void From_sql_queryable_multiple_composed_with_closure_parameters()
        {
            var startDate = new DateTime(1997, 1, 1);
            var endDate = new DateTime(1998, 1, 1);

            using (var context = CreateContext())
            {
                var actual
                    = (from c in context.Set<Customer>().FromSql(@"SELECT * FROM `Customers`")
                       from o in context.Set<Order>().FromSql(@"SELECT * FROM `Orders` WHERE `OrderDate` BETWEEN {0} AND {1}",
                           startDate,
                           endDate)
                       where c.CustomerID == o.CustomerID
                       select new { c, o })
                        .ToArray();

                Assert.Equal(411, actual.Length);
            }

            Assert.Equal(
                @"@__8__locals1_startDate_1: 01/01/1997 00:00:00
@__8__locals1_endDate_2: 01/01/1998 00:00:00

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`, `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM (
    SELECT * FROM `Customers`
) AS `c`
CROSS JOIN (
    SELECT * FROM `Orders` WHERE `OrderDate` BETWEEN @__8__locals1_startDate_1 AND @__8__locals1_endDate_2
) AS `o`
WHERE `c`.`CustomerID` = `o`.`CustomerID`",
                Sql);
        }

        public override void From_sql_queryable_multiple_composed_with_parameters_and_closure_parameters()
        {
            var city = "London";
            var startDate = new DateTime(1997, 1, 1);
            var endDate = new DateTime(1998, 1, 1);

            using (var context = CreateContext())
            {
                var actual
                    = (from c in context.Set<Customer>().FromSql(@"SELECT * FROM `Customers` WHERE `City` = {0}",
                        city)
                       from o in context.Set<Order>().FromSql(@"SELECT * FROM `Orders` WHERE `OrderDate` BETWEEN {0} AND {1}",
                           startDate,
                           endDate)
                       where c.CustomerID == o.CustomerID
                       select new { c, o })
                        .ToArray();

                Assert.Equal(25, actual.Length);
            }

            Assert.Equal(
                @"@p0: London
@__8__locals1_startDate_1: 01/01/1997 00:00:00
@__8__locals1_endDate_2: 01/01/1998 00:00:00

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`, `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM (
    SELECT * FROM `Customers` WHERE `City` = @p0
) AS `c`
CROSS JOIN (
    SELECT * FROM `Orders` WHERE `OrderDate` BETWEEN @__8__locals1_startDate_1 AND @__8__locals1_endDate_2
) AS `o`
WHERE `c`.`CustomerID` = `o`.`CustomerID`",
                Sql);
        }

        public override void From_sql_queryable_multiple_line_query()
        {
            using (var context = CreateContext())
            {
                var actual = context.Set<Customer>()
                    .FromSql(@"SELECT *
FROM `Customers`
WHERE `City` = 'London'")
                    .ToArray();

                Assert.Equal(6, actual.Length);
                Assert.True(actual.All(c => c.City == "London"));
            }

            Assert.Equal(
                @"SELECT *
FROM `Customers`
WHERE `City` = 'London'",
                Sql);
        }

        public override void From_sql_queryable_composed_multiple_line_query()
        {
            using (var context = CreateContext())
            {
                var actual = context.Set<Customer>()
                    .FromSql(@"SELECT *
FROM `Customers`")
                    .Where(c => c.City == "London")
                    .ToArray();

                Assert.Equal(6, actual.Length);
                Assert.True(actual.All(c => c.City == "London"));
            }

            Assert.Equal(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM (
    SELECT *
    FROM `Customers`
) AS `c`
WHERE `c`.`City` = 'London'",
                Sql);
        }

        public override void From_sql_queryable_with_parameters()
        {
            var city = "London";
            var contactTitle = "Sales Representative";

            using (var context = CreateContext())
            {
                var actual = context.Set<Customer>()
                    .FromSql(@"SELECT * FROM `Customers` WHERE `City` = {0}", city)
                    .Where(c => c.ContactTitle == contactTitle)
                    .ToArray();

                Assert.Equal(3, actual.Length);
                Assert.True(actual.All(c => c.City == "London"));
                Assert.True(actual.All(c => c.ContactTitle == "Sales Representative"));
            }

            Assert.Equal(
                @"@p0: London
@__contactTitle_1: Sales Representative

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM (
    SELECT * FROM `Customers` WHERE `City` = @p0
) AS `c`
WHERE `c`.`ContactTitle` = @__contactTitle_1",
                Sql);
        }

        public override void From_sql_queryable_with_null_parameter()
        {
            int? reportsTo = null;

            using (var context = CreateContext())
            {
                var actual = context.Set<Employee>()
                    .FromSql(
                        @"SELECT * FROM `Employees` WHERE `ReportsTo` = {0} OR (`ReportsTo` IS NULL AND {0} IS NULL)",
                        // ReSharper disable once ExpressionIsAlwaysNull
                        reportsTo)
                    .ToArray();

                Assert.Equal(1, actual.Length);
            }

            Assert.Equal(
                @"@p0: 

SELECT * FROM `Employees` WHERE `ReportsTo` = @p0 OR (`ReportsTo` IS NULL AND @p0 IS NULL)",
                Sql);
        }

        public override void From_sql_queryable_with_parameters_and_closure()
        {
            var city = "London";
            var contactTitle = "Sales Representative";

            using (var context = CreateContext())
            {
                var actual = context.Set<Customer>()
                    .FromSql(@"SELECT * FROM `Customers` WHERE `City` = {0}", city)
                    .Where(c => c.ContactTitle == contactTitle)
                    .ToArray();

                Assert.Equal(3, actual.Length);
                Assert.True(actual.All(c => c.City == "London"));
                Assert.True(actual.All(c => c.ContactTitle == "Sales Representative"));
            }

            Assert.Equal(
                @"@p0: London
@__contactTitle_1: Sales Representative

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM (
    SELECT * FROM `Customers` WHERE `City` = @p0
) AS `c`
WHERE `c`.`ContactTitle` = @__contactTitle_1",
                Sql);
        }

        public override void From_sql_queryable_simple_cache_key_includes_query_string()
        {
            using (var context = CreateContext())
            {
                var actual = context.Set<Customer>()
                    .FromSql(@"SELECT * FROM `Customers` WHERE `City` = 'London'")
                    .ToArray();

                Assert.Equal(6, actual.Length);
                Assert.True(actual.All(c => c.City == "London"));

                actual = context.Set<Customer>()
                    .FromSql(@"SELECT * FROM `Customers` WHERE `City` = 'Seattle'")
                    .ToArray();

                Assert.Equal(1, actual.Length);
                Assert.True(actual.All(c => c.City == "Seattle"));
            }

            Assert.Equal(
                @"SELECT * FROM `Customers` WHERE `City` = 'London'

SELECT * FROM `Customers` WHERE `City` = 'Seattle'",
                Sql);
        }

        public override void From_sql_queryable_with_parameters_cache_key_includes_parameters()
        {
            var city = "London";
            var contactTitle = "Sales Representative";
            var sql = @"SELECT * FROM `Customers` WHERE `City` = {0} AND `ContactTitle` = {1}";

            using (var context = CreateContext())
            {
                var actual = context.Set<Customer>()
                    .FromSql(sql, city, contactTitle)
                    .ToArray();

                Assert.Equal(3, actual.Length);
                Assert.True(actual.All(c => c.City == "London"));
                Assert.True(actual.All(c => c.ContactTitle == "Sales Representative"));

                city = "Madrid";
                contactTitle = "Accounting Manager";

                actual = context.Set<Customer>()
                    .FromSql(sql, city, contactTitle)
                    .ToArray();

                Assert.Equal(2, actual.Length);
                Assert.True(actual.All(c => c.City == "Madrid"));
                Assert.True(actual.All(c => c.ContactTitle == "Accounting Manager"));
            }

            Assert.Equal(
                @"@p0: London
@p1: Sales Representative

SELECT * FROM `Customers` WHERE `City` = @p0 AND `ContactTitle` = @p1

@p0: Madrid
@p1: Accounting Manager

SELECT * FROM `Customers` WHERE `City` = @p0 AND `ContactTitle` = @p1",
                Sql);
        }

        public override void From_sql_queryable_simple_as_no_tracking_not_composed()
        {
            using (var context = CreateContext())
            {
                var actual = context.Set<Customer>()
                    .FromSql(@"SELECT * FROM `Customers`")
                    .AsNoTracking()
                    .ToArray();

                Assert.Equal(91, actual.Length);
                Assert.Equal(0, context.ChangeTracker.Entries().Count());
            }

            Assert.Equal(
                @"SELECT * FROM `Customers`",
                Sql);
        }

        public override void From_sql_queryable_simple_projection_composed()
        {
            base.From_sql_queryable_simple_projection_composed();

            Assert.Equal(
                @"SELECT `p`.`ProductName`
FROM (
    SELECT *
    FROM Products
    WHERE Discontinued <> 1
    AND ((UnitsInStock + UnitsOnOrder) < ReorderLevel)
) AS `p`",
                Sql);
        }

        public override void From_sql_queryable_simple_include()
        {
            using (var context = CreateContext())
            {
                var actual = context.Set<Customer>()
                    .FromSql(@"SELECT * FROM `Customers`")
                    .Include(c => c.Orders)
                    .ToArray();

                Assert.Equal(830, actual.SelectMany(c => c.Orders).Count());
            }

            Assert.Equal(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM (
    SELECT * FROM `Customers`
) AS `c`
ORDER BY `c`.`CustomerID`

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
INNER JOIN (
    SELECT DISTINCT `c`.`CustomerID`
    FROM (
        SELECT * FROM `Customers`
    ) AS `c`
) AS `c` ON `o`.`CustomerID` = `c`.`CustomerID`
ORDER BY `c`.`CustomerID`",
                Sql);
        }

        public override void From_sql_queryable_simple_composed_include()
        {
            using (var context = CreateContext())
            {
                var actual = context.Set<Customer>()
                    .FromSql(@"SELECT * FROM `Customers`")
                    .Where(c => c.City == "London")
                    .Include(c => c.Orders)
                    .ToArray();

                Assert.Equal(46, actual.SelectMany(c => c.Orders).Count());
            }

            Assert.Equal(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM (
    SELECT * FROM `Customers`
) AS `c`
WHERE `c`.`City` = 'London'
ORDER BY `c`.`CustomerID`

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
INNER JOIN (
    SELECT DISTINCT `c`.`CustomerID`
    FROM (
        SELECT * FROM `Customers`
    ) AS `c`
    WHERE `c`.`City` = 'London'
) AS `c` ON `o`.`CustomerID` = `c`.`CustomerID`
ORDER BY `c`.`CustomerID`",
                Sql);
        }

        public override void From_sql_annotations_do_not_affect_successive_calls()
        {
            using (var context = CreateContext())
            {
                var actual = context.Customers
                    .FromSql(@"SELECT * FROM `Customers` WHERE `ContactName` LIKE '%z%'")
                    .ToArray();

                Assert.Equal(14, actual.Length);

                actual = context.Customers
                    .ToArray();

                Assert.Equal(91, actual.Length);
            }

            Assert.Equal(
                @"SELECT * FROM `Customers` WHERE `ContactName` LIKE '%z%'

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`",
                Sql);
        }

        public override void From_sql_composed_with_nullable_predicate()
        {
            using (var context = CreateContext())
            {
                var actual = context.Set<Customer>()
                    .FromSql(@"SELECT * FROM `Customers`")
                    .Where(c => c.ContactName == c.CompanyName)
                    .ToArray();

                Assert.Equal(0, actual.Length);
            }

            Assert.Equal(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM (
    SELECT * FROM `Customers`
) AS `c`
WHERE (`c`.`ContactName` = `c`.`CompanyName`) OR (`c`.`ContactName` IS NULL AND `c`.`CompanyName` IS NULL)",
                Sql);
        }

        public FromSqlQueryMySqlTest(NorthwindQueryMySqlFixture fixture)
            : base(fixture)
        {
        }

        private static string FileLineEnding = @"
";

        private static string Sql => TestSqlLoggerFactory.Sql.Replace(Environment.NewLine, FileLineEnding);
    }
}
