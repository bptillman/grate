using System;
using System.Threading.Tasks;
using grate.Configuration;
using grate.Migration;
using grate.unittests.TestInfrastructure;
using NUnit.Framework;

namespace grate.unittests.Oracle
{
    [TestFixture]
    [Category("Oracle")]
    public class Database
    {
        protected IGrateTestContext Context => GrateTestContext.Oracle;
        
        [Test]
        public async Task Throws_Error_If_Create_Database_Is_Specified()
        {
            var db = "SomeOtherdatabase";
            
            await using var migrator = GetMigrator(GetConfiguration(db, true));
            
            // The migration should throw an error, as we don't support creating databases with Oracle.
            Assert.ThrowsAsync<NotImplementedException>(() => migrator.Migrate());
        }
        

        private GrateMigrator GetMigrator(GrateConfiguration config) => Context.GetMigrator(config);

        private GrateConfiguration GetConfiguration(string databaseName, bool createDatabase, string? adminConnectionString = null)
        {
            return new()
            {
                CreateDatabase = createDatabase, 
                ConnectionString = Context.ConnectionString(databaseName),
                AdminConnectionString = adminConnectionString ?? Context.AdminConnectionString,
                KnownFolders = KnownFolders.In(TestConfig.CreateRandomTempDirectory()),
                NonInteractive = true,
                DatabaseType = Context.DatabaseType
            };
        }
    }
}
