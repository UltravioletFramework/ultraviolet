using System;
using System.Globalization;
using System.Threading;
using NUnit.Framework;

[SetUpFixture]
public sealed class SetUpFixture
{
    [OneTimeSetUp]
    public void SetUp()
    {
        Environment.CurrentDirectory = TestContext.CurrentContext.WorkDirectory;
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
    }
}
