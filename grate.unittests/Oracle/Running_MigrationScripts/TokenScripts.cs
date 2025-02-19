﻿using grate.unittests.TestInfrastructure;
using NUnit.Framework;

namespace grate.unittests.Oracle.Running_MigrationScripts
{
    [TestFixture]
    [Category("Oracle")]
    public class TokenScripts : Generic.Running_MigrationScripts.TokenScripts
    {
        protected override IGrateTestContext Context => GrateTestContext.Oracle;
        
        protected override string CreateDatabaseName => base.CreateDatabaseName + " FROM DUAL";
        protected override string CreateViewMyCustomToken => base.CreateViewMyCustomToken + " FROM DUAL";
    }
}
