using System;
using System.Collections.Generic;

namespace ReadLine.Tests
{
    public static class ConsoleKeyInfoExtensions
    {
        public static readonly ConsoleKeyInfo Backspace = new ConsoleKeyInfo('\0', ConsoleKey.Backspace, false, false, false);
        public static readonly ConsoleKeyInfo Delete = new ConsoleKeyInfo('\0', ConsoleKey.Delete, false, false, false);

        public static readonly ConsoleKeyInfo Home = new ConsoleKeyInfo('\0', ConsoleKey.Home, false, false, false);
        public static readonly ConsoleKeyInfo End = new ConsoleKeyInfo('\0', ConsoleKey.End, false, false, false);

        public static readonly ConsoleKeyInfo LeftArrow = new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false);
        public static readonly ConsoleKeyInfo RightArrow = new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false);
        public static readonly ConsoleKeyInfo UpArrow = new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false);
        public static readonly ConsoleKeyInfo DownArrow = new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false);

        public static readonly ConsoleKeyInfo Tab = new ConsoleKeyInfo('\0', ConsoleKey.Tab, false, false, false);
        public static readonly ConsoleKeyInfo ShiftTab = new ConsoleKeyInfo('\0', ConsoleKey.Tab, true, false, false);
    }
}