namespace Utilities
{
    public class AppSettings
    {
        private static AppSettings? _appSettings;

        public string LocalCookieJson { get; set; }
        public string TestEnvironment { get; set; }
        public string TestBrowserPath { get; set; }
        public string TestUrl { get; set; }
        public string TestUserName { get; set; }
        public string TestUserPassword { get; set; }

        public bool Headless { get; set; }
        public int SlowMo { get; set; }

        public AppSettings(IConfiguration config)
        {
            LocalCookieJson = config.GetValue<string>("LocalCookieJson") ?? "";
            TestEnvironment = config.GetValue<string>("Environment") ?? "TEST";
            //If not using playwright binaries, you can specify paths here
            var chromePath1 = Environment.GetEnvironmentVariable("ProgramFiles") + @"\Google\Chrome\Application\chrome.exe";
            var chromePath2 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\Google\Chrome\Application\chrome.exe";
            if (Environment.Is64BitOperatingSystem)
                TestBrowserPath = File.Exists(chromePath1) ? chromePath1 : File.Exists(chromePath2) ? chromePath2 : "";
            else
                TestBrowserPath = File.Exists(chromePath1) ? chromePath1 : "";
            TestUrl = config.GetValue<string>("TestUrl") ?? "";
            TestUserName = config.GetValue<string>("TestUserName") ?? "";
            TestUserPassword = config.GetValue<string>("TestUserPassword") ?? "";
            Headless = bool.Parse(config.GetValue<string>("Headless") ?? "true");
            SlowMo = int.Parse(config.GetValue<string>("SlowMo") ?? "0");

            // Now set Current
            _appSettings = this;
        }

        public static AppSettings Current
        {
            get
            {
                if (_appSettings == null)
                {
                    _appSettings = GetCurrentSettings();
                }

                return _appSettings;
            }
        }

        public static AppSettings GetCurrentSettings()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var settings = new AppSettings(configuration.GetSection("TestSettings"));

            return settings;
        }
    }
}
