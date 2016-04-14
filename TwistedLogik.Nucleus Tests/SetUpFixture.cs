using NUnit.Framework;
using System.Globalization;
using System.Threading;

[SetUpFixture]
public sealed class SetUpFixture
{
    [OneTimeSetUp]
    public void SetUp()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
    }
}
