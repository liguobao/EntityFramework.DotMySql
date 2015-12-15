// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Data.Entity.FunctionalTests;
using Microsoft.Data.Entity.FunctionalTests.TestModels.Inheritance;
using System.Linq;
using Xunit;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class InheritanceMySqlTest : InheritanceTestBase<InheritanceMySqlFixture>
    {
        public override void Can_use_of_type_animal()
        {
            base.Can_use_of_type_animal();


            Assert.Equal(
                @"SELECT `t0`.`Species`, `t0`.`CountryId`, `t0`.`Discriminator`, `t0`.`Name`, `t0`.`EagleId`, `t0`.`IsFlightless`, `t0`.`Group`, `t0`.`FoundOn`
FROM (
    SELECT `a`.`Species`, `a`.`CountryId`, `a`.`Discriminator`, `a`.`Name`, `a`.`EagleId`, `a`.`IsFlightless`, `a`.`Group`, `a`.`FoundOn`
    FROM `Animal` AS `a`
    WHERE `a`.`Discriminator` IN ('Kiwi', 'Eagle')
) AS `t0`
ORDER BY `t0`.`Species`",
                Sql);
        }

        public override void Can_use_of_type_bird()
        {
            base.Can_use_of_type_bird();

            Assert.Equal(
                @"SELECT `t0`.`Species`, `t0`.`CountryId`, `t0`.`Discriminator`, `t0`.`Name`, `t0`.`EagleId`, `t0`.`IsFlightless`, `t0`.`Group`, `t0`.`FoundOn`
FROM (
    SELECT `a`.`Species`, `a`.`CountryId`, `a`.`Discriminator`, `a`.`Name`, `a`.`EagleId`, `a`.`IsFlightless`, `a`.`Group`, `a`.`FoundOn`
    FROM `Animal` AS `a`
    WHERE `a`.`Discriminator` IN ('Kiwi', 'Eagle')
) AS `t0`
ORDER BY `t0`.`Species`",
                Sql);
        }

        public override void Can_use_of_type_bird_predicate()
        {
            base.Can_use_of_type_bird_predicate();

            Assert.Equal(
                @"SELECT `t0`.`Species`, `t0`.`CountryId`, `t0`.`Discriminator`, `t0`.`Name`, `t0`.`EagleId`, `t0`.`IsFlightless`, `t0`.`Group`, `t0`.`FoundOn`
FROM (
    SELECT `a`.`Species`, `a`.`CountryId`, `a`.`Discriminator`, `a`.`Name`, `a`.`EagleId`, `a`.`IsFlightless`, `a`.`Group`, `a`.`FoundOn`
    FROM `Animal` AS `a`
    WHERE `a`.`Discriminator` IN ('Kiwi', 'Eagle') AND (`a`.`CountryId` = 1)
) AS `t0`
ORDER BY `t0`.`Species`",
                Sql);
        }

        public override void Can_use_of_type_bird_with_projection()
        {
            base.Can_use_of_type_bird_with_projection();

            Assert.Equal(
                @"SELECT `t0`.`EagleId`
FROM (
    SELECT `a`.`Species`, `a`.`CountryId`, `a`.`Discriminator`, `a`.`Name`, `a`.`EagleId`, `a`.`IsFlightless`, `a`.`Group`, `a`.`FoundOn`
    FROM `Animal` AS `a`
    WHERE `a`.`Discriminator` IN ('Kiwi', 'Eagle')
) AS `t0`",
                Sql);
        }

        public override void Can_use_of_type_bird_first()
        {
            base.Can_use_of_type_bird_first();

            Assert.Equal(
                @"SELECT `t0`.`Species`, `t0`.`CountryId`, `t0`.`Discriminator`, `t0`.`Name`, `t0`.`EagleId`, `t0`.`IsFlightless`, `t0`.`Group`, `t0`.`FoundOn`
FROM (
    SELECT `a`.`Species`, `a`.`CountryId`, `a`.`Discriminator`, `a`.`Name`, `a`.`EagleId`, `a`.`IsFlightless`, `a`.`Group`, `a`.`FoundOn`
    FROM `Animal` AS `a`
    WHERE `a`.`Discriminator` IN ('Kiwi', 'Eagle')
) AS `t0`
ORDER BY `t0`.`Species`
LIMIT 1",
                Sql);
        }

        public override void Can_use_of_type_kiwi()
        {
            base.Can_use_of_type_kiwi();

            Assert.Equal(
                @"SELECT `a`.`Species`, `a`.`CountryId`, `a`.`Discriminator`, `a`.`Name`, `a`.`EagleId`, `a`.`IsFlightless`, `a`.`Group`, `a`.`FoundOn`
FROM `Animal` AS `a`
WHERE `a`.`Discriminator` = 'Kiwi'",
                Sql);
        }

        public override void Can_use_of_type_rose()
        {
            base.Can_use_of_type_rose();

            Assert.Equal(
                @"SELECT `p`.`Species`, `p`.`CountryId`, `p`.`Genus`, `p`.`Name`, `p`.`HasThorns`
FROM `Plant` AS `p`
WHERE `p`.`Genus` = 0",
                Sql);
        }

        public override void Can_query_all_animals()
        {
            base.Can_query_all_animals();

            Assert.Equal(
                @"SELECT `a`.`Species`, `a`.`CountryId`, `a`.`Discriminator`, `a`.`Name`, `a`.`EagleId`, `a`.`IsFlightless`, `a`.`Group`, `a`.`FoundOn`
FROM `Animal` AS `a`
WHERE `a`.`Discriminator` IN ('Kiwi', 'Eagle')
ORDER BY `a`.`Species`",
                Sql);
        }

        public override void Can_query_all_plants()
        {
            base.Can_query_all_plants();

            Assert.Equal(
                @"SELECT `a`.`Species`, `a`.`CountryId`, `a`.`Genus`, `a`.`Name`, `a`.`HasThorns`
FROM `Plant` AS `a`
WHERE `a`.`Genus` IN (0, 1)
ORDER BY `a`.`Species`",
                Sql);
        }

        public override void Can_filter_all_animals()
        {
            base.Can_filter_all_animals();

            Assert.Equal(
                @"SELECT `a`.`Species`, `a`.`CountryId`, `a`.`Discriminator`, `a`.`Name`, `a`.`EagleId`, `a`.`IsFlightless`, `a`.`Group`, `a`.`FoundOn`
FROM `Animal` AS `a`
WHERE `a`.`Discriminator` IN ('Kiwi', 'Eagle') AND (`a`.`Name` = 'Great spotted kiwi')
ORDER BY `a`.`Species`",
                Sql);
        }

        public override void Can_query_all_birds()
        {
            base.Can_query_all_birds();

            Assert.Equal(
                @"SELECT `a`.`Species`, `a`.`CountryId`, `a`.`Discriminator`, `a`.`Name`, `a`.`EagleId`, `a`.`IsFlightless`, `a`.`Group`, `a`.`FoundOn`
FROM `Animal` AS `a`
WHERE `a`.`Discriminator` IN ('Kiwi', 'Eagle')
ORDER BY `a`.`Species`",
                Sql);
        }

        public override void Can_query_just_kiwis()
        {
            base.Can_query_just_kiwis();

            Assert.Equal(
                @"SELECT `a`.`Species`, `a`.`CountryId`, `a`.`Discriminator`, `a`.`Name`, `a`.`EagleId`, `a`.`IsFlightless`, `a`.`FoundOn`
FROM `Animal` AS `a`
WHERE `a`.`Discriminator` = 'Kiwi'
LIMIT 2",
                Sql);
        }

        public override void Can_query_just_roses()
        {
            base.Can_query_just_roses();

            Assert.Equal(
                @"SELECT `p`.`Species`, `p`.`CountryId`, `p`.`Genus`, `p`.`Name`, `p`.`HasThorns`
FROM `Plant` AS `p`
WHERE `p`.`Genus` = 0
LIMIT 2",
                Sql
                );
        }

        public override void Can_include_prey()
        {
            using (var context = CreateContext())
            {
                var eagle
                    = context.Set<Eagle>()
                        .Include(e => e.Prey)
                        .Single();

                Assert.NotNull(eagle);
                Assert.Equal(1, eagle.Prey.Count);
            }

            Assert.Equal(
                @"SELECT `e`.`Species`, `e`.`CountryId`, `e`.`Discriminator`, `e`.`Name`, `e`.`EagleId`, `e`.`IsFlightless`, `e`.`Group`
FROM `Animal` AS `e`
WHERE `e`.`Discriminator` = 'Eagle'
ORDER BY `e`.`Species`
LIMIT 2

SELECT `a`.`Species`, `a`.`CountryId`, `a`.`Discriminator`, `a`.`Name`, `a`.`EagleId`, `a`.`IsFlightless`, `a`.`Group`, `a`.`FoundOn`
FROM `Animal` AS `a`
INNER JOIN (
    SELECT DISTINCT `e`.`Species`
    FROM `Animal` AS `e`
    WHERE `e`.`Discriminator` = 'Eagle'
    LIMIT 2
) AS `e` ON `a`.`EagleId` = `e`.`Species`
WHERE (`a`.`Discriminator` = 'Kiwi') OR (`a`.`Discriminator` = 'Eagle')
ORDER BY `e`.`Species`",
                Sql);
        }

        public override void Can_include_animals()
        {
            base.Can_include_animals();

            Assert.Equal(
                @"SELECT `c`.`Id`, `c`.`Name`
FROM `Country` AS `c`
ORDER BY `c`.`Name`, `c`.`Id`

SELECT `a`.`Species`, `a`.`CountryId`, `a`.`Discriminator`, `a`.`Name`, `a`.`EagleId`, `a`.`IsFlightless`, `a`.`Group`, `a`.`FoundOn`
FROM `Animal` AS `a`
INNER JOIN (
    SELECT DISTINCT `c`.`Name`, `c`.`Id`
    FROM `Country` AS `c`
) AS `c` ON `a`.`CountryId` = `c`.`Id`
WHERE (`a`.`Discriminator` = 'Kiwi') OR (`a`.`Discriminator` = 'Eagle')
ORDER BY `c`.`Name`, `c`.`Id`",
                Sql);
        }

        public override void Can_insert_update_delete()
        {
            //base.Can_insert_update_delete();

            using (var context = CreateContext())
            {
                var kiwi = new Kiwi
                {
                    Species = "Apteryx owenii",
                    Name = "Little spotted kiwi",
                    IsFlightless = true,
                    FoundOn = Island.North
                };

                var nz = context.Set<Country>().Single(c => c.Id == 1);

                nz.Animals.Add(kiwi);

                context.SaveChanges();
            }

            using (var context = CreateContext())
            {
                var kiwi = context.Set<Kiwi>().Single(k => k.Species.EndsWith("owenii"));

                kiwi.EagleId = "Aquila chrysaetos canadensis";

                context.SaveChanges();
            }

            using (var context = CreateContext())
            {
                var kiwi = context.Set<Kiwi>().Single(k => k.Species.EndsWith("owenii"));

                Assert.Equal("Aquila chrysaetos canadensis", kiwi.EagleId);

                context.Set<Bird>().Remove(kiwi);

                context.SaveChanges();
            }

            using (var context = CreateContext())
            {
                var count = context.Set<Kiwi>().Count(k => k.Species.EndsWith("owenii"));

                Assert.Equal(0, count);
            }

            Assert.Equal(
                @"SELECT `c`.`Id`, `c`.`Name`
FROM `Country` AS `c`
WHERE `c`.`Id` = 1
LIMIT 2

@p0: Apteryx owenii
@p1: 1
@p2: Kiwi
@p3: Little spotted kiwi
@p4: 
@p5: True
@p6: North

INSERT INTO `Animal` (`Species`, `CountryId`, `Discriminator`, `Name`, `EagleId`, `IsFlightless`, `FoundOn`)
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6);
SELECT ROW_COUNT()

SELECT `k`.`Species`, `k`.`CountryId`, `k`.`Discriminator`, `k`.`Name`, `k`.`EagleId`, `k`.`IsFlightless`, `k`.`FoundOn`
FROM `Animal` AS `k`
WHERE (`k`.`Discriminator` = 'Kiwi') AND `k`.`Species` LIKE CONCAT('%','owenii')
LIMIT 2

@p0: Apteryx owenii
@p1: Aquila chrysaetos canadensis

UPDATE `Animal` SET `EagleId` = @p1
WHERE `Species` = @p0;
SELECT ROW_COUNT()

SELECT `k`.`Species`, `k`.`CountryId`, `k`.`Discriminator`, `k`.`Name`, `k`.`EagleId`, `k`.`IsFlightless`, `k`.`FoundOn`
FROM `Animal` AS `k`
WHERE (`k`.`Discriminator` = 'Kiwi') AND `k`.`Species` LIKE CONCAT('%','owenii')
LIMIT 2

@p0: Apteryx owenii

DELETE FROM `Animal`
WHERE `Species` = @p0;
SELECT ROW_COUNT()

SELECT CAST(COUNT(*) AS UNSIGNED)
FROM `Animal` AS `k`
WHERE (`k`.`Discriminator` = 'Kiwi') AND `k`.`Species` LIKE CONCAT('%','owenii')",
                Sql);
        }

        public InheritanceMySqlTest(InheritanceMySqlFixture fixture)
            : base(fixture)
        {
        }

        private static string FileLineEnding = @"
";

        private static string Sql => TestSqlLoggerFactory.Sql.Replace(Environment.NewLine, FileLineEnding);
    }
}
