using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

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

                    if (!string.IsNullOrEmpty(downloadsPath))
                    {
                        chromeOptions.AddUserProfilePreference("download.default_directory", downloadsPath);
                        chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
                        chromeOptions.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
                    }

                    var chromeDriver = new ChromeDriver(chromeOptions);

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

                    var firefoxDriver = new FirefoxDriver(firefoxOptions);

                    return firefoxDriver;

                case "edge":
                    var edgeOptions = new EdgeOptions();

                    if (!string.IsNullOrEmpty(downloadsPath))
                    {
                        edgeOptions.AddUserProfilePreference("download.default_directory", downloadsPath);
                        edgeOptions.AddUserProfilePreference("download.prompt_for_download", false);
                        edgeOptions.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
                    }
                    var edgeDriver = new EdgeDriver(edgeOptions);

                    return edgeDriver;

                default:
                    throw new ArgumentException($"Unsupported browser: {browserType}");
            }
        }
    }
}
