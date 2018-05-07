using System;
using System.Collections.Generic;
using System.Linq;
using ReadLine.Abstractions;
using ReadLine.Tests.Abstractions;
using Xunit;
using static ReadLine.Tests.ConsoleKeyInfoExtensions;


namespace ReadLine.Tests
{
    public class KeyHandlerTests
    {
        private KeyHandler _keyHandler;
        private readonly List<string> _history;
        private readonly AutoCompleteHandler _autoCompleteHandler;
        private readonly string[] _completions;
        private readonly IConsole _console;

        public KeyHandlerTests()
        {
            _autoCompleteHandler = new AutoCompleteHandler();
            _completions = _autoCompleteHandler.GetSuggestions("", 0);
            _history = new List<string>(new[] { "dotnet run", "git init", "clear" });

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
            _keyHandler.Handle(Backspace);
            Assert.Equal("Hell", _keyHandler.Text);
        }

        [Fact]
        public void TestDelete()
        {
            new List<ConsoleKeyInfo>() { LeftArrow, Delete }
                .ForEach(_keyHandler.Handle);

            Assert.Equal("Hell", _keyHandler.Text);
        }

        [Fact]
        public void TestDelete_EndOfLine()
        {
            _keyHandler.Handle(Delete);
            Assert.Equal("Hello", _keyHandler.Text);
        }

        [Fact]
        public void TestControlH()
        {
            _keyHandler.Handle(CtrlH);
            Assert.Equal("Hell", _keyHandler.Text);
        }

        [Fact]
        public void TestControlT()
        {
            var initialCursorCol = _console.CursorLeft;
            _keyHandler.Handle(CtrlT);

            Assert.Equal("Helol", _keyHandler.Text);
            Assert.Equal(initialCursorCol, _console.CursorLeft);
        }

        [Fact]
        public void TestControlT_LeftOnce_CursorMovesToEnd()
        {
            var initialCursorCol = _console.CursorLeft;

            new List<ConsoleKeyInfo>() { LeftArrow, CtrlT }
                .ForEach(_keyHandler.Handle);
            
            Assert.Equal("Helol", _keyHandler.Text);
            Assert.Equal(initialCursorCol, _console.CursorLeft);
        }

        [Fact]
        public void TestControlT_CursorInMiddleOfLine()
        {
            Enumerable
                .Repeat(LeftArrow, 3)
                .ToList()
                .ForEach(_keyHandler.Handle);

            var initialCursorCol = _console.CursorLeft;

            _keyHandler.Handle(CtrlT);

            Assert.Equal("Hlelo", _keyHandler.Text);
            Assert.Equal(initialCursorCol + 1, _console.CursorLeft);
        }

        [Fact]
        public void TestControlT_CursorAtBeginningOfLine_HasNoEffect()
        {
            _keyHandler.Handle(CtrlA);

            var initialCursorCol = _console.CursorLeft;

            _keyHandler.Handle(CtrlT);

            Assert.Equal("Hello", _keyHandler.Text);
            Assert.Equal(initialCursorCol, _console.CursorLeft);
        }

        [Fact]
        public void TestHome()
        {
            new List<ConsoleKeyInfo>() { Home, 'S'.ToConsoleKeyInfo() }
                .ForEach(_keyHandler.Handle);

            Assert.Equal("SHello", _keyHandler.Text);
        }

        [Fact]
        public void TestControlA()
        {
            new List<ConsoleKeyInfo>() { CtrlA, 'S'.ToConsoleKeyInfo() }
                .ForEach(_keyHandler.Handle);

            Assert.Equal("SHello", _keyHandler.Text);
        }

        [Fact]
        public void TestEnd()
        {
            new List<ConsoleKeyInfo>() { Home, End, ExclamationPoint }
                .ForEach(_keyHandler.Handle);

            Assert.Equal("Hello!", _keyHandler.Text);
        }

        [Fact]
        public void TestControlE()
        {
            new List<ConsoleKeyInfo>() { CtrlA, CtrlE, ExclamationPoint }
                .ForEach(_keyHandler.Handle);

            Assert.Equal("Hello!", _keyHandler.Text);
        }

        [Fact]
        public void TestLeftArrow()
        {
            " N".Select(c => c.ToConsoleKeyInfo())
                .Prepend(LeftArrow)
                .ToList()
                .ForEach(_keyHandler.Handle);

            Assert.Equal("Hell No", _keyHandler.Text);
        }

        [Fact]
        public void TestControlB()
        {
            " N".Select(c => c.ToConsoleKeyInfo())
                .Prepend(CtrlB)
                .ToList()
                .ForEach(_keyHandler.Handle);

            Assert.Equal("Hell No", _keyHandler.Text);
        }

