using System.Data.Common;
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
        public override bool SupportsSchemas => true;

        protected override DbConnection GetSqlConnection(string? connectionString) => new OracleConnection(connectionString);
    }
}
