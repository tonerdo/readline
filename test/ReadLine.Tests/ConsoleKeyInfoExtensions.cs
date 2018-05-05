using System;
using System.Collections.Generic;

namespace ReadLine.Tests
{
    public static class ConsoleKeyInfoExtensions
    {
        public static readonly ConsoleKeyInfo LeftArrow = new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false);
    }
}