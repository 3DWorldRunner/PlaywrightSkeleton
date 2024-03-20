
namespace UITests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    internal class UiTests : TestBase
    {

        [Test,
        Category("Smoke"),
        Description("Navigates to a page and checks visibility of a component"),
        Author("JeremiahHall")]
        public async Task NavigateToGoogleAndVerifySearchIsVisible()
        {
            await PageName.NavigateToPage(p.page);
            await PageName.VerifyPageComponents(p.page);
        }

        [Test,
        Category("Smoke"),
        Description("Navigates to a page and checks visibility of a component"),
        Author("JeremiahHall")]
        public async Task NavigateToGoogleAndVerifySearchIsVisibleParallel1()
        {
            await PageName.NavigateToPage(p.page);
            await PageName.VerifyPageComponents(p.page);
        }

        [Test,
        Category("Smoke"),
        Description("Navigates to a page and checks visibility of a component"),
        Author("JeremiahHall")]
        public async Task NavigateToGoogleAndVerifySearchIsVisibleParallel2()
        {
            await PageName.NavigateToPage(p.page);
            await PageName.VerifyPageComponents(p.page);
        }

        [Test,
        Category("Smoke"),
        Description("Navigates to a page and checks visibility of a component"),
        Author("JeremiahHall")]
        public async Task NavigateToGoogleAndVerifySearchIsVisibleParallel3()
        {
            await PageName.NavigateToPage(p.page);
            await PageName.VerifyPageComponents(p.page);
            Assert.Fail("Test screenshot for failed tests");
        }

    }
}