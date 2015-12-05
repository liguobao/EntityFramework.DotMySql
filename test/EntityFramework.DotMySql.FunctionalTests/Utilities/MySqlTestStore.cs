// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Entity.FunctionalTests;
using MySql.Data.MySqlClient;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class MySqlTestStore : RelationalTestStore
    {
        public const int CommandTimeout = 90;

        public static MySqlTestStore GetOrCreateShared(string name, Action initializeDatabase)
            => new MySqlTestStore(name).CreateShared(initializeDatabase);

        /// <summary>
        ///     A non-transactional, transient, isolated test database. Use this in the case
        ///     where transactions are not appropriate.
        /// </summary>
        public static Task<MySqlTestStore> CreateScratchAsync(bool createDatabase = true)
            => new MySqlTestStore(GetScratchDbName()).CreateTransientAsync(createDatabase);

        public static MySqlTestStore CreateScratch(bool createDatabase = true)
            => new MySqlTestStore(GetScratchDbName()).CreateTransient(createDatabase);

        private MySqlConnection _connection;
        private MySqlTransaction _transaction;
        private readonly string _name;
        private bool _deleteDatabase;

        // Use async static factory method
        private MySqlTestStore(string name)
        {
            _name = name;
        }

        private static string GetScratchDbName()
        {
            string name;
            do
            {
                name = "Scratch_" + Guid.NewGuid();
            }
            while (DatabaseExists(name)
                   || DatabaseFilesExist(name));

            return name;
        }

        private MySqlTestStore CreateShared(Action initializeDatabase)
        {
            CreateShared(typeof(MySqlTestStore).Name + _name, initializeDatabase);

            _connection = new MySqlConnection(CreateConnectionString(_name));

            _connection.Open();

            _transaction = _connection.BeginTransaction();

            return this;
        }

        public static async Task CreateDatabaseAsync(string name, string scriptPath = null, bool recreateIfAlreadyExists = false)
        {
            using (var master = new MySqlConnection(CreateConnectionString("mysql")))
            {
                await master.OpenAsync();

                using (var command = master.CreateCommand())
                {
                    var exists = DatabaseExists(name);
                    if (exists && recreateIfAlreadyExists)
                    {
                        // if scriptPath is non-null assume that the script will handle dropping DB
                        if (scriptPath == null)
                        {
                            command.CommandText = $@"DROP DATABASE `{name}`";

                            await command.ExecuteNonQueryAsync();

                            using (var newConnection = new MySqlConnection(CreateConnectionString(name)))
                            {
                                await WaitForExistsAsync(newConnection);
                            }
                        }
                    }

                    if (!exists || recreateIfAlreadyExists)
                    {
                        if (scriptPath == null)
                        {
                            command.CommandText = $@"CREATE DATABASE `{name}`";

                            await command.ExecuteNonQueryAsync();

                            using (var newConnection = new MySqlConnection(CreateConnectionString(name)))
                            {
                                await WaitForExistsAsync(newConnection);
                            }
                        }
                        else
                        {
                            // HACK: Probe for script file as current dir
                            // is different between k build and VS run.
                            if (File.Exists(@"..\..\" + scriptPath))
                            {
                                //executing in VS - so path is relative to bin\<config> dir
                                scriptPath = @"..\..\" + scriptPath;
                            }
                            else
                            {
                                var appBase = Environment.GetEnvironmentVariable("DNX_APPBASE");
                                if (appBase != null)
                                {
                                    scriptPath = Path.Combine(appBase, scriptPath);
                                }
                            }

                            var script = File.ReadAllText(scriptPath);

                            foreach (var batch
                                in new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline, TimeSpan.FromMilliseconds(1000.0))
                                    .Split(script))
                            {
                                command.CommandText = batch;

                                await command.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }
            }
        }

        public static void CreateDatabase(string name, string scriptPath = null, bool recreateIfAlreadyExists = false)
        {
            using (var master = new MySqlConnection(CreateConnectionString("mysql")))
            {
                master.Open();

                using (var command = master.CreateCommand())
                {
                    command.CommandTimeout = CommandTimeout;

                    var exists = DatabaseExists(name);
                    if (exists && recreateIfAlreadyExists)
                    {
                        // if scriptPath is non-null assume that the script will handle dropping DB
                        if (scriptPath == null)
                        {
                            command.CommandText = $@"DROP DATABASE `{name}`";

                            command.ExecuteNonQuery();

                            using (var newConnection = new MySqlConnection(CreateConnectionString(name)))
                            {
                                WaitForExists(newConnection);
                            }
                        }
                    }

                    if (!exists || recreateIfAlreadyExists)
                    {
                        if (scriptPath == null)
                        {
                            command.CommandText = $@"CREATE DATABASE `{name}`";

                            command.ExecuteNonQuery();

                            using (var newConnection = new MySqlConnection(CreateConnectionString(name)))
                            {
                                WaitForExists(newConnection);
                            }
                        }
                        else
                        {
                            // HACK: Probe for script file as current dir
                            // is different between k build and VS run.
                            if (File.Exists(@"..\..\" + scriptPath))
                            {
                                //executing in VS - so path is relative to bin\<config> dir
                                scriptPath = @"..\..\" + scriptPath;
                            }
                            else
                            {
                                var appBase = Environment.GetEnvironmentVariable("DNX_APPBASE");
                                if (appBase != null)
                                {
                                    scriptPath = Path.Combine(appBase, scriptPath);
                                }
                            }

                            var script = File.ReadAllText(scriptPath);

                            foreach (var batch
                                in new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline, TimeSpan.FromMilliseconds(1000.0))
                                    .Split(script))
                            {
                                command.CommandText = batch;

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }

        private static async Task WaitForExistsAsync(MySqlConnection connection)
        {
            var retryCount = 0;
            while (true)
            {
                try
                {
                    await connection.OpenAsync();

                    connection.Close();

                    return;
                }
                catch (MySqlException e)
                {
                    if (++retryCount >= 30
                        || (e.Number != 233 && e.Number != -2 && e.Number != 4060))
                    {
                        throw;
                    }

                    MySqlConnection.ClearPool(connection);

                    Thread.Sleep(100);
                }
            }
        }

        private static void WaitForExists(MySqlConnection connection)
        {
            var retryCount = 0;
            while (true)
            {
                try
                {
                    connection.Open();

                    connection.Close();

                    return;
                }
                catch (MySqlException e)
                {
                    if (++retryCount >= 30
                        || (e.Number != 233 && e.Number != -2 && e.Number != 4060))
                    {
                        throw;
                    }

                    MySqlConnection.ClearPool(connection);

                    Thread.Sleep(100);
                }
            }
        }

        private async Task<MySqlTestStore> CreateTransientAsync(bool createDatabase)
        {
            _connection = new MySqlConnection(CreateConnectionString(_name));

            if (createDatabase)
            {
                using (var master = new MySqlConnection(CreateConnectionString("mysql")))
                {
                    await master.OpenAsync();
                    using (var command = master.CreateCommand())
                    {
                        command.CommandTimeout = CommandTimeout;
                        command.CommandText = $"{Environment.NewLine}CREATE DATABASE `{_name}`";

                        await command.ExecuteNonQueryAsync();

                        await WaitForExistsAsync(_connection);
                    }
                }
                await _connection.OpenAsync();
            }

            _deleteDatabase = true;
            return this;
        }

        private MySqlTestStore CreateTransient(bool createDatabase)
        {
            _connection = new MySqlConnection(CreateConnectionString(_name));

            if (createDatabase)
            {
                using (var master = new MySqlConnection(CreateConnectionString("mysql")))
                {
                    master.Open();
                    using (var command = master.CreateCommand())
                    {
                        command.CommandTimeout = CommandTimeout;
                        command.CommandText = $"{Environment.NewLine}CREATE DATABASE `{_name}`";

                        command.ExecuteNonQuery();

                        WaitForExists(_connection);
                    }
                }
                _connection.Open();
            }

            _deleteDatabase = true;
            return this;
        }

        private static bool DatabaseExists(string name)
        {
            using (var master = new MySqlConnection(CreateConnectionString("mysql")))
            {
                master.Open();
                var command = master.CreateCommand();
                command.CommandTimeout = CommandTimeout;
                command.CommandText = $@"SELECT COUNT(*) FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = N'{name}'";
                object result = command.ExecuteScalar();

                //command.Dispose();

                return Convert.ToInt32(result) > 0;

            }
        }

        private static bool DatabaseFilesExist(string name)
        {
            var userFolder = Environment.GetEnvironmentVariable("USERPROFILE") ?? Environment.GetEnvironmentVariable("HOME");
            return userFolder != null
                   && (File.Exists(Path.Combine(userFolder, name + ".mdf"))
                       || File.Exists(Path.Combine(userFolder, name + "_log.ldf")));
        }

        private async Task DeleteDatabaseAsync(string name)
        {
            using (var master = new MySqlConnection(CreateConnectionString("master")))
            {
                await master.OpenAsync();

                using (var command = master.CreateCommand())
                {
                    command.CommandTimeout = CommandTimeout; // Query will take a few seconds if (and only if) there are active connections

                    // SET SINGLE_USER will close any open connections that would prevent the drop
                    command.CommandText
                        = string.Format(@"IF EXISTS (SELECT * FROM sys.databases WHERE name = N'{0}')
                                          BEGIN
                                              ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                              DROP DATABASE [{0}];
                                          END", name);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private void DeleteDatabase(string name)
        {
            using (var master = new MySqlConnection(CreateConnectionString("mysql")))
            {
                master.Open();

                using (var command = master.CreateCommand())
                {
                    command.CommandTimeout = CommandTimeout; // Query will take a few seconds if (and only if) there are active connections

                    // SET SINGLE_USER will close any open connections that would prevent the drop
                    command.CommandText
                        = string.Format(@"IF EXISTS (SELECT * FROM sys.databases WHERE name = N'{0}')
                                          BEGIN
                                              ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                              DROP DATABASE [{0}];
                                          END", name);

                    command.ExecuteNonQuery();
                }
            }
        }

        public override DbConnection Connection => _connection;

        public override DbTransaction Transaction => _transaction;

        public async Task<T> ExecuteScalarAsync<T>(string sql, CancellationToken cancellationToken, params object[] parameters)
        {
            using (var command = CreateCommand(sql, parameters))
            {
                return (T)await command.ExecuteScalarAsync(cancellationToken);
            }
        }

        public int ExecuteNonQuery(string sql, params object[] parameters)
        {
            using (var command = CreateCommand(sql, parameters))
            {
                return command.ExecuteNonQuery();
            }
        }

        public Task<int> ExecuteNonQueryAsync(string sql, params object[] parameters)
        {
            using (var command = CreateCommand(sql, parameters))
            {
                return command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, params object[] parameters)
        {
            using (var command = CreateCommand(sql, parameters))
            {
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    var results = Enumerable.Empty<T>();

                    while (await dataReader.ReadAsync())
                    {
                        try
                        {
                            results = results.Concat(new[] { await dataReader.GetFieldValueAsync<T>(0) });
                        }
                        catch (NotImplementedException)
                        {
                            // TODO remove workaround for mono limitation.
                            results = results.Concat(new[] { (T)dataReader.GetValue(0) });
                        }
                    }

                    return results;
                }
            }
        }

        private DbCommand CreateCommand(string commandText, object[] parameters)
        {
            var command = _connection.CreateCommand();

            if (_transaction != null)
            {
                command.Transaction = _transaction;
            }

            command.CommandText = commandText;
            command.CommandTimeout = CommandTimeout;

            for (var i = 0; i < parameters.Length; i++)
            {
                command.Parameters.AddWithValue("p" + i, parameters[i]);
            }

            return command;
        }

        public override void Dispose()
        {
            _transaction?.Dispose();

            _connection.Dispose();

            if (_deleteDatabase)
            {
                DeleteDatabase(_name);
            }
        }

        public static string CreateConnectionString(string name)
        {
            var connStrBuilder = new MySqlConnectionStringBuilder
            {
                //MultipleActiveResultSets = false,
                Database = name
            };
            return connStrBuilder.ApplyConfiguration().ConnectionString;
        }
    }
}