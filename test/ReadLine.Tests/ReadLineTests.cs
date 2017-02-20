using System;
using System.Linq;
using Xunit;

using static System.ReadLine;

namespace ReadLine.Tests
{
    public class ReadLineTests : IDisposable
    {
        public ReadLineTests()
        {
            string[] history = new string[] { "ls -a", "dotnet run", "git init" };
            History.Add(history);
        }

        [Fact]
        public void TestNoInitialHistory() 
        {
            Assert.Equal(3, History.Count);
        }

        [Fact]
        public void TestUpdatesHistory() 
        {
            History.Add("mkdir");
            Assert.Equal(4, History.Count);
            Assert.Equal("mkdir", History.Last());
        }

        [Fact]
        public void TestGetCorrectHistory() 
        {
            Assert.Equal("ls -a", History[0]);
            Assert.Equal("dotnet run", History[1]);
            Assert.Equal("git init", History[2]);
        }

        public void Dispose()
        {
            // If all above tests pass
            // clear history works
            History.Clear();
        }
    }
}
