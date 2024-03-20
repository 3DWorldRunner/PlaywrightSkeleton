
namespace Utilities
{
    public class TestBase
    {

        private static readonly AsyncLocal<TestPlaywrightInstance> playwrightInstances = new AsyncLocal<TestPlaywrightInstance>();

        public static TestPlaywrightInstance p
        {
            get => playwrightInstances.Value;

            set => playwrightInstances.Value = value;
        }

        public static void Start(TestPlaywrightInstance p)
        {
            playwrightInstances.Value = p;
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            //Enable the following lines if for some reason you need the playwright binaries installed
            //var exitCode = Program.Main(new[] { "install" });
            //Assert.That(exitCode, Is.EqualTo(0), "Playwright did not install correctly");
            
            //Enable the following check if a specific chrome binary needs to exist
            //Assert.IsTrue(AppSettings.Current.TestBrowserPath != "", "Did not detect chrome exe in program files");
            
            //Enable the following if you want to generate cookie data from a session / login prior to tests to use for all of them
            //if(AppSettings.Current.TestEnvironment != "local") 
            //    CookieHelper.SetCookies();
        }

        [SetUp]
        public void Setup() 
        {
            //Hack to call async playwright methods from synchronous NUnit Setup 
            var playwrightSetup = CreatePlaywrightInstance();
            var p = playwrightSetup.WaitAndUnwrapException();
            Start(p);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != NUnit.Framework.Interfaces.TestStatus.Passed)
            {
                var takeScreenshot = TakeScreenshot(p.page);
                takeScreenshot.WaitAndUnwrapException();
            }
        }

        public async Task<TestPlaywrightInstance> CreatePlaywrightInstance()
        {
            TestPlaywrightInstance p = new();
            TestContext.Out.WriteLine("Configuring Playwright");
            p.playwright = await Playwright.CreateAsync();
            TestContext.Out.WriteLine("Launching Browser");
            //Uses specified chrome
            //p.browser = await p.playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = AppSettings.Current.Headless, SlowMo = AppSettings.Current.SlowMo, ExecutablePath = AppSettings.Current.TestBrowserPath });
            //Uses playwright binaries
            p.browser = await p.playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = AppSettings.Current.Headless, SlowMo = AppSettings.Current.SlowMo });
            p.browserContext = await p.browser.NewContextAsync();
            TestContext.Out.WriteLine("Injecting Cookies");
            //You'll need to add cookie data to the appSettings file or configure SetCookies for this to do anything
            await p.browserContext.AddCookiesAsync(CookieHelper.GetCookies());
            p.page = await p.browserContext.NewPageAsync();
            Assert.IsNotNull(p.page, "Playwright failed to initialize");
            TestContext.Out.WriteLine("Ready To Test");

            return p;
        }

        public async Task TakeScreenshot(IPage page) 
        {
            TestContext.Out.WriteLine("Test Failed - Attempting to take screenshot");
            var sspath = TestContext.CurrentContext.WorkDirectory + '/' + TestContext.CurrentContext.Test.Name + ".png";
            await page.ScreenshotAsync(new PageScreenshotOptions { Path = sspath });
            TestContext.AddTestAttachment(sspath);
            TestContext.Out.WriteLine($"Screenshot saved to {sspath} and attached to test results");
        }
    }
}
