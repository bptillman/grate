using grate.unittests.TestInfrastructure;
using NUnit.Framework;

namespace grate.unittests.PostgreSQL.Running_MigrationScripts
{
    [TestFixture]
    [Category("PostgreSQL")]
    public class Environment_scripts: Generic.Running_MigrationScripts.Environment_scripts
    {
        protected override IGrateTestContext Context => GrateTestContext.PostgreSql;
    }
}
