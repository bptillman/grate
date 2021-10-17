using grate.unittests.TestInfrastructure;
using NUnit.Framework;

namespace grate.unittests.Oracle.Running_MigrationScripts
{
    [TestFixture]
    [Category("Oracle")]
    public class Failing_Scripts: Generic.Running_MigrationScripts.Failing_Scripts
    {
        protected override IGrateTestContext Context => GrateTestContext.Oracle;

        protected override string ExpextedErrorMessageForInvalidSql =>
@"42703: column ""top"" does not exist";
    }
}
