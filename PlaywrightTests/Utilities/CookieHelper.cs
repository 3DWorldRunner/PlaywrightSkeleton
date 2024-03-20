namespace Utilities
{
    public static class CookieHelper
    {
        public static List<Cookie> Cookies = new List<Cookie>();

        public static IEnumerable<Cookie> GetCookies()
        {
            if (Cookies.Count > 0)
                return Cookies;

            string _cookieJson = AppSettings.Current.LocalCookieJson; 
            Cookies = JsonConvert.DeserializeObject<List<Cookie>>(_cookieJson) ?? new List<Cookie>();
            TestContext.Out.WriteLine($"Found {Cookies.Count} cookies");
            return Cookies;
        }

        public static void SetCookies()
        {
            //Hack to call async playwright methods from synchronous OneTimeSetup
            var task = SetCookiesAsync();
            task.WaitAndUnwrapException();
        }

        /// <summary>
        /// This method was created for single user testing
        /// You could create new Cookie collections for each user and pass them in when appropriate
        /// If you have a large amount of test users, you'll likely need a different approach
        /// </summary>
        /// <returns></returns>
        public static async Task SetCookiesAsync()
        {
            TestPlaywrightInstance p = new();
            TestContext.Out.WriteLine("Configuring Playwright Cookies");
            p.playwright = await Playwright.CreateAsync();
            TestContext.Out.WriteLine("Launching Browser");
            p.browser = await p.playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = AppSettings.Current.Headless, SlowMo = AppSettings.Current.SlowMo, ExecutablePath = AppSettings.Current.TestBrowserPath });
            p.browserContext = await p.browser.NewContextAsync();
            p.page = await p.browserContext.NewPageAsync();

            try
            {
                ////This is a Microsoft Entra login, can be replaced if you have other needs.
                //await p.page.GotoAsync(AppSettings.Current.TestUrl);
                //await p.page.FillAsync("#i0116", AppSettings.Current.TestUserName);
                //await p.page.ClickAsync("#idSIButton9");
                //await p.page.FillAsync("#i0118", AppSettings.Current.TestUserPassword);
                //await p.page.ClickAsync("#idSIButton9");
                //await p.page.WaitForURLAsync(AppSettings.Current.TestUrl);
            }
            catch (Exception)
            {
                var sspath = TestContext.CurrentContext.WorkDirectory + '/' + TestContext.CurrentContext.Test.Name + ".png";
                await p.page.ScreenshotAsync(new PageScreenshotOptions { Path = sspath });
                TestContext.AddTestAttachment(sspath);
                throw;
            }

            var cookies = await p.page.Context.CookiesAsync();
            if (cookies.Count == 0)
            {
                Thread.Sleep(3000);
                await p.page.ReloadAsync();
                Thread.Sleep(3000);
                cookies = await p.page.Context.CookiesAsync();
                if (cookies.Count == 0)
                    Assert.Inconclusive("Failed to login and create valid cookies for this test session");
            }

            foreach (var cookieResult in cookies)
            {

                var cookie = new Cookie
                {
                    Name = cookieResult.Name,
                    Value = cookieResult.Value,
                    Domain = cookieResult.Domain,
                    Path = cookieResult.Path,
                    Expires = cookieResult.Expires,
                    HttpOnly = cookieResult.HttpOnly,
                    Secure = cookieResult.Secure,
                    SameSite = cookieResult.SameSite,
                };
                Cookies.Add(cookie);
            }
        }

    }
}
