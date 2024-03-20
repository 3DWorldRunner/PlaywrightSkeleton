using S = TestData.Models.PageName_Selectors;

namespace TestData.Models
{
    public static class PageName
    {
        public static async Task NavigateToPage(IPage page)
        {
            TestContext.Out.WriteLine($"Navigating to Google");
            await page.GotoAsync(AppSettings.Current.TestUrl);
            Assert.IsTrue(page.Url == AppSettings.Current.TestUrl, "Did not navigate to Google successfully.");
            TestContext.Out.WriteLine($"Arrived at Google");
        }

        public static async Task VerifyPageComponents(IPage page)
        {
            Assert.IsTrue(await page.LogIsVisibleAsync(nameof(S.searchInput), S.searchInput), $"Expected {nameof(S.searchInput)} to be visible");
        }

    }
}
