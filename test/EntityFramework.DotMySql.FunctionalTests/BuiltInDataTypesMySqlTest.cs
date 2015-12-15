// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.Data.Entity.FunctionalTests;
using Microsoft.Data.Entity.Metadata;
using Xunit;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class BuiltInDataTypesMySqlTest : BuiltInDataTypesTestBase<BuiltInDataTypesMySqlFixture>
    {
        public BuiltInDataTypesMySqlTest(BuiltInDataTypesMySqlFixture fixture)
            : base(fixture)
        {
        }

        public override void Can_query_using_any_data_type()
        {
            using (var context = CreateContext())
            {
                context.Set<BuiltInDataTypes>().Add(
                    new BuiltInDataTypes
                    {
                        Id = 11,
                        PartitionId = 1,
                        TestInt16 = -1234,
                        TestInt32 = -123456789,
                        TestInt64 = -1234567890123456789L,
                        TestDouble = -1.23456789,
                        TestDecimal = -1234567890.01M,
                        TestDateTime = Fixture.DefaultDateTime,
                        TestDateTimeOffset = new DateTimeOffset(new DateTime(), TimeSpan.FromHours(-8.0)),
                        TestTimeSpan = new TimeSpan(0, 10, 9, 8, 7),
                        TestSingle = -1.234F,
                        TestBoolean = true,
                        TestByte = 255,
                        TestUnsignedInt16 = 1234,
                        TestUnsignedInt32 = 1234565789U,
                        TestUnsignedInt64 = 1234567890123456789UL,
                        TestCharacter = 'a',
                        TestSignedByte = -128,
                        Enum64 = Enum64.SomeValue,
                        Enum32 = Enum32.SomeValue,
                        Enum16 = Enum16.SomeValue,
                        Enum8 = Enum8.SomeValue
                    });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var entity = context.Set<BuiltInDataTypes>().Single(e => e.Id == 11);
                var entityType = context.Model.FindEntityType(typeof(BuiltInDataTypes));

                var param1 = (short)-1234;
                Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.TestInt16 == param1));

                var param2 = -123456789;
                Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.TestInt32 == param2));

                var param3 = -1234567890123456789L;
                Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.TestInt64 == param3));

                var param4 = -1.23456789;
                Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.TestDouble == param4));

                var param5 = -1234567890.01M;
                Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.TestDecimal == param5));

                var param6 = Fixture.DefaultDateTime;
                Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.TestDateTime == param6));

                if (entityType.FindProperty("TestDateTimeOffset") != null)
                {
                    var param7 = new DateTimeOffset(new DateTime(), TimeSpan.FromHours(-8.0));
                    Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.TestDateTimeOffset == param7));
                }

                if (entityType.FindProperty("TestTimeSpan") != null)
                {
                    var param8 = new TimeSpan(0, 10, 9, 8, 7);
                    Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.TestTimeSpan == param8));
                }



                var param9 = -1.2354F;
                Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.TestSingle == param9));

                var param10 = true;
                Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.TestBoolean == param10));

                if (entityType.FindProperty("TestByte") != null)
                {
                    var param11 = (byte)255;
                    Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.TestByte == param11));
                }

                var param12 = Enum64.SomeValue;
                Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.Enum64 == param12));

                var param13 = Enum32.SomeValue;
                Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.Enum32 == param13));

                var param14 = Enum16.SomeValue;
                Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.Enum16 == param14));

                if (entityType.FindProperty("TestEnum8") != null)
                {
                    var param15 = Enum8.SomeValue;
                    Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.Enum8 == param15));
                }

                if (entityType.FindProperty("TestUnsignedInt16") != null)
                {
                    var param16 = (ushort)1234;
                    Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.TestUnsignedInt16 == param16));
                }

                if (entityType.FindProperty("TestUnsignedInt32") != null)
                {
                    var param17 = 1234565789U;
                    Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.TestUnsignedInt32 == param17));
                }

                if (entityType.FindProperty("TestUnsignedInt64") != null)
                {
                    var param18 = 1234567890123456789UL;
                    Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.TestUnsignedInt64 == param18));
                }

                if (entityType.FindProperty("TestCharacter") != null)
                {
                    var param19 = 'a';
                    Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.TestCharacter == param19));
                }

                if (entityType.FindProperty("TestSignedByte") != null)
                {
                    var param20 = (sbyte)-128;
                    Assert.Same(entity, context.Set<BuiltInDataTypes>().Single(e => e.Id == 11 && e.TestSignedByte == param20));
                }
            }
        }

        public override void Can_perform_query_with_max_length()
        {
            var shortString = "Sky";
            var shortBinary = new byte[] { 8, 8, 7, 8, 7 };
            var longString = new string('X', 8000);
            var longBinary = new byte[8000];
            for (var i = 0; i < longBinary.Length; i++)
            {
                longBinary[i] = (byte)(i);
            }

            using (var context = CreateContext())
            {
                context.Set<MaxLengthDataTypes>().Add(
                    new MaxLengthDataTypes
                    {
                        Id = 799,
                        String3 = shortString,
                        ByteArray5 = shortBinary,
                        String9000 = longString,
                        ByteArray9000 = longBinary
                    });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                Assert.NotNull(context.Set<MaxLengthDataTypes>().SingleOrDefault(e => e.Id == 799 && e.String3 == shortString));
                Assert.NotNull(context.Set<MaxLengthDataTypes>().SingleOrDefault(e => e.Id == 799 && e.ByteArray5 == shortBinary));

                Assert.NotNull(context.Set<MaxLengthDataTypes>().SingleOrDefault(e => e.Id == 799 && e.String9000 == longString));
                Assert.NotNull(context.Set<MaxLengthDataTypes>().SingleOrDefault(e => e.Id == 799 && e.ByteArray9000 == longBinary));
            }
        }

        public override void Can_insert_and_read_with_max_length_set()
        {
            const string shortString = "Sky";
            var shortBinary = new byte[] { 8, 8, 7, 8, 7 };

            var longString = new string('X', 8000);
            var longBinary = new byte[8000];
            for (var i = 0; i < longBinary.Length; i++)
            {
                longBinary[i] = (byte)(i);
            }

            using (var context = CreateContext())
            {
                context.Set<MaxLengthDataTypes>().Add(
                    new MaxLengthDataTypes
                    {
                        Id = 79,
                        String3 = shortString,
                        ByteArray5 = shortBinary,
                        String9000 = longString,
                        ByteArray9000 = longBinary
                    });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var dt = context.Set<MaxLengthDataTypes>().Single(e => e.Id == 79);

                Assert.Equal(shortString, dt.String3);
                Assert.Equal(shortBinary, dt.ByteArray5);
                Assert.Equal(longString, dt.String9000);
                Assert.Equal(longBinary, dt.ByteArray9000);
            }
        }


        public override void Can_insert_and_read_back_all_non_nullable_data_types()
        {
            using (var context = CreateContext())
            {
                context.Set<BuiltInDataTypes>().Add(
                    new BuiltInDataTypes
                    {
                        Id = 1,
                        PartitionId = 1,
                        TestInt16 = -1234,
                        TestInt32 = -123456789,
                        TestInt64 = -1234567890123456789L,
                        TestDouble = -1.23456789,
                        TestDecimal = -1234567890.01M,
                        TestDateTime = DateTime.Parse("01/01/2000 12:34:56"),
                        TestDateTimeOffset = new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)),
                        TestTimeSpan = new TimeSpan(0, 10, 9, 8, 7),
                        TestSingle = -1.234F,
                        TestBoolean = true,
                        TestByte = 255,
                        TestUnsignedInt16 = 1234,
                        TestUnsignedInt32 = 1234565789U,
                        TestUnsignedInt64 = 1234567890123456789UL,
                        TestCharacter = 'a',
                        TestSignedByte = -128,
                        Enum64 = Enum64.SomeValue,
                        Enum32 = Enum32.SomeValue,
                        Enum16 = Enum16.SomeValue,
                        Enum8 = Enum8.SomeValue
                    });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var dt = context.Set<BuiltInDataTypes>().Single(e => e.Id == 1);

                var entityType = context.Model.FindEntityType(typeof(BuiltInDataTypes));
                AssertEqualIfMapped(entityType, (short)-1234, () => dt.TestInt16);
                AssertEqualIfMapped(entityType, -123456789, () => dt.TestInt32);
                AssertEqualIfMapped(entityType, -1234567890123456789L, () => dt.TestInt64);
                AssertEqualIfMapped(entityType, -1.23456789, () => dt.TestDouble);
                AssertEqualIfMapped(entityType, -1234567890.01M, () => dt.TestDecimal);
                AssertEqualIfMapped(entityType, DateTime.Parse("01/01/2000 12:34:56"), () => dt.TestDateTime);
                AssertEqualIfMapped(entityType, new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)), () => dt.TestDateTimeOffset);
                AssertEqualIfMapped(entityType, new TimeSpan(0, 10, 9, 8, 7), () => dt.TestTimeSpan);
                AssertEqualIfMapped(entityType, -1.234F, () => dt.TestSingle);
                AssertEqualIfMapped(entityType, true, () => dt.TestBoolean);
                AssertEqualIfMapped(entityType, (byte)255, () => dt.TestByte);
                AssertEqualIfMapped(entityType, Enum64.SomeValue, () => dt.Enum64);
                AssertEqualIfMapped(entityType, Enum32.SomeValue, () => dt.Enum32);
                AssertEqualIfMapped(entityType, Enum16.SomeValue, () => dt.Enum16);
                AssertEqualIfMapped(entityType, Enum8.SomeValue, () => dt.Enum8);
                AssertEqualIfMapped(entityType, (ushort)1234, () => dt.TestUnsignedInt16);
                AssertEqualIfMapped(entityType, 1234565789U, () => dt.TestUnsignedInt32);
                AssertEqualIfMapped(entityType, 1234567890123456789UL, () => dt.TestUnsignedInt64);
                AssertEqualIfMapped(entityType, 'a', () => dt.TestCharacter);
                AssertEqualIfMapped(entityType, (sbyte)-128, () => dt.TestSignedByte);
            }
        }


        [Fact]
        public virtual void Can_query_using_any_mapped_data_type()
        {
            using (var context = CreateContext())
            {
                context.Set<MappedNullableDataTypes>().Add(
                    new MappedNullableDataTypes
                    {
                        Int = 999,
                        Bigint = 78L,
                        Smallint = 79,
                        Tinyint = 80,
                        Bit = true,
                        Double = 83.3,
                        Float = 84.4f,
                        Double_precision = 85.5,
                        Date = new DateTime(2015, 1, 2, 10, 11, 12),
                        //Datetimeoffset = new DateTimeOffset(new DateTime(), TimeSpan.Zero),
                        Datetime = new DateTime(2019, 1, 2, 14, 11, 12),
                        Time = new TimeSpan(11, 15, 12),
                        Char = "A",
                        Character = "B",
                        VarcharMax = "C",
                        Nchar = "D",
                        National_character = "E",
                        NvarcharMax = "don't",
                        Binary = new byte[] { 86 },
                        VarbinaryMax = new byte[] { 89, 90, 91, 92 },
                        Decimal = 101.7m,
                        Numeric = 103.9m
                    });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var entity = context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999);

                long? param1 = 78L;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.Bigint == param1));

                short? param2 = 79;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.Smallint == param2));

                byte? param3 = 80;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.Tinyint == param3));

                bool? param4 = true;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.Bit == param4));
                
                double? param7a = 83.3;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.Double == param7a));

                float? param7b = 84.4f;
                var obj = context.Set<MappedNullableDataTypes>().FirstOrDefault();
                Assert.Equal(entity.Float, obj.Float);
                Assert.Equal(entity.Int, obj.Int);

                double? param7c = 85.5;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.Double_precision == param7c));

                DateTime? param8 = new DateTime(2015, 1, 2);
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.Date == param8));

                //DateTimeOffset? param9 = new DateTimeOffset(new DateTime(), TimeSpan.Zero);
                //Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.Datetimeoffset == param9));

                
                DateTime? param11 = new DateTime(2019, 1, 2, 14, 11, 12);
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.Datetime == param11));

                TimeSpan? param13 = new TimeSpan(11, 15, 12);
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.Time == param13));

                var param14 = "A";
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.Char == param14));

                var param15 = "B";
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.Character== param15));

                
                var param19 = "C";
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.VarcharMax == param19));

                var param22 = "D";
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.Nchar == param22));

                var param23 = "E";
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.National_character == param23));

                var param27 = "don't";
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.NvarcharMax == param27));

                var param32 = new byte[] { 86 };
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.Binary == param32));

                
                var param35 = new byte[] { 89, 90, 91, 92 };
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.VarbinaryMax == param35));

                decimal? param38 = 102m;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.Decimal == param38));

                decimal? param40 = 104m;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 999 && e.Numeric == param40));
            }
        }

        [Fact]
        public virtual void Can_query_using_any_mapped_data_types_with_nulls()
        {
            using (var context = CreateContext())
            {
                context.Set<MappedNullableDataTypes>().Add(
                    new MappedNullableDataTypes
                    {
                        Int = 911,
                    });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var entity = context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911);

                long? param1 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Bigint == param1));

                short? param2 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Smallint == param2));
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && ((long?)((int?)e.Smallint)) == param2));

                byte? param3 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Tinyint == param3));

                bool? param4 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Bit == param4));

                double? param7a = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Double == param7a));

                float? param7b = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Float == param7b));

                double? param7c = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Double_precision == param7c));

                DateTime? param8 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Date == param8));

                //DateTimeOffset? param9 = null;
                //Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Datetimeoffset == param9));

                DateTime? param11 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Datetime == param11));

                TimeSpan? param13 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Time == param13));

                string param14 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Char == param14));

                string param15 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Character == param15));

                
                string param19 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.VarcharMax == param19));

                
                string param22 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Nchar == param22));

                string param23 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.National_character == param23));

                string param27 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.NvarcharMax == param27));

                byte[] param32 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Binary == param32));

                
                byte[] param35 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.VarbinaryMax == param35));

                decimal? param38 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Decimal == param38));
                
                decimal? param40 = null;
                Assert.Same(entity, context.Set<MappedNullableDataTypes>().Single(e => e.Int == 911 && e.Numeric == param40));
            }
        }

        [Fact]
        public virtual void Can_insert_and_read_back_all_mapped_data_types()
        {
            using (var context = CreateContext())
            {
                context.Set<MappedDataTypes>().Add(
                    new MappedDataTypes
                        {
                            Int = 77,
                            Bigint = 78L,
                            Smallint = 79,
                            Tinyint = 80,
                            Bit = true,
                            Float = 84.4f,
                            Double_precision = 85.5,
                            Date = new DateTime(2015, 1, 2, 10, 11, 12),
                            Datetime = new DateTime(2019, 1, 2, 14, 11, 12),
                            Time = new TimeSpan(11, 15, 12),
                            Char = "A",
                            Character = "B",
                            VarcharMax = "C",
                            Nchar = "D",
                            National_character = "E",
                            NvarcharMax = "F",
                            Text = "Gumball Rules!",
                            Binary = new byte[] { 86 },
                            Decimal = 101.1m,
                            Numeric = 103.3m,
                            VarbinaryMax = new byte[] { 87 },
                            Double = 83.3
                        });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var entity = context.Set<MappedDataTypes>().Single(e => e.Int == 77);

                Assert.Equal(77, entity.Int);
                Assert.Equal(78, entity.Bigint);
                Assert.Equal(79, entity.Smallint);
                Assert.Equal(80, entity.Tinyint);
                Assert.Equal(true, entity.Bit);
                Assert.Equal(83.3, entity.Double);
                Assert.Equal(84.4f, entity.Float);
                Assert.Equal(85.5, entity.Double_precision);
                Assert.Equal(new DateTime(2015, 1, 2), entity.Date);
                //Assert.Equal(new DateTimeOffset(new DateTime(2016, 1, 2, 11, 11, 12), TimeSpan.Zero), entity.Datetimeoffset);
                Assert.Equal(new DateTime(2019, 1, 2, 14, 11, 12), entity.Datetime);
                Assert.Equal(new TimeSpan(11, 15, 12), entity.Time);
                Assert.Equal("A", entity.Char);
                Assert.Equal("B", entity.Character);
                Assert.Equal("C", entity.VarcharMax);
                Assert.Equal("D", entity.Nchar);
                Assert.Equal("E", entity.National_character);
                Assert.Equal("F", entity.NvarcharMax);
                Assert.Equal("Gumball Rules!", entity.Text);
                Assert.Equal(new byte[] { 86 }, entity.Binary);
                Assert.Equal(new byte[] { 87 }, entity.VarbinaryMax);
                Assert.Equal(101m, entity.Decimal);
                Assert.Equal(103m, entity.Numeric);
            }
        }

        [Fact]
        public virtual void Can_insert_and_read_back_all_mapped_nullable_data_types()
        {
            using (var context = CreateContext())
            {
                context.Set<MappedNullableDataTypes>().Add(
                    new MappedNullableDataTypes
                        {
                            Int = 77,
                            Bigint = 78L,
                            Smallint = 79,
                            Tinyint = 80,
                            Bit = true,
                            Double = 83.3,
                            Float = 84.4f,
                            Double_precision = 85.5,
                            Date = new DateTime(2015, 1, 2, 10, 11, 12),
                            //Datetimeoffset = new DateTimeOffset(new DateTime(2016, 1, 2, 11, 11, 12), TimeSpan.Zero),
                            Datetime = new DateTime(2019, 1, 2, 14, 11, 12),
                            Time = new TimeSpan(11, 15, 12),
                            Char = "A",
                            Character = "B",
                            VarcharMax = "C",
                            Nchar = "D",
                            National_character = "E",
                            NvarcharMax = "don't",
                            Binary = new byte[] { 86 },
                            VarbinaryMax = new byte[] { 89, 90, 91, 92 },
                            Decimal = 101.1m,
                            Numeric = 103.3m
                        });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var entity = context.Set<MappedNullableDataTypes>().Single(e => e.Int == 77);

                Assert.Equal(77, entity.Int);
                Assert.Equal(78, entity.Bigint);
                Assert.Equal(79, entity.Smallint.Value);
                Assert.Equal(80, entity.Tinyint.Value);
                Assert.Equal(true, entity.Bit);
                Assert.Equal(83.3, entity.Double);
                Assert.Equal(84.4f, entity.Float);
                Assert.Equal(85.5, entity.Double_precision);
                Assert.Equal(new DateTime(2015, 1, 2), entity.Date);
                //Assert.Equal(new DateTimeOffset(new DateTime(2016, 1, 2, 11, 11, 12), TimeSpan.Zero), entity.Datetimeoffset);
                Assert.Equal(new DateTime(2019, 1, 2, 14, 11, 12), entity.Datetime);
                Assert.Equal(new TimeSpan(11, 15, 12), entity.Time);
                Assert.Equal("A", entity.Char);
                Assert.Equal("B", entity.Character);
                Assert.Equal("C", entity.VarcharMax);
                Assert.Equal("D", entity.Nchar);
                Assert.Equal("E", entity.National_character);
                Assert.Equal("don't", entity.NvarcharMax);
                Assert.Equal(new byte[] { 86 }, entity.Binary);
                Assert.Equal(new byte[] { 89, 90, 91, 92 }, entity.VarbinaryMax);
                Assert.Equal(101m, entity.Decimal);
                Assert.Equal(103m, entity.Numeric);
            }
        }

        [Fact]
        public virtual void Can_insert_and_read_back_all_mapped_data_types_set_to_null()
        {
            using (var context = CreateContext())
            {
                context.Set<MappedNullableDataTypes>().Add(
                    new MappedNullableDataTypes
                        {
                            Int = 78
                        });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var entity = context.Set<MappedNullableDataTypes>().Single(e => e.Int == 78);

                Assert.Null(entity.Bigint);
                Assert.Null(entity.Smallint);
                Assert.Null(entity.Tinyint);
                Assert.Null(entity.Bit);
                Assert.Null(entity.Float);
                Assert.Null(entity.Double);
                Assert.Null(entity.Double_precision);
                Assert.Null(entity.Date);
                //Assert.Null(entity.Datetimeoffset);
                Assert.Null(entity.Datetime);
                Assert.Null(entity.Time);
                Assert.Null(entity.Char);
                Assert.Null(entity.Character);
                Assert.Null(entity.VarcharMax);
                Assert.Null(entity.Nchar);
                Assert.Null(entity.National_character);
                Assert.Null(entity.NvarcharMax);
                Assert.Null(entity.Binary);
                Assert.Null(entity.VarbinaryMax);
                Assert.Null(entity.Decimal);
                Assert.Null(entity.Numeric);
            }
        }

        [Fact]
        public virtual void Can_insert_and_read_back_all_mapped_sized_data_types()
        {
            using (var context = CreateContext())
            {
                context.Set<MappedSizedDataTypes>().Add(
                    new MappedSizedDataTypes
                        {
                            Id = 77,
                            Char = "Wor",
                            Character = "Lon",
                            Varchar = "Tha",
                            Char_varying = "Thr",
                            Nchar = "Won",
                            National_character = "Squ",
                            Nvarchar = "Int",
                            Binary = new byte[] { 10, 11, 12 },
                            Varbinary = new byte[] { 11, 12, 13 },
                        });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var entity = context.Set<MappedSizedDataTypes>().Single(e => e.Id == 77);

                Assert.Equal("Wor", entity.Char);
                Assert.Equal("Lon", entity.Character);
                Assert.Equal("Tha", entity.Varchar);
                Assert.Equal("Thr", entity.Char_varying);
                Assert.Equal("Won", entity.Nchar);
                Assert.Equal("Squ", entity.National_character);
                Assert.Equal("Int", entity.Nvarchar);
                Assert.Equal(new byte[] { 10, 11, 12 }, entity.Binary);
                Assert.Equal(new byte[] { 11, 12, 13 }, entity.Varbinary);
            }
        }

        [Fact]
        public virtual void Can_insert_and_read_back_nulls_for_all_mapped_sized_data_types()
        {
            using (var context = CreateContext())
            {
                context.Set<MappedSizedDataTypes>().Add(
                    new MappedSizedDataTypes
                        {
                            Id = 78
                        });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var entity = context.Set<MappedSizedDataTypes>().Single(e => e.Id == 78);

                Assert.Null(entity.Char);
                Assert.Null(entity.Character);
                Assert.Null(entity.Varchar);
                Assert.Null(entity.Char_varying);
                Assert.Null(entity.Nchar);
                Assert.Null(entity.National_character);
                Assert.Null(entity.Nvarchar);
                Assert.Null(entity.Binary);
                Assert.Null(entity.Varbinary);
                
            }
        }

        [Fact]
        public virtual void Can_insert_and_read_back_all_mapped_data_types_with_scale()
        {
            using (var context = CreateContext())
            {
                context.Set<MappedScaledDataTypes>().Add(
                    new MappedScaledDataTypes
                        {
                            Id = 77,
                            Float = 83.3f,
                            Double = 85.5,
                            Decimal = 101.1m,
                            Numeric = 103.3m
                        });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var entity = context.Set<MappedScaledDataTypes>().Single(e => e.Id == 77);

                Assert.Equal(83.3f, entity.Float);
                Assert.Equal(85.5, entity.Double);
                //Assert.Equal(new DateTimeOffset(new DateTime(2016, 1, 2, 11, 11, 12), TimeSpan.Zero), entity.Datetimeoffset);
                Assert.Equal(101.1m, entity.Decimal);
                Assert.Equal(103.3m, entity.Numeric);
            }
        }

        [Fact]
        public virtual void Can_insert_and_read_back_all_mapped_data_types_with_precision_and_scale()
        {
            using (var context = CreateContext())
            {
                context.Set<MappedPrecisionAndScaledDataTypes>().Add(
                    new MappedPrecisionAndScaledDataTypes
                        {
                            Id = 77,
                            Decimal = 101.1m,
                            Numeric = 103.3m
                        });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var entity = context.Set<MappedPrecisionAndScaledDataTypes>().Single(e => e.Id == 77);

                Assert.Equal(101.1m, entity.Decimal);
                Assert.Equal(103.3m, entity.Numeric);
            }
        }

        [Fact]
        public virtual void Columns_have_expected_data_types()
        {
            string query
                = @"SELECT 
                        TABLE_NAME, 
                        COLUMN_NAME, 
                        DATA_TYPE, 
                        IS_NULLABLE,
                        CHARACTER_MAXIMUM_LENGTH, 
                        NUMERIC_PRECISION, 
                        NUMERIC_SCALE, 
                        DATETIME_PRECISION 
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME IN ('BinaryForeignKeyDataType', 'BuiltInDataTypes', 'BuiltInNullableDataTypes'
                    , 'MappedDataTypes', 'MappedNullableDataTypes', 'MappedPrecisionAndScaledDataTypes', 
                    'MappedScaledDataTypes', 'MappedSizedDataTypes', 'MaxLengthDataTypes', 'StringForeignKeyDataType', 
                    'StringKeyDataType')";

            var columns = new List<ColumnInfo>();

            using (var context = CreateContext())
            {
                var connection = context.Database.GetDbConnection();

                var command = connection.CreateCommand();
                command.CommandText = query;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var columnInfo = new ColumnInfo
                            {
                                TableName = reader.GetString(0),
                                ColumnName = reader.GetString(1),
                                DataType = reader.GetString(2),
                                IsNullable = reader.IsDBNull(3) ? null : (bool?)(reader.GetString(3) == "YES"),
                                MaxLength = reader.IsDBNull(4) ? null : (int?)reader.GetInt64(4),
                                NumericPrecision = reader.IsDBNull(5) ? null : (int?)reader.GetInt64(5),
                                NumericScale = reader.IsDBNull(6) ? null : (int?)reader.GetInt64(6),
                                DateTimePrecision = reader.IsDBNull(7) ? null : (int?)reader.GetInt16(7)
                            };

                        columns.Add(columnInfo);
                    }
                }
            }

            var builder = new StringBuilder();

            foreach (var column in columns.OrderBy(e => e.TableName).ThenBy(e => e.ColumnName))
            {
                builder.Append(column.TableName);
                builder.Append(".");
                builder.Append(column.ColumnName);
                builder.Append(" ---> [");

                if (column.IsNullable == true)
                {
                    builder.Append("nullable ");
                }

                builder.Append(column.DataType);
                builder.Append("]");

                if (column.MaxLength.HasValue)
                {
                    builder.Append(" [MaxLength = ");
                    builder.Append(column.MaxLength);
                    builder.Append("]");
                }

                if (column.NumericPrecision.HasValue)
                {
                    builder.Append(" [Precision = ");
                    builder.Append(column.NumericPrecision);
                }

                if (column.DateTimePrecision.HasValue)
                {
                    builder.Append(" [Precision = ");
                    builder.Append(column.DateTimePrecision);
                }

                if (column.NumericScale.HasValue)
                {
                    builder.Append(" Scale = ");
                    builder.Append(column.NumericScale);
                }

                if (column.NumericPrecision.HasValue
                    || column.DateTimePrecision.HasValue
                    || column.NumericScale.HasValue)
                {
                    builder.Append("]");
                }

                builder.AppendLine();
            }

            var actual = builder.ToString();

            string expected = @"binaryforeignkeydatatype.BinaryKeyDataTypeId ---> [nullable varbinary] [MaxLength = 450]
