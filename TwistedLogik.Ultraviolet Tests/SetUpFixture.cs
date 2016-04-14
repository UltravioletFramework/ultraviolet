using NUnit.Framework;
using System;
using System.Globalization;
using System.IO;
using System.Threading;

[SetUpFixture]
public sealed class SetUpFixture
{
    [OneTimeSetUp]
    public void SetUp()
    {
        Environment.CurrentDirectory = TestContext.CurrentContext.WorkDirectory;
        
        foreach (var image in Directory.GetFiles(Environment.CurrentDirectory, "*.png"))
            File.Delete(image);

        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
    }
}
