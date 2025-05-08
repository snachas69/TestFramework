using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using Serilog;

namespace TestProject1.Core.Drivers
{
    public static class WebDriverFactory
    {
        public static IWebDriver CreateDriver(string? browserType, string? downloadsPath)
        {
            if (string.IsNullOrWhiteSpace(browserType))
                throw new ArgumentNullException(nameof(browserType), "Browser type must be specified.");

            switch (browserType?.ToLower())
            {
                case "chrome":
                    var chromeOptions = new ChromeOptions();

                    chromeOptions.AddArguments("--no-sandbox");
                    chromeOptions.AddArguments("--headless");
                    chromeOptions.AddArguments("--disable-dev-shm-usage");
                    chromeOptions.AddArguments("--incognito");
                    chromeOptions.AddArguments("--disable-cache");
                    chromeOptions.AddArguments("--disk-cache-size=0");
                    chromeOptions.AddArguments("--disable-gpu");
                    chromeOptions.AddArguments("--ignore-certificate-errors");
                    chromeOptions.AddArguments("--window-size=1920,1080");
                    chromeOptions.AddExcludedArgument("enable-automation");

                    if (!string.IsNullOrEmpty(downloadsPath))
                    {
                        chromeOptions.AddUserProfilePreference("download.default_directory", downloadsPath);
                        chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
                        chromeOptions.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
                    }

                    var chromeDriver = new ChromeDriver(chromeOptions);

                    Log.Information("Chrome web driver has been created");

                    return chromeDriver;

                case "firefox":
                    var profile = new FirefoxProfile();

                    if (!string.IsNullOrEmpty(downloadsPath))
                    {
                        profile.SetPreference("browser.download.folderList", 2);
                        profile.SetPreference("browser.download.dir", downloadsPath);
                        profile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf");
                        profile.SetPreference("pdfjs.disabled", true);
                    }

                    var firefoxOptions = new FirefoxOptions
                    {
                        Profile = profile
                    };

                    firefoxOptions.AddArgument("--headless");
                    firefoxOptions.AddArgument("--width=1920");
                    firefoxOptions.AddArgument("--height=1080");

                    var firefoxDriver = new FirefoxDriver(firefoxOptions);

                    Log.Information("Firefox web driver has been created");

                    return firefoxDriver;

                case "edge":
                    var edgeOptions = new EdgeOptions();

                    edgeOptions.AddArgument("headless");
                    edgeOptions.AddArgument("disable-gpu");
                    edgeOptions.AddArgument("window-size=1920,1080");
                    edgeOptions.AddArgument("no-sandbox");
                    edgeOptions.AddArgument("disable-dev-shm-usage");

                    if (!string.IsNullOrEmpty(downloadsPath))
                    {
                        edgeOptions.AddUserProfilePreference("download.default_directory", downloadsPath);
                        edgeOptions.AddUserProfilePreference("download.prompt_for_download", false);
                        edgeOptions.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
                    }
                    var edgeDriver = new EdgeDriver(edgeOptions);

                    Log.Information("Edge web driver has been created");

                    return edgeDriver;

                default:
                    throw new ArgumentException($"Unsupported browser: {browserType}");
            }
        }
    }
}
