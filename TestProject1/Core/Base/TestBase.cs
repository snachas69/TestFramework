using TestProject1.Core.Logging;

namespace TestProject1.Core.Base
{
    public abstract class TestBase
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            SerilogConfiguration.Configure();
        }
    }
}
