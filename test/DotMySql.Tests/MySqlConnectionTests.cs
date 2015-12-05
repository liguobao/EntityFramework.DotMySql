using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using MySql.Data.MySqlClient;

namespace DotMySql.Tests
{
    public class MySqlConnectionTests : TestBase
    {

        public MySqlConnectionTests()
        {
            
        }

        [Fact]
        public void BasicLifeCycle()
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                Assert.Equal(ConnectionState.Closed, conn.State);
                conn.Open();
                Assert.Equal(ConnectionState.Open, conn.State);
                conn.Close();
                Assert.Equal(ConnectionState.Closed, conn.State);
            }
        }
    }
}
