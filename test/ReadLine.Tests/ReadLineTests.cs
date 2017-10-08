using System;
using System.Linq;
using Xunit;

namespace ReadLine.Tests
{
    public class ReadLineTests : IDisposable
    {
        public ReadLineTests()
        {
            string[] history =
            {
                "ls -a",
                "dotnet run",
                "git init"
            };
            ReadLine.AddHistory(history);
        }

        public void Dispose()
        {
            // If all above tests pass
            // clear history works
            ReadLine.ClearHistory();
        }

        [Fact]
        public void TestGetCorrectHistory()
        {
            Assert.Equal("ls -a", ReadLine.GetHistory()[0]);
            Assert.Equal("dotnet run", ReadLine.GetHistory()[1]);
            Assert.Equal("git init", ReadLine.GetHistory()[2]);
        }

        [Fact]
        public void TestNoInitialHistory()
        {
            Assert.Equal(3, ReadLine.GetHistory().Count);
        }

        [Fact]
        public void TestUpdatesHistory()
        {
            ReadLine.AddHistory("mkdir");
            Assert.Equal(4, ReadLine.GetHistory().Count);
            Assert.Equal("mkdir", ReadLine.GetHistory().Last());
        }
    }
}