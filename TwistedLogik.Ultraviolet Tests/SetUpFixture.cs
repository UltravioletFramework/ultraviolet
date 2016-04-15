using NUnit.Framework;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using TwistedLogik.Ultraviolet.Testing;

[SetUpFixture]
public sealed class SetUpFixture
{
    [OneTimeSetUp]
    public void SetUp()
    {
        Environment.CurrentDirectory = TestContext.CurrentContext.WorkDirectory;

        var imageDir = Path.Combine(Environment.CurrentDirectory, UltravioletTestFramework.GetSanitizedMachineName());
        foreach (var image in Directory.GetFiles(imageDir, "*.png"))
            File.Delete(image);

        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
    }
}
