using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests
{
    public partial class SeleniumTester
    {

        [SetUpFixture]
        public class SeleniumChromeDriverSetUp
        {
            public static IWebDriver ChromeDriver { get; private set; }

            [OneTimeSetUp]
            public void SeleniumDriverSetUp()
            {
                ChromeOptions options = new ChromeOptions();
                ChromeDriver = new ChromeDriver(".", options);
            }

            [OneTimeTearDown]
            public void SeleniumDriverTearDown()
            {
                ChromeDriver.Close();
                ChromeDriver.Dispose();
            }
        }

        [Test]
        public void Test1()
        {
            SeleniumChromeDriverSetUp.ChromeDriver.Url = @"https://www.google.pl";
            Assert.Pass();
        }
    }
}