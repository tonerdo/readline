using System;
using System.Collections.Generic;
using Xunit;

using ReadLine.Tests.Abstractions;
using Internal.ReadLine;

namespace ReadLine.Tests
{
    public class KeyHandlerTests
    {
        private KeyHandler _keyHandler;
        private ConsoleKeyInfo _keyInfo;
        private List<string> _history;
        private string[] _completions;

        public KeyHandlerTests()
        {
            _completions = new string[] { "World", "Angel", "Love" };
            _history = new List<string>(new string[] { "dotnet run", "git init", "clear" });
            _keyHandler = new KeyHandler(new Console2(), _history, null);

            _keyInfo = new ConsoleKeyInfo('H', ConsoleKey.H, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('e', ConsoleKey.E, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('l', ConsoleKey.L, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('l', ConsoleKey.L, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('o', ConsoleKey.O, false, false, false);
            _keyHandler.Handle(_keyInfo);
        }

        [Fact]
        public void TestWriteChar()
        {
            Assert.Equal("Hello", _keyHandler.Text);

            _keyInfo = new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('W', ConsoleKey.W, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('o', ConsoleKey.O, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('r', ConsoleKey.R, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('l', ConsoleKey.L, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('d', ConsoleKey.D, false, false, false);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal("Hello World", _keyHandler.Text);
        }

        [Fact]
        public void TestBackspace()
        {
            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.Backspace, false, false, false);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal("Hell", _keyHandler.Text);
        }

        [Fact]
        public void TestDelete()
        {
            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false);
            _keyHandler.Handle(_keyInfo);
            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.Delete, false, false, false);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal("Hell", _keyHandler.Text);
        }

        [Fact]
        public void TestDelete_EndOfLine()
        {
            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.Delete, false, false, false);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal("Hello", _keyHandler.Text);
        }

        [Fact]
        public void TestControlH()
        {
            _keyInfo = new ConsoleKeyInfo('H', ConsoleKey.H, false, false, true);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal("Hell", _keyHandler.Text);
        }

        [Fact]
        public void TestHome()
        {
            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.Home, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('S', ConsoleKey.S, false, false, false);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal("SHello", _keyHandler.Text);
        }

        [Fact]
        public void TestControlA()
        {
            _keyInfo = new ConsoleKeyInfo('A', ConsoleKey.A, false, false, true);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('S', ConsoleKey.S, false, false, false);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal("SHello", _keyHandler.Text);
        }

        [Fact]
        public void TestEnd()
        {
            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.Home, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.End, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('!', ConsoleKey.D0, false, false, false);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal("Hello!", _keyHandler.Text);
        }

        [Fact]
        public void TestControlE()
        {
            _keyInfo = new ConsoleKeyInfo('A', ConsoleKey.A, false, false, true);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('E', ConsoleKey.E, false, false, true);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('!', ConsoleKey.D0, false, false, false);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal("Hello!", _keyHandler.Text);
        }

        [Fact]
        public void TestLeftArrow()
        {
            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('N', ConsoleKey.N, false, false, false);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal("Hell No", _keyHandler.Text);
        }

        [Fact]
        public void TestControlB()
        {
            _keyInfo = new ConsoleKeyInfo('B', ConsoleKey.B, false, false, true);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('N', ConsoleKey.N, false, false, false);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal("Hell No", _keyHandler.Text);
        }

        [Fact]
        public void TestRightArrow()
        {
            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('!', ConsoleKey.D0, false, false, false);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal("Hello!", _keyHandler.Text);
        }

        [Fact]
        public void TestControlF()
        {
            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('F', ConsoleKey.F, false, false, true);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('!', ConsoleKey.D0, false, false, false);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal("Hello!", _keyHandler.Text);
        }

        [Fact]
        public void TestControlL()
        {
            _keyInfo = new ConsoleKeyInfo('L', ConsoleKey.L, false, false, true);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal(string.Empty, _keyHandler.Text);
        }

        [Fact]
        public void TestUpArrow()
        {
            for (int i = _history.Count - 1; i >= 0 ; i--)
            {
                _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false);
                _keyHandler.Handle(_keyInfo);

                Assert.Equal(_history[i], _keyHandler.Text);
            }
        }

        [Fact]
        public void TestControlP()
        {
            for (int i = _history.Count - 1; i >= 0 ; i--)
            {
                _keyInfo = new ConsoleKeyInfo('P', ConsoleKey.P, false, false, true);
                _keyHandler.Handle(_keyInfo);
                
                Assert.Equal(_history[i], _keyHandler.Text);
            }
        }

        [Fact]
        public void TestDownArrow()
        {
            for (int i = _history.Count - 1; i >= 0 ; i--)
            {
                _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false);
                _keyHandler.Handle(_keyInfo);
            }

            for (int i = 0; i < _history.Count; i++)
            {
                Assert.Equal(_history[i], _keyHandler.Text);

                _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false);
                _keyHandler.Handle(_keyInfo);
            }
        }

        [Fact]
        public void TestControlN()
        {
            for (int i = _history.Count - 1; i >= 0 ; i--)
            {
                _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false);
                _keyHandler.Handle(_keyInfo);
            }

            for (int i = 0; i < _history.Count; i++)
            {
                Assert.Equal(_history[i], _keyHandler.Text);
                
                _keyInfo = new ConsoleKeyInfo('N', ConsoleKey.N, false, false, true);
                _keyHandler.Handle(_keyInfo);
            }
        }