        [Fact]
        public void TestRightArrow()
        {
            new List<ConsoleKeyInfo>() { LeftArrow, RightArrow, ExclamationPoint }
                .ForEach(_keyHandler.Handle);

            Assert.Equal("Hello!", _keyHandler.Text);
        }

        [Fact]
        public void TestControlD()
        {
            Enumerable.Repeat(LeftArrow, 4)
                    .Append(CtrlD)
                    .ToList()
                    .ForEach(_keyHandler.Handle);

            Assert.Equal("Hllo", _keyHandler.Text);
        }

        [Fact]
        public void TestControlF()
        {
            new List<ConsoleKeyInfo>() { LeftArrow, CtrlF, ExclamationPoint }
                .ForEach(_keyHandler.Handle);

            Assert.Equal("Hello!", _keyHandler.Text);
        }

        [Fact]
        public void TestControlL()
        {
            _keyHandler.Handle(CtrlL);
            Assert.Equal(string.Empty, _keyHandler.Text);
        }

        [Fact]
        public void TestUpArrow()
        {
            _history.AsEnumerable().Reverse().ToList().ForEach((history) => {
                _keyHandler.Handle(UpArrow);
                Assert.Equal(history, _keyHandler.Text);
            });
        }

        [Fact]
        public void TestControlP()
        {
            _history.AsEnumerable().Reverse().ToList().ForEach((history) => {
                _keyHandler.Handle(CtrlP);
                Assert.Equal(history, _keyHandler.Text);
            });
        }

        [Fact]
        public void TestDownArrow()
        {
            Enumerable.Repeat(UpArrow, _history.Count)
                    .ToList()
                    .ForEach(_keyHandler.Handle);

            _history.ForEach( history => {
                Assert.Equal(history, _keyHandler.Text);
                _keyHandler.Handle(DownArrow);
            });
        }

        [Fact]
        public void TestControlN()
        {
            Enumerable.Repeat(UpArrow, _history.Count)
                    .ToList()
                    .ForEach(_keyHandler.Handle);

            _history.ForEach( history => {
                Assert.Equal(history, _keyHandler.Text);
                _keyHandler.Handle(CtrlN);
            });
        }

        [Fact]
        public void TestControlU()
        {
            _keyHandler.Handle(LeftArrow);
            _keyHandler.Handle(CtrlU);

            Assert.Equal("o", _keyHandler.Text);

            _keyHandler.Handle(End);
            _keyHandler.Handle(CtrlU);

            Assert.Equal(string.Empty, _keyHandler.Text);
        }

        [Fact]
        public void TestControlK()
        {
            _keyHandler.Handle(LeftArrow);
            _keyHandler.Handle(CtrlK);

            Assert.Equal("Hell", _keyHandler.Text);

            _keyHandler.Handle(Home);
            _keyHandler.Handle(CtrlK);

            Assert.Equal(string.Empty, _keyHandler.Text);
        }

        [Fact]
        public void TestControlW()
        {
            " World".Select(c => c.ToConsoleKeyInfo())
                    .Append(CtrlW)
                    .ToList()
                    .ForEach(_keyHandler.Handle);

            Assert.Equal("Hello ", _keyHandler.Text);

            _keyHandler.Handle(Backspace);
            _keyHandler.Handle(CtrlW);

            Assert.Equal(string.Empty, _keyHandler.Text);
        }

        [Fact]
        public void TestTab()
        {
            _keyHandler.Handle(Tab);
            // Nothing happens when no auto complete handler is set
            Assert.Equal("Hello", _keyHandler.Text);

            _keyHandler = new KeyHandler(new Console2(), _history, _autoCompleteHandler);

            "Hi ".Select(c => c.ToConsoleKeyInfo()).ToList().ForEach(_keyHandler.Handle);

            _completions.ToList().ForEach(completion => {
                _keyHandler.Handle(Tab);
                Assert.Equal($"Hi {completion}", _keyHandler.Text);
            });
        }

        [Fact]
        public void TestBackwardsTab()
        {
            _keyHandler.Handle(Tab);

            // Nothing happens when no auto complete handler is set
            Assert.Equal("Hello", _keyHandler.Text);

            _keyHandler = new KeyHandler(new Console2(), _history, _autoCompleteHandler);

            "Hi ".Select(c => c.ToConsoleKeyInfo()).ToList().ForEach(_keyHandler.Handle);

            // Bring up the first Autocomplete
            _keyHandler.Handle(Tab);

            _completions.Reverse().ToList().ForEach(completion => {
                _keyHandler.Handle(ShiftTab);
                Assert.Equal($"Hi {completion}", _keyHandler.Text);
            });
        }

        [Fact]
        public void MoveCursorThenPreviousHistory()
        {
            _keyHandler.Handle(LeftArrow);
            _keyHandler.Handle(UpArrow);

            Assert.Equal("clear", _keyHandler.Text);
        }
    }
}