using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

using ReadLine.Tests.Abstractions;
using Internal.ReadLine;

using static ReadLine.Tests.CharExtensions;

namespace ReadLine.Tests
{
    public class KeyHandlerTests
    {
        private KeyHandler _keyHandler;
        private List<string> _history;
        private AutoCompleteHandler _autoCompleteHandler;
        private string[] _completions;
        private Internal.ReadLine.Abstractions.IConsole _console;

        public KeyHandlerTests()
        {
            _autoCompleteHandler = new AutoCompleteHandler();
            _completions = _autoCompleteHandler.GetSuggestions("", 0);
            _history = new List<string>(new string[] { "dotnet run", "git init", "clear" });

            _console = new Console2();
            _keyHandler = new KeyHandler(_console, _history, null);

            "Hello".Select(c => c.ToConsoleKeyInfo())
                    .ToList()
                    .ForEach(_keyHandler.Handle);
        }

        [Fact]
        public void TestWriteChar()
        {
            Assert.Equal("Hello", _keyHandler.Text);
            
            " World".Select(c => c.ToConsoleKeyInfo())
                    .ToList()
                    .ForEach(_keyHandler.Handle);
                    
            Assert.Equal("Hello World", _keyHandler.Text);
        }

        [Fact]
        public void TestBackspace()
        {
            var backspace = new ConsoleKeyInfo('\0', ConsoleKey.Backspace, false, false, false);
            _keyHandler.Handle(backspace);

            Assert.Equal("Hell", _keyHandler.Text);
        }

        [Fact]
        public void TestDelete()
        {
            new List<ConsoleKeyInfo>()
            {
                new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false),
                new ConsoleKeyInfo('\0', ConsoleKey.Delete, false, false, false)
            }.ForEach(_keyHandler.Handle);

            Assert.Equal("Hell", _keyHandler.Text);
        }

        [Fact]
        public void TestDelete_EndOfLine()
        {
            var delete = new ConsoleKeyInfo('\0', ConsoleKey.Delete, false, false, false);
            _keyHandler.Handle(delete);

            Assert.Equal("Hello", _keyHandler.Text);
        }

        [Fact]
        public void TestControlH()
        {
            _keyHandler.Handle(CtrlH.ToConsoleKeyInfo());

            Assert.Equal("Hell", _keyHandler.Text);
        }

        [Fact]
        public void TestControlT()
        {
            var initialCursorCol = _console.CursorLeft;
            _keyHandler.Handle(CtrlT.ToConsoleKeyInfo());

            Assert.Equal("Helol", _keyHandler.Text);
            Assert.Equal(initialCursorCol, _console.CursorLeft);
        }

        [Fact]
        public void TestControlT_LeftOnce_CursorMovesToEnd()
        {
            var initialCursorCol = _console.CursorLeft;

            new List<ConsoleKeyInfo>()
            {
                new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false),
                CtrlT.ToConsoleKeyInfo()
            }.ForEach(_keyHandler.Handle);
            
