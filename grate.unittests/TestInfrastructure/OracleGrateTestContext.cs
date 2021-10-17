using System;
using System.Data.Common;
using grate.Configuration;
using grate.Infrastructure;
using grate.Migration;
using Microsoft.Extensions.Logging;
using Npgsql;
using NSubstitute;
using Oracle.ManagedDataAccess.Client;

namespace grate.unittests.TestInfrastructure
{
    internal class OracleGrateTestContext : TestContextBase, IGrateTestContext, IDockerTestContext
    {
        public string AdminPassword { get; set; } = default!;
        public int? Port { get; set; }

        public string DockerCommand(string serverName, string adminPassword) =>
            $"run -d --name {serverName} -e ORACLE_ENABLE_XDB=true -P oracleinanutshell/oracle-xe-11g:latest";
            //$"run -d --name {serverName} -e ORACLE_ENABLE_XDB=true {adminPassword} -P oracleinanutshell/oracle-xe-11g:latest";
        //$"run -d -P -e ORACLE_ENABLE_XDB=true"

        public string ConnString1 =
            "SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort))(CONNECT_DATA=(SERVICE_NAME=MyOracleSID)));uid=myUsername;pwd=myPassword;";

        public string ConnString2 =
            "Data Source=localhost:1521/XE;Persist Security Info=True;User ID=scott;Password=tiger";

        
//         public string ConnectionString(string database) => $@"
// SERVER=
//     (DESCRIPTION=
//         (ADDRESS=(PROTOCOL=TCP)
//         (HOST=127.0.0.1)
//         (PORT={Port}))
//     (CONNECT_DATA=
//         (SERVICE_NAME={database})));
// uid=system;pwd=oracle;
// ";

        //public string AdminConnectionString => $"user id=system;password={AdminPassword};data source=localhost:{Port}/XE";
        public string AdminConnectionString =>
            $@"Data Source=localhost:{Port}/XE;Persist Security Info=True;User ID=system;Password=oracle";
        public string ConnectionString(string database) => $"user id=system;password=oracle;data source=localhost:{Port}/{database}";
        //public string ConnectionString(string database) => $"user id=system;password={AdminPassword};data source=localhost:{Port}/{database}";

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
            SelectVersion = "SELECT version()",
            SelectCurrentDatabase = "SELECT current_database()"
        };


        public string ExpectedVersionPrefix => "PostgreSQL 14.";
    }
}
