using System;

namespace grate.Infrastructure
{
    public class OracleSyntax : ISyntax
    {
        public string StatementSeparatorRegex
        {
            get
            {
                const string strings = @"(?<KEEP1>'[^']*')";
                const string dashComments = @"(?<KEEP1>--.*$)";
                const string starComments = @"(?<KEEP1>/\*[\S\s]*?\*/)";
                const string separator = @"(?<KEEP1>^|\s)(?<BATCHSPLITTER>GO)(?<KEEP2>\s|;|$)";
                return strings + "|" + dashComments + "|" + starComments + "|" + separator;
            }
        }

        public string CurrentDatabase => "select name from v$database";
        public string ListDatabases => "SELECT datname FROM pg_database";
        public string VarcharType => "VARCHAR2";
        public string TextType => "CLOB";
        public string BigintType => "NUMBER(19)";
        public string BooleanType => "CHAR(1)";
        public string PrimaryKeyColumn(string columnName) => $"{columnName} NUMBER(19) PRIMARY KEY";

        public string CreateSchema(string schemaName) => throw new NotImplementedException("Create schema is not implemented for Oracle DB");
        
        public string CreateDatabase(string userName, string? password) => $"CREATE USER {userName} IDENTIFIED BY {password}; GRANT ALL PRIVILEGES TO {userName};";
        //public string CreateDatabase(string databaseName) => throw new NotImplementedException("Create database is not implemented for Oracle DB");
        public string DropDatabase(string databaseName) => $"DROP USER {databaseName} CASCADE";
        
        public string TableWithSchema(string schemaName, string tableName) => $"{schemaName}_{tableName}";
        public string ReturnId => "RETURNING id;";
        public string TimestampType => "timestamp";
        public string Quote(string text) => $"\"{text}\"";
        public string PrimaryKeyConstraint(string tableName, string column) => "";
        public string LimitN(string sql, int n) => sql + $"\nLIMIT {n}";
    }
}
