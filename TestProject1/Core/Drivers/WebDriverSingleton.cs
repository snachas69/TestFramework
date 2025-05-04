using OpenQA.Selenium;

namespace TestProject1.Core.Drivers
{
    public static class WebDriverSingleton
    {
        private static IWebDriver? _driver;
        private static string _browserType = "chrome";

        public static string? DownloadsPath { get; set; }

        public static string BrowserType
        {
            get => _browserType ?? string.Empty;
            set => _browserType = value ?? "chrome";
        }

        public static IWebDriver Driver
        {
            get
            {
                if (_driver == null)
                {
                    _driver = WebDriverFactory.CreateDriver(BrowserType, DownloadsPath);
                }

                return _driver;
            }
        }

        public static void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
            _driver = null;
        }
    }
}