binaryforeignkeydatatype.Id ---> [int] [Precision = 10 Scale = 0]
builtindatatypes.Enum16 ---> [smallint] [Precision = 5 Scale = 0]
builtindatatypes.Enum32 ---> [int] [Precision = 10 Scale = 0]
builtindatatypes.Enum64 ---> [bigint] [Precision = 19 Scale = 0]
builtindatatypes.Enum8 ---> [tinyint] [Precision = 3 Scale = 0]
builtindatatypes.Id ---> [int] [Precision = 10 Scale = 0]
builtindatatypes.PartitionId ---> [int] [Precision = 10 Scale = 0]
builtindatatypes.TestBoolean ---> [bit] [Precision = 1]
builtindatatypes.TestByte ---> [tinyint] [Precision = 3 Scale = 0]
builtindatatypes.TestDateTime ---> [datetime] [Precision = 0]
builtindatatypes.TestDecimal ---> [decimal] [Precision = 18 Scale = 2]
builtindatatypes.TestDouble ---> [double] [Precision = 22]
builtindatatypes.TestInt16 ---> [smallint] [Precision = 5 Scale = 0]
builtindatatypes.TestInt32 ---> [int] [Precision = 10 Scale = 0]
builtindatatypes.TestInt64 ---> [bigint] [Precision = 19 Scale = 0]
builtindatatypes.TestSingle ---> [float] [Precision = 12]
builtindatatypes.TestTimeSpan ---> [time] [Precision = 0]
builtinnullabledatatypes.Enum16 ---> [nullable smallint] [Precision = 5 Scale = 0]
builtinnullabledatatypes.Enum32 ---> [nullable int] [Precision = 10 Scale = 0]
builtinnullabledatatypes.Enum64 ---> [nullable bigint] [Precision = 19 Scale = 0]
builtinnullabledatatypes.Enum8 ---> [nullable tinyint] [Precision = 3 Scale = 0]
builtinnullabledatatypes.Id ---> [int] [Precision = 10 Scale = 0]
builtinnullabledatatypes.PartitionId ---> [int] [Precision = 10 Scale = 0]
builtinnullabledatatypes.TestByteArray ---> [nullable varbinary] [MaxLength = 8000]
builtinnullabledatatypes.TestNullableBoolean ---> [nullable bit] [Precision = 1]
builtinnullabledatatypes.TestNullableByte ---> [nullable tinyint] [Precision = 3 Scale = 0]
builtinnullabledatatypes.TestNullableDateTime ---> [nullable datetime] [Precision = 0]
builtinnullabledatatypes.TestNullableDecimal ---> [nullable decimal] [Precision = 18 Scale = 2]
builtinnullabledatatypes.TestNullableDouble ---> [nullable double] [Precision = 22]
builtinnullabledatatypes.TestNullableInt16 ---> [nullable smallint] [Precision = 5 Scale = 0]
builtinnullabledatatypes.TestNullableInt32 ---> [nullable int] [Precision = 10 Scale = 0]
builtinnullabledatatypes.TestNullableInt64 ---> [nullable bigint] [Precision = 19 Scale = 0]
builtinnullabledatatypes.TestNullableSingle ---> [nullable float] [Precision = 12]
builtinnullabledatatypes.TestNullableTimeSpan ---> [nullable time] [Precision = 0]
builtinnullabledatatypes.TestString ---> [nullable varchar] [MaxLength = 8000]
mappeddatatypes.Bigint ---> [bigint] [Precision = 19 Scale = 0]
mappeddatatypes.Binary ---> [binary] [MaxLength = 1]
mappeddatatypes.Bit ---> [bit] [Precision = 1]
mappeddatatypes.Char ---> [char] [MaxLength = 1]
mappeddatatypes.Character ---> [char] [MaxLength = 1]
mappeddatatypes.Date ---> [date]
mappeddatatypes.Datetime ---> [datetime] [Precision = 0]
mappeddatatypes.Decimal ---> [decimal] [Precision = 10 Scale = 0]
mappeddatatypes.Double ---> [double] [Precision = 22]
mappeddatatypes.Double_precision ---> [double] [Precision = 22]
mappeddatatypes.Float ---> [float] [Precision = 12]
mappeddatatypes.Int ---> [int] [Precision = 10 Scale = 0]
mappeddatatypes.National_character ---> [char] [MaxLength = 1]
mappeddatatypes.Nchar ---> [char] [MaxLength = 1]
mappeddatatypes.Numeric ---> [decimal] [Precision = 10 Scale = 0]
mappeddatatypes.NvarcharMax ---> [varchar] [MaxLength = 8000]
mappeddatatypes.Smallint ---> [smallint] [Precision = 5 Scale = 0]
mappeddatatypes.Text ---> [text] [MaxLength = 65535]
mappeddatatypes.Time ---> [time] [Precision = 0]
mappeddatatypes.Tinyint ---> [tinyint] [Precision = 3 Scale = 0]
mappeddatatypes.VarbinaryMax ---> [varbinary] [MaxLength = 8000]
mappeddatatypes.VarcharMax ---> [varchar] [MaxLength = 8000]
mappednullabledatatypes.Bigint ---> [nullable bigint] [Precision = 19 Scale = 0]
mappednullabledatatypes.Binary ---> [nullable binary] [MaxLength = 1]
mappednullabledatatypes.Bit ---> [nullable bit] [Precision = 1]
mappednullabledatatypes.Char ---> [nullable char] [MaxLength = 1]
mappednullabledatatypes.Character ---> [nullable char] [MaxLength = 1]
mappednullabledatatypes.Date ---> [nullable date]
mappednullabledatatypes.Datetime ---> [nullable datetime] [Precision = 0]
mappednullabledatatypes.Decimal ---> [nullable decimal] [Precision = 10 Scale = 0]
mappednullabledatatypes.Double ---> [nullable double] [Precision = 22]
mappednullabledatatypes.Double_precision ---> [nullable double] [Precision = 22]
mappednullabledatatypes.Float ---> [nullable float] [Precision = 12]
mappednullabledatatypes.Int ---> [int] [Precision = 10 Scale = 0]
mappednullabledatatypes.National_character ---> [nullable char] [MaxLength = 1]
mappednullabledatatypes.Nchar ---> [nullable char] [MaxLength = 1]
mappednullabledatatypes.Numeric ---> [nullable decimal] [Precision = 10 Scale = 0]
mappednullabledatatypes.NvarcharMax ---> [nullable varchar] [MaxLength = 8000]
mappednullabledatatypes.Smallint ---> [nullable smallint] [Precision = 5 Scale = 0]
mappednullabledatatypes.Time ---> [nullable time] [Precision = 0]
mappednullabledatatypes.Tinyint ---> [nullable tinyint] [Precision = 3 Scale = 0]
mappednullabledatatypes.VarbinaryMax ---> [nullable varbinary] [MaxLength = 8000]
mappednullabledatatypes.VarcharMax ---> [nullable varchar] [MaxLength = 8000]
mappedprecisionandscaleddatatypes.Decimal ---> [decimal] [Precision = 5 Scale = 2]
mappedprecisionandscaleddatatypes.Id ---> [int] [Precision = 10 Scale = 0]
mappedprecisionandscaleddatatypes.Numeric ---> [decimal] [Precision = 5 Scale = 2]
mappedscaleddatatypes.Decimal ---> [decimal] [Precision = 4 Scale = 1]
mappedscaleddatatypes.Double ---> [double] [Precision = 4 Scale = 1]
mappedscaleddatatypes.Float ---> [float] [Precision = 4 Scale = 1]
mappedscaleddatatypes.Id ---> [int] [Precision = 10 Scale = 0]
mappedscaleddatatypes.Numeric ---> [decimal] [Precision = 4 Scale = 1]
mappedsizeddatatypes.Binary ---> [nullable binary] [MaxLength = 3]
mappedsizeddatatypes.Char ---> [nullable char] [MaxLength = 3]
mappedsizeddatatypes.Char_varying ---> [nullable varchar] [MaxLength = 3]
mappedsizeddatatypes.Character ---> [nullable char] [MaxLength = 3]
mappedsizeddatatypes.Id ---> [int] [Precision = 10 Scale = 0]
mappedsizeddatatypes.National_character ---> [nullable char] [MaxLength = 3]
mappedsizeddatatypes.Nchar ---> [nullable char] [MaxLength = 3]
mappedsizeddatatypes.Nvarchar ---> [nullable varchar] [MaxLength = 3]
mappedsizeddatatypes.Varbinary ---> [nullable varbinary] [MaxLength = 3]
mappedsizeddatatypes.Varchar ---> [nullable varchar] [MaxLength = 3]
maxlengthdatatypes.ByteArray5 ---> [nullable varbinary] [MaxLength = 5]
maxlengthdatatypes.ByteArray9000 ---> [nullable varbinary] [MaxLength = 8000]
maxlengthdatatypes.Id ---> [int] [Precision = 10 Scale = 0]
maxlengthdatatypes.String3 ---> [nullable varchar] [MaxLength = 3]
maxlengthdatatypes.String9000 ---> [nullable varchar] [MaxLength = 8000]
stringforeignkeydatatype.Id ---> [int] [Precision = 10 Scale = 0]
stringforeignkeydatatype.StringKeyDataTypeId ---> [nullable varchar] [MaxLength = 450]
stringkeydatatype.Id ---> [varchar] [MaxLength = 450]
";
            Console.WriteLine(actual);
            Assert.Equal(expected, actual);
        }

        private class ColumnInfo
        {
            public string TableName { get; set; }
            public string ColumnName { get; set; }
            public string DataType { get; set; }
            public bool? IsNullable { get; set; }
            public int? MaxLength { get; set; }
            public int? NumericPrecision { get; set; }
            public int? NumericScale { get; set; }
            public int? DateTimePrecision { get; set; }
        }

        private static void AssertEqualIfMapped<T>(IEntityType entityType, T expected, Expression<Func<T>> actual)
        {
            if (entityType.FindProperty(((MemberExpression)actual.Body).Member.Name) != null)
            {
                Assert.Equal(expected, actual.Compile()());
            }
        }
    }
}