            Assert.Equal("Helol", _keyHandler.Text);
            Assert.Equal(initialCursorCol, _console.CursorLeft);
        }

        [Fact]
        public void TestControlT_CursorInMiddleOfLine()
        {
            var leftArrow = new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false);
            Enumerable
                .Repeat(leftArrow, 3)
                .ToList()
                .ForEach(_keyHandler.Handle);

            var initialCursorCol = _console.CursorLeft;

            _keyHandler.Handle(CtrlT.ToConsoleKeyInfo());

            Assert.Equal("Hlelo", _keyHandler.Text);
            Assert.Equal(initialCursorCol + 1, _console.CursorLeft);
        }

        [Fact]
        public void TestControlT_CursorAtBeginningOfLine_HasNoEffect()
        {
            _keyHandler.Handle(CtrlA.ToConsoleKeyInfo());

            var initialCursorCol = _console.CursorLeft;

            _keyHandler.Handle(CtrlT.ToConsoleKeyInfo());

            Assert.Equal("Hello", _keyHandler.Text);
            Assert.Equal(initialCursorCol, _console.CursorLeft);
        }

        [Fact]
        public void TestHome()
        {
            new List<ConsoleKeyInfo>()
            {
                new ConsoleKeyInfo('\0', ConsoleKey.Home, false, false, false),
                'S'.ToConsoleKeyInfo()
            }.ForEach(_keyHandler.Handle);

            Assert.Equal("SHello", _keyHandler.Text);
        }

        [Fact]
        public void TestControlA()
        {
            new List<ConsoleKeyInfo>() 
            {
                CtrlA.ToConsoleKeyInfo(),
                'S'.ToConsoleKeyInfo()
            }.ForEach(_keyHandler.Handle);

            Assert.Equal("SHello", _keyHandler.Text);
        }

        [Fact]
        public void TestEnd()
        {
            new List<ConsoleKeyInfo>()
            {
                new ConsoleKeyInfo('\0', ConsoleKey.Home, false, false, false),
                new ConsoleKeyInfo('\0', ConsoleKey.End, false, false, false),
                ExclamationPoint.ToConsoleKeyInfo()
            }.ForEach(_keyHandler.Handle);

            Assert.Equal("Hello!", _keyHandler.Text);
        }

        [Fact]
        public void TestControlE()
        {
            new List<ConsoleKeyInfo>()
            {
                CtrlA.ToConsoleKeyInfo(),
                CtrlE.ToConsoleKeyInfo(),
                ExclamationPoint.ToConsoleKeyInfo()
            }.ForEach(_keyHandler.Handle);

            Assert.Equal("Hello!", _keyHandler.Text);
        }

        [Fact]
        public void TestLeftArrow()
        {
            " N".Select(c => c.ToConsoleKeyInfo())
                .Prepend(new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false))
                .ToList()
                .ForEach(_keyHandler.Handle);

            Assert.Equal("Hell No", _keyHandler.Text);
        }

        [Fact]
        public void TestControlB()
        {
            " N".Select(c => c.ToConsoleKeyInfo())
                .Prepend(CtrlB.ToConsoleKeyInfo())
                .ToList()
                .ForEach(_keyHandler.Handle);

            Assert.Equal("Hell No", _keyHandler.Text);
        }

        [Fact]
        public void TestRightArrow()
        {
            new List<ConsoleKeyInfo>()
            {
                new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false),
                new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false),
                ExclamationPoint.ToConsoleKeyInfo()
            }.ForEach(_keyHandler.Handle);

            Assert.Equal("Hello!", _keyHandler.Text);
        }

        [Fact]
        public void TestControlD()
        {
            var leftArrow = new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false);
            Enumerable.Repeat(leftArrow, 4)
                    .Append(CtrlD.ToConsoleKeyInfo())
                    .ToList()
                    .ForEach(_keyHandler.Handle);

            Assert.Equal("Hllo", _keyHandler.Text);
        }

        [Fact]
        public void TestControlF()
        {
            new List<ConsoleKeyInfo>()
            {
                new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false),
                CtrlF.ToConsoleKeyInfo(),
                ExclamationPoint.ToConsoleKeyInfo()
            }.ForEach(_keyHandler.Handle);

            Assert.Equal("Hello!", _keyHandler.Text);
        }

        [Fact]
        public void TestControlL()
        {
            _keyHandler.Handle(CtrlL.ToConsoleKeyInfo());
            Assert.Equal(string.Empty, _keyHandler.Text);
        }

        [Fact]
        public void TestUpArrow()
        {
            _history.AsEnumerable().Reverse().ToList().ForEach((history) => {
                _keyHandler.Handle(new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false));
                Assert.Equal(history, _keyHandler.Text);
            });
        }

        [Fact]
        public void TestControlP()
        {
            _history.AsEnumerable().Reverse().ToList().ForEach((history) => {
                _keyHandler.Handle(CtrlP.ToConsoleKeyInfo());
                Assert.Equal(history, _keyHandler.Text);
            });
        }

        [Fact]
        public void TestDownArrow()
        {
            var upArrow = new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false);
            Enumerable.Repeat(upArrow, _history.Count)
                    .ToList()
                    .ForEach(_keyHandler.Handle);

            var downArrow = new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false);
            _history.ForEach( history => {
                Assert.Equal(history, _keyHandler.Text);
                _keyHandler.Handle(downArrow);
            });
        }

        [Fact]
        public void TestControlN()
        {
            var upArrow = new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false);
            Enumerable.Repeat(upArrow, _history.Count)
                    .ToList()
                    .ForEach(_keyHandler.Handle);

            _history.ForEach( history => {
                Assert.Equal(history, _keyHandler.Text);
                _keyHandler.Handle(CtrlN.ToConsoleKeyInfo());
            });
        }

        [Fact]
        public void TestControlU()
        {
            var leftArrow = new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false);
            _keyHandler.Handle(leftArrow);
            _keyHandler.Handle(CtrlU.ToConsoleKeyInfo());

            Assert.Equal("o", _keyHandler.Text);

            var end = new ConsoleKeyInfo('\0', ConsoleKey.End, false, false, false);
            _keyHandler.Handle(end);
            _keyHandler.Handle(CtrlU.ToConsoleKeyInfo());

            Assert.Equal(string.Empty, _keyHandler.Text);
        }

        [Fact]
        public void TestControlK()
        {
            var leftArrow = new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false);
            _keyHandler.Handle(leftArrow);
            _keyHandler.Handle(CtrlK.ToConsoleKeyInfo());

            Assert.Equal("Hell", _keyHandler.Text);

            var home = new ConsoleKeyInfo('\0', ConsoleKey.Home, false, false, false);
            _keyHandler.Handle(home);
            _keyHandler.Handle(CtrlK.ToConsoleKeyInfo());

            Assert.Equal(string.Empty, _keyHandler.Text);
        }

        [Fact]
        public void TestControlW()
        {
            " World".Append(CtrlW)
                    .Select(c => c.ToConsoleKeyInfo())
                    .ToList()
                    .ForEach(_keyHandler.Handle);

            Assert.Equal("Hello ", _keyHandler.Text);

            var backspace = new ConsoleKeyInfo('\0', ConsoleKey.Backspace, false, false, false);
            _keyHandler.Handle(backspace);
            _keyHandler.Handle(CtrlW.ToConsoleKeyInfo());

            Assert.Equal(string.Empty, _keyHandler.Text);
        }

        [Fact]
        public void TestTab()
        {
             var tab = new ConsoleKeyInfo('\0', ConsoleKey.Tab, false, false, false);
            _keyHandler.Handle(tab);

            // Nothing happens when no auto complete handler is set
            Assert.Equal("Hello", _keyHandler.Text);

            _keyHandler = new KeyHandler(new Console2(), _history, _autoCompleteHandler);

            "Hi ".Select(c => c.ToConsoleKeyInfo()).ToList().ForEach(_keyHandler.Handle);

            _completions.ToList().ForEach(completion => {
                _keyHandler.Handle(tab);
                Assert.Equal($"Hi {completion}", _keyHandler.Text);
            });
        }

        [Fact]
        public void TestBackwardsTab()
        {
            var tab = new ConsoleKeyInfo('\0', ConsoleKey.Tab, false, false, false);
            _keyHandler.Handle(tab);

            // Nothing happens when no auto complete handler is set
            Assert.Equal("Hello", _keyHandler.Text);

            _keyHandler = new KeyHandler(new Console2(), _history, _autoCompleteHandler);

            "Hi ".Select(c => c.ToConsoleKeyInfo()).ToList().ForEach(_keyHandler.Handle);

            // Bring up the first Autocomplete
            tab = new ConsoleKeyInfo('\0', ConsoleKey.Tab, false, false, false);
            _keyHandler.Handle(tab);

            var shiftTab = new ConsoleKeyInfo('\0', ConsoleKey.Tab, true, false, false);
            _completions.Reverse().ToList().ForEach(completion => {
                _keyHandler.Handle(shiftTab);
                Assert.Equal($"Hi {completion}", _keyHandler.Text);
            });
        }

        [Fact]
        public void MoveCursorThenPreviousHistory()
        {
            var leftArrow = new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false);
            _keyHandler.Handle(leftArrow);
            var upArrow = new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false);
            _keyHandler.Handle(upArrow);

            Assert.Equal("clear", _keyHandler.Text);
        }
    }
}
