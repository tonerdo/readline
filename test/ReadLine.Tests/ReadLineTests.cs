using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Tests : IDisposable
    {
        public Tests()
        {
            string[] history = new string[] { "ls -a", "dotnet run", "git init" };
            ReadLine.AddHistory(history);
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
        }

        [Fact]
        public void TestGetCorrectHistory() 
        {
            var history = ReadLine.GetHistory();

            Assert.Equal(3, history.Count);
            Assert.Equal("git init", history.Last());
        }

        public void Dispose()
        {
            // If all above tests pass
            // clear history works
            ReadLine.ClearHistory();
        }
    }
}
