using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace grate.unittests.TestInfrastructure
{
    public static class TestConfig
    {
        private static readonly Random Random = new();

        public static string RandomDatabase() => Random.GetString(15);
        
        public static readonly ILoggerFactory LogFactory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(new NUnitLoggerProvider())
                .SetMinimumLevel(GetLogLevel());
        });
        
        public static DirectoryInfo CreateRandomTempDirectory()
        {
            var dummyFile = Path.GetTempFileName();
            File.Delete(dummyFile);

            var scriptsDir = Directory.CreateDirectory(dummyFile);
            return scriptsDir;
        }
    
        private static LogLevel GetLogLevel()
        {
            if (!Enum.TryParse(Environment.GetEnvironmentVariable("LogLevel"), out LogLevel logLevel))
            {
                logLevel = LogLevel.Trace;
            }
            return logLevel;
        }
        
    }
}
