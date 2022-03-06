using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace SeleniumTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var options = new ChromeOptions();

            var config = new ConfigurationBuilder().AddJsonFile("jsconfig1.json").Build();

            var section = config.GetSection("environmentvariables");
            string workingdir = section["workingdirectory"].ToString();



            options.AddUserProfilePreference("download.default_directory",workingdir);
            options.AddUserProfilePreference("disable-popup-blocking", "true");

            using (var driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),options))
            {
                
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(@"https://admin:admin!@xxxxx.xxx.xxxxx.com/");
                var elem = new WebDriverWait(driver, TimeSpan.FromSeconds(60)).Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Name("username"))
                 );

                var elem2 = new WebDriverWait(driver, TimeSpan.FromSeconds(60)).Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Name("password"))
                );

                var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(30));
                var clickableElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("button.ng-star-inserted")));

                var username = driver.FindElement(By.Name("username"));
                var password = driver.FindElement(By.Name("password"));
                //var login = driver.FindElement(By.("password"));

               
                username.SendKeys("admin");
                password.SendKeys("admin");
                clickableElement.Click();

                var elem3 = new WebDriverWait(driver, TimeSpan.FromSeconds(60)).Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector("span.menu-item-icon.fal.fa-tablet-rugged"))
                 );

                var wait2 = new WebDriverWait(driver, TimeSpan.FromMinutes(30));
                var clickableElement2 = wait2.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("span.menu-item-icon.fal.fa-tablet-rugged")));

                clickableElement2.Click();

                var wait3 = new WebDriverWait(driver, TimeSpan.FromMinutes(30));
                var clickableElement3 = wait3.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("button.btn.btn-secondary.dropdown-button-split.float-left.ng-star-inserted")));

                clickableElement3.Click();

                var mainWindow = driver.CurrentWindowHandle;

                var js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.open()");

                foreach (var winHandle in driver.WindowHandles) 
                {
                    driver.SwitchTo().Window(winHandle);
                }

                driver.Navigate().GoToUrl("chrome://downloads");

                var js1 = (IJavaScriptExecutor)driver;
                long percentage = (long)0;
                while (percentage != 100)
                {
                    try
                    {
                        percentage = (long)0;
                        String fileName1 = (String)js1.ExecuteScript("return document.querySelector('downloads-manager').shadowRoot.querySelector('#downloadsList downloads-item').shadowRoot.querySelector('div#content #file-link').text");

                        if (Directory.GetFiles(workingdir,fileName1).Length > 0)
                        {
                            percentage = 100;
                        }

                        //percentage = (long) js1.ExecuteScript("return document.querySelector('downloads-manager').shadowRoot.querySelector('#downloadsList downloads-item').items.filter(e => e.state === 'COMPLETE').map(e => e.filePath || e.file_path || e.fileUrl || e.file_url);");
              //System.out.println(percentage);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        // Nothing to do just wait
                    }

                    Thread.Sleep(1000);

                }

                String fileName = (String)js1.ExecuteScript("return document.querySelector('downloads-manager').shadowRoot.querySelector('#downloadsList downloads-item').shadowRoot.querySelector('div#content #file-link').text");

                Console.WriteLine("downloaded filename ==" + fileName);
                driver.Close();
                driver.SwitchTo().Window(mainWindow);


                //var link = driver.FindElement(By.PartialLinkText("TFS Test API"));
                //var jsToBeExecuted = $"window.scroll(0, {link.Location.Y});";
                //((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecuted);
                //var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
                //var clickableElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.PartialLinkText("TFS Test API")));
                //clickableElement.Click();
            }
        }
    }
}
