﻿using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

using static System.ReadLine;

namespace ReadLine.Tests
{
    public class ReadLineTests : IDisposable
    {
        public ReadLineTests()
        {
            var history = new List<string>() { "ls -a", "dotnet run", "git init" };
            AddHistory(history);
        }

        [Fact]
        public void TestNoInitialHistory() 
        {
            Assert.Equal(3, GetHistory().Count);
        }

        [Fact]
        public void TestUpdatesHistory() 
        {
            AddHistory("mkdir");
            Assert.Equal(4, GetHistory().Count);
            Assert.Equal("mkdir", GetHistory().Last());
        }

        [Fact]
        public void TestGetCorrectHistory() 
        {
            Assert.Equal("ls -a", GetHistory()[0]);
            Assert.Equal("dotnet run", GetHistory()[1]);
            Assert.Equal("git init", GetHistory()[2]);
        }

        public void Dispose()
        {
            // If all above tests pass
            // clear history works
            ClearHistory();
        }
    }
}
