using System.Data.Common;
using System.Threading.Tasks;
using grate.Infrastructure;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace grate.Migration
{
    public class OracleDatabase : AnsiSqlDatabase
    {
        public OracleDatabase(ILogger<OracleDatabase> logger)
            : base(logger, new OracleSyntax())
        {
        }

        public override bool SupportsDdlTransactions => true;
        public override bool SupportsSchemas => false;

        protected override DbConnection GetSqlConnection(string? connectionString) => new OracleConnection(connectionString);

        protected override string ExistsSql(string tableSchema, string fullTableName)
        {
            return $@"
SELECT * FROM all_tables
WHERE 
table_name = '{fullTableName}'
";
        }

        protected override async Task CreateScriptsRunTable()
        {
            await base.CreateScriptsRunTable();
            await CreateIdSequence(ScriptsRunTable);
            await CreateIdInsertTrigger(ScriptsRunTable);
        }
        
        protected override async Task CreateScriptsRunErrorsTable()
        {
            await base.CreateScriptsRunErrorsTable();
            await CreateIdSequence(ScriptsRunErrorsTable);
            await CreateIdInsertTrigger(ScriptsRunErrorsTable);
        }
        
        protected override async Task CreateVersionTable()
        {
            await base.CreateVersionTable();
            await CreateIdSequence(VersionTable);
            await CreateIdInsertTrigger(VersionTable);
        }
        
        private async Task CreateIdSequence(string table)
        {
            var sql = $"CREATE SEQUENCE {table}_sequence;";
            await ExecuteNonQuery(Connection, sql);
        }

        private async Task CreateIdInsertTrigger(string table)
        {
            var sql = $@"
CREATE OR REPLACE TRIGGER {table}_on_insert
  BEFORE INSERT ON {table}
  FOR EACH ROW
BEGIN
  SELECT {table}_sequence.nextval
  INTO :new.id
  FROM dual;
END;
";
            await ExecuteNonQuery(Connection, sql);
        }
    }
}
