using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DotMySql.Tests
{
    public class TestBase : IDisposable
    {

        public TestBase()
        {
            TestFixtureSetup();
            SetUp();   
        }

        internal MySqlConnection Conn { get; set; }

        protected virtual string ConnectionString { get { return _connectionString; } }
        private string _connectionString;

        static bool _loggingSetUp;

        /*public string ConnectionString
        {
            get { return "server=localhost;Database=test;Uid=root;Pwd="; }
        }*/

        protected virtual string ConnectionStringEF
        {
            get
            {
                if (connectionStringEF == null)
                {
                    //Reuse all strings just add _ef at end of database name for
                    var connectionSB = new MySqlConnectionStringBuilder(ConnectionString);
                    connectionSB.Database += "_ef";
                    connectionStringEF = connectionSB.ConnectionString;
                }
                return connectionStringEF;
            }
        }
        private string connectionStringEF;

        private const string DEFAULT_CONNECTION_STRING = "server=localhost;Database=mysql_tests;Uid=root;Pwd=";

        public virtual void TestFixtureSetup()
        {
            SetupLogging();

            _connectionString = DEFAULT_CONNECTION_STRING;

            
        }

        protected virtual void SetUp()
        {
            Conn = new MySqlConnection(ConnectionString);
            try
            {
                Conn.Open();
            }
            catch (MySqlException e)
            {
                /*if (e.Code == "3D000")
                    TestUtil.IgnoreExceptOnBuildServer("Please create a database npgsql_tests, owned by user npgsql_tests");
                else if (e.Code == "28P01")
                    TestUtil.IgnoreExceptOnBuildServer("Please create a user npgsql_tests as follows: create user npgsql_tests with password 'npgsql_tests'");
                else
                    throw;*/
            }
        }

        protected virtual void TearDown()
        {
            try { Conn.Close(); }
            finally { Conn = null; }
        }
        protected void CreateSchema(string schemaName)
        {
            ExecuteNonQuery(String.Format("CREATE SCHEMA IF NOT EXISTS {0}", schemaName));
            
        }


        #region Utilities for use by tests

        protected int ExecuteNonQuery(string sql, MySqlConnection conn = null, MySqlTransaction tx = null)
        {
            if (conn == null)
                conn = Conn;
            var cmd = tx == null ? new MySqlCommand(sql, conn) : new MySqlCommand(sql, conn, tx);
            using (cmd)
                return cmd.ExecuteNonQuery();
        }

        protected object ExecuteScalar(string sql, MySqlConnection conn = null, MySqlTransaction tx = null)
        {
            if (conn == null)
                conn = Conn;
            var cmd = tx == null ? new MySqlCommand(sql, conn) : new MySqlCommand(sql, conn, tx);
            using (cmd)
                return cmd.ExecuteScalar();
        }

        protected virtual void SetupLogging()
        {
            /*var config = new LoggingConfiguration();
            var consoleTarget = new ConsoleTarget();
            consoleTarget.Layout = @"${message} ${exception:format=tostring}";
            config.AddTarget("console", consoleTarget);
            var rule = new LoggingRule("*", NLog.LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule);
            NLog.LogManager.Configuration = config;

            if (!_loggingSetUp)
            {
                NpgsqlLogManager.Provider = new NLogLoggingProvider();
                NpgsqlLogManager.IsParameterLoggingEnabled = true;
                _loggingSetUp = true;
            }*/
        }

#if !NET40
        /*protected async Task<int> ExecuteNonQueryAsync(string sql, MySqlConnection conn = null, MySqlTransaction tx = null)
        {
            if (conn == null)
                conn = Conn;
            var cmd = tx == null ? new MySqlCommand(sql, conn) : new MySqlCommand(sql, conn, tx);
            using (cmd)
                return await cmd.ExecuteNonQueryAsync();
        }

        protected async Task<object> ExecuteScalarAsync(string sql, MySqlConnection conn = null, MySqlTransaction tx = null)
        {
            if (conn == null)
                conn = Conn;
            var cmd = tx == null ? new MySqlCommand(sql, conn) : new MySqlCommand(sql, conn, tx);
            using (cmd)
                return await cmd.ExecuteScalarAsync();
        }*/
#endif

        protected static bool IsSequential(CommandBehavior behavior)
        {
            return (behavior & CommandBehavior.SequentialAccess) != 0;
        }


        /// <summary>
        /// In PG under 9.1 you can't do SELECT pg_sleep(2) in binary because that function returns void and PG doesn't know
        /// how to transfer that. So cast to text server-side.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        /*protected static MySqlCommand CreateSleepCommand(MySqlConnection conn, int seconds)
        {
            return new MySqlCommand(string.Format("SELECT pg_sleep({0}){1}", seconds, conn.PostgreSqlVersion < new Version(9, 1, 0) ? "::TEXT" : ""), conn);
        }*/

        #endregion

        public void Dispose()
        {
            TearDown();
        }
    }
}