        [Fact]
        public void TestControlU()
        {
            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('U', ConsoleKey.U, false, false, true);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal("o", _keyHandler.Text);

            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.End, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('U', ConsoleKey.U, false, false, true);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal(string.Empty, _keyHandler.Text);
        }

        [Fact]
        public void TestControlK()
        {
            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('K', ConsoleKey.K, false, false, true);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal("Hell", _keyHandler.Text);

            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.Home, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('K', ConsoleKey.K, false, false, true);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal(string.Empty, _keyHandler.Text);
        }

        [Fact]
        public void TestControlW()
        {
            _keyInfo = new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('W', ConsoleKey.W, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('o', ConsoleKey.O, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('r', ConsoleKey.R, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('l', ConsoleKey.L, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('d', ConsoleKey.D, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('W', ConsoleKey.W, false, false, true);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal("Hello ", _keyHandler.Text);

            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.Backspace, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('W', ConsoleKey.W, false, false, true);
            _keyHandler.Handle(_keyInfo);

            Assert.Equal(string.Empty, _keyHandler.Text);
        }

        [Fact]
        public void TestTab()
        {
            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.Tab, false, false, false);
            _keyHandler.Handle(_keyInfo);

            // Nothing happens when no auto complete handler is set
            Assert.Equal("Hello", _keyHandler.Text);

            _keyHandler = new KeyHandler(new Console2(), _history, (t, s) => _completions);

            _keyInfo = new ConsoleKeyInfo('H', ConsoleKey.H, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('i', ConsoleKey.I, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
            _keyHandler.Handle(_keyInfo);

            for (int i = 0; i < _completions.Length; i++)
            {
                _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.Tab, false, false, false);
                _keyHandler.Handle(_keyInfo);

                Assert.Equal($"Hi {_completions[i]}", _keyHandler.Text);
            }
        }

        [Fact]
        public void TestBackwardsTab()
        {
            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.Tab, false, false, false);
            _keyHandler.Handle(_keyInfo);

            // Nothing happens when no auto complete handler is set
            Assert.Equal("Hello", _keyHandler.Text);

            _keyHandler = new KeyHandler(new Console2(), _history, (t, s) => _completions);

            _keyInfo = new ConsoleKeyInfo('H', ConsoleKey.H, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo('i', ConsoleKey.I, false, false, false);
            _keyHandler.Handle(_keyInfo);

            _keyInfo = new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
            _keyHandler.Handle(_keyInfo);

            // Bring up the first Autocomplete
            _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.Tab, false, false, false);
            _keyHandler.Handle(_keyInfo);

            for (int i = _completions.Length - 1; i >= 0; i--)
            {
                _keyInfo = new ConsoleKeyInfo('\0', ConsoleKey.Tab, true, false, false);
                _keyHandler.Handle(_keyInfo);

                Assert.Equal($"Hi {_completions[i]}", _keyHandler.Text);
            }
        }
    }
}
