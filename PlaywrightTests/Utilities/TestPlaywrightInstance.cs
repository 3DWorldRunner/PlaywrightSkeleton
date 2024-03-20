namespace Utilities
{
    public class TestPlaywrightInstance
    {
        public IPlaywright? playwright;
        public IBrowser? browser;
        public IBrowserContext? browserContext;
        public IPage? page;
    }

    public static class PlaywrightExtensions
    {
        public static async Task<bool> LogIsVisibleAsync(this IPage page, string selectorName, string selectorValue, PageIsVisibleOptions? options = null)
        {
            TestContext.Out.WriteLine($"Verifying [{selectorName}] is visible");
            return await page.IsVisibleAsync(selectorValue, options);
        }

        public static async Task LogClickAsync(this IPage page, string selectorName, string selectorValue, PageClickOptions? options = null)
        {
            TestContext.Out.WriteLine($"Clicking [{selectorName}]");
            await page.ClickAsync(selectorValue, options);
        }

        public static async Task LogFillAsync(this IPage page, string selectorName, string selectorValue, string fillValue, PageFillOptions? options = null)
        {
            TestContext.Out.WriteLine($"Fill [{selectorName}] with value [{fillValue}]");
            await page.FillAsync(selectorValue, fillValue, options);
        }

        public static async Task VerifyObjectIsVisibleAndHasExpectedValue(this IPage page, string objectSelector, string objectName, string objectValue)
        {
            TestContext.Out.WriteLine($"Verify [{objectName}] has value [{objectValue}]");
            Assert.IsTrue(await page.LogIsVisibleAsync(objectName, objectSelector), $"Expected {objectName} to be present");
            Assert.IsTrue(await page.InnerTextAsync(objectSelector) == objectValue, $"Expected {objectName} to be {objectValue}");
        }


    }

}
