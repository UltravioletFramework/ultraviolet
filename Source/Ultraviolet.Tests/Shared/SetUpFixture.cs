using System;
using System.Globalization;
using System.IO;
using System.Threading;
using NUnit.Framework;
using Ultraviolet.TestFramework;

[SetUpFixture]
public sealed class SetUpFixture
{
    [OneTimeSetUp]
    public void SetUp()
    {
        Environment.CurrentDirectory = TestContext.CurrentContext.WorkDirectory;

        try
        {
            var imageDir = Path.Combine(Environment.CurrentDirectory, UltravioletTestFramework.GetSanitizedMachineName());
            foreach (var image in Directory.GetFiles(imageDir, "*.png"))
                File.Delete(image);
        }
        catch (DirectoryNotFoundException) { }

        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
    }
}
