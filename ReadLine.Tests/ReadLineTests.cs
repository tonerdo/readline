using System.Linq;
using NUnit.Framework;


namespace ReadLine.Tests
{
    [TestFixture]
    public class ReadLineTests
    {
        private readonly IReadLine _readLine = ReadLine.Instance;
        private static readonly string[] History = { "ls -a", "dotnet run", "git init" };

        [SetUp]
        public void Initialize() {
            _readLine.AddHistory(History);
        }

        [TearDown]
        public void Destruct() {
            _readLine.ClearHistory();
        }

        [Test]
        public void TestClearHistory() {
            _readLine.ClearHistory();
            Assert.AreEqual(0, _readLine.GetHistory().Count);
        }

        [Test]
        public void TestNoInitialHistory() 
        {
            Assert.AreEqual(3, _readLine.GetHistory().Count);
        }

        [Test]
        public void TestUpdatesHistory() 
        {
            _readLine.AddHistory("mkdir");
            Assert.AreEqual(4, _readLine.GetHistory().Count);
            Assert.AreEqual("mkdir", _readLine.GetHistory().Last());
        }

        [Test]
        public void TestGetCorrectHistory() 
        {
            Assert.AreEqual("ls -a", _readLine.GetHistory()[0]);
            Assert.AreEqual("dotnet run", _readLine.GetHistory()[1]);
            Assert.AreEqual("git init", _readLine.GetHistory()[2]);
        }
    }
}
