using System;


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

        public static readonly ConsoleKeyInfo ExclamationPoint = CharExtensions.ExclamationPoint.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo Space = CharExtensions.Space.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlA = CharExtensions.CtrlA.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlB = CharExtensions.CtrlB.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlD = CharExtensions.CtrlD.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlE = CharExtensions.CtrlE.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlF = CharExtensions.CtrlF.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlH = CharExtensions.CtrlH.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlK = CharExtensions.CtrlK.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlL = CharExtensions.CtrlL.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlN = CharExtensions.CtrlN.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlP = CharExtensions.CtrlP.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlT = CharExtensions.CtrlT.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlU = CharExtensions.CtrlU.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlW = CharExtensions.CtrlW.ToConsoleKeyInfo();
    }
}