using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotMySql.Tests
{
    public class MySqlCommandTests : TestBase
    {

        [Theory]
        [InlineData(new[] { true })]
        [InlineData(new[] { false })]
        [InlineData(new[] { true, true })]
        [InlineData(new[] { false, false })]
        [InlineData(new[] { false, true })]
        [InlineData(new[] { true, false })]
        public void Multiqueries(bool[] queries)
        {
            ExecuteNonQuery("CREATE TEMPORARY TABLE data (name TEXT)");
            var sb = new StringBuilder();
            foreach (var query in queries)
                sb.Append(query ? "SELECT 1;" : "UPDATE data SET name='yo' WHERE 1=0;");
            var sql = sb.ToString();
            foreach (var prepare in new[] { false, true })
            {
                var cmd = new MySqlCommand(sql, Conn);
                if (prepare)
                    cmd.Prepare();
                var reader = cmd.ExecuteReader();
                var numResultSets = queries.Count(q => q);
                for (var i = 0; i < numResultSets; i++)
                {
                    Assert.Equal(true, reader.Read());
                    Assert.Equal(1, Convert.ToInt32((long)reader[0]));
                    Assert.Equal(i != numResultSets - 1, reader.NextResult());
                }
                reader.Close();
                cmd.Dispose();
            }
            ExecuteNonQuery("DROP TABLE data");
        }

        [Theory]
        [InlineData(TestUtil.PrepareOrNot.NotPrepared)]
        [InlineData(TestUtil.PrepareOrNot.Prepared)]
        public void MultipleQueriesWithParameters(TestUtil.PrepareOrNot prepare)
        {
            var cmd = new MySqlCommand("SELECT @p1; SELECT @p2", Conn);
            var p1 = new MySqlParameter("p1", MySqlDbType.Int32);
            var p2 = new MySqlParameter("p2", MySqlDbType.Text);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            if (prepare == TestUtil.PrepareOrNot.Prepared)
            {
                cmd.Prepare();
            }
            p1.Value = 8;
            p2.Value = "foo";
            var reader = cmd.ExecuteReader();
            Assert.True(reader.Read());
            Assert.Equal(8, reader.GetInt32(0));
            Assert.True(reader.NextResult());
            Assert.True(reader.Read());
            Assert.Equal("foo", reader.GetString(0));
            Assert.False(reader.NextResult());
            reader.Close();
            cmd.Dispose();
        }

        /*[Theory]
        [InlineData(TestUtil.PrepareOrNot.NotPrepared)]
        [InlineData(TestUtil.PrepareOrNot.Prepared)]
        public void MultipleQueriesSingleRow(TestUtil.PrepareOrNot prepare)
        {
            var cmd = new MySqlCommand("SELECT 1; SELECT 2", Conn);
            if (prepare == TestUtil.PrepareOrNot.Prepared)
                cmd.Prepare();
            var reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
            Assert.True(reader.Read());
            Assert.Equal(1, reader.GetInt32(0));
            Assert.False(reader.Read());
            Assert.False(reader.NextResult());
            reader.Close();
            cmd.Dispose();
        }*/
    }
}
