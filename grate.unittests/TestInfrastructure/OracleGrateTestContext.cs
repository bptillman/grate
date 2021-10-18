﻿using System;
using System.Data.Common;
using grate.Configuration;
using grate.Infrastructure;
using grate.Migration;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace grate.unittests.TestInfrastructure
{
    internal class OracleGrateTestContext : TestContextBase, IGrateTestContext, IDockerTestContext
    {
        public string AdminPassword { get; set; } = default!;
        public int? Port { get; set; }

        public string DockerCommand(string serverName, string adminPassword) =>
            $"run -d --name {serverName} -e ORACLE_ENABLE_XDB=true -P oracleinanutshell/oracle-xe-11g:latest";

        public string AdminConnectionString => $@"Data Source=localhost:{Port}/XE;User ID=system;Password=oracle";
        //public string ConnectionString(string database) => $@"Data Source=localhost:{Port}/XE;User ID=system;Password=oracle";
        public string ConnectionString(string database) => $@"Data Source=localhost:{Port}/XE;User ID=system;Password=oracle;Proxy User Id={database}";

        public DbConnection GetDbConnection(string connectionString) => new OracleConnection(connectionString);

        public ISyntax Syntax => new OracleSyntax();
        public Type DbExceptionType => typeof(OracleException);

        public DatabaseType DatabaseType => DatabaseType.oracle;
        public bool SupportsTransaction => true;
        public string DatabaseTypeName => "Oracle";
        public string MasterDatabase => "oracle";

        public IDatabase DatabaseMigrator => new OracleDatabase(TestConfig.LogFactory.CreateLogger<OracleDatabase>());

        public SqlStatements Sql => new()
        {
            SelectAllDatabases = "SELECT datname FROM pg_database",
            SelectVersion = "SELECT * FROM v$version WHERE banner LIKE 'Oracle%'",
            SelectCurrentDatabase = "select name from V$database;"
        };

        public string ExpectedVersionPrefix => "Oracle Database 11g Express Edition Release 11.2.0.2.0 - 64bit Production";
        public bool SupportsCreateDatabase => true;
    }
}
