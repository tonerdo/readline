[![Windows build status](https://ci.appveyor.com/api/projects/status/github/tonerdo/readline?branch=master&svg=true)](https://ci.appveyor.com/project/tonerdo/readline)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![NuGet version](https://badge.fury.io/nu/ReadLine.svg)](https://www.nuget.org/packages/ReadLine)
# ReadLine

ReadLine is a [GNU Readline](https://en.wikipedia.org/wiki/GNU_Readline) like library built in pure C#. It can serve as a drop in replacement for the inbuilt `Console.ReadLine()` and brings along
with it some of the terminal goodness you get from unix shells, like command history navigation and tab auto completion.

It is cross platform and runs anywhere .NET is supported, targeting `netstandard1.3` means that it can be used with .NET Core as well as the full .NET Framework.

## Shortcut Guide

| Shortcut                       | Comment                           |
| ------------------------------ | --------------------------------- |
| `Ctrl`+`A` / `HOME`            | Beginning of line                 |
| `Ctrl`+`B` / `←`               | Backward one character            |
| `Ctrl`+`C`                     | Send EOF                          |
| `Ctrl`+`E` / `END`             | End of line                       |
| `Ctrl`+`F` / `→`               | Forward one character             |
| `Ctrl`+`H` / `Backspace`       | Delete previous character         |
| `Tab`                          | Command line completion           |
| `Shift`+`Tab`                  | Backwards command line completion |
| `Ctrl`+`J` / `Enter`           | Line feed                         |
| `Ctrl`+`K`                     | Cut text to the end of line       |
| `Ctrl`+`L`                     | Clear line                        |
| `Ctrl`+`M`                     | Same as Enter key                 |
| `Ctrl`+`N` / `↓`               | Forward in history                |
| `Ctrl`+`P` / `↑`               | Backward in history               |
| `Ctrl`+`U`                     | Cut text to the start of line     |
| `Ctrl`+`W`                     | Cut previous word                 |
| `Backspace`                    | Delete previous character         |


## Usage

### Add ReadLine as a dependency

#### .NET Core CLI

```bash
dotnet add package ReadLine
```
#### Package Manager Console

```powershell
Install-Package ReadLine
```

### Read input from the console

```csharp
string input = ReadLine.Read("(prompt)> ");
```

### Read password from the console

```csharp
ReadLine.PasswordMode = true;
string password = ReadLine.Read("(prompt)> ");
```

_Note: The `(prompt>)` is  optional_

### History management

```csharp
// Get command history
ReadLine.GetHistory();

// Add command to history
ReadLine.AddHistory("dotnet run");

// Clear history
ReadLine.ClearHistory();
```

_Note: History information is persisted for an entire application session. Also, calls to `ReadLine.Read()` automatically adds the console input to history_

### Auto-Completion

```csharp
// t:string - The current text entered in the console
// s:int - The index of the command fragment that needs to be completed
// returns string[]
ReadLine.AutoCompletionHandler = (t, s) =>
{
    string[] suggestions = /* logic to generate suggestions */;
    // return string array of suggestions 
    // or null if no suggestions are available
    return suggestions;
};
```

_Note: If no "AutoCompletionHandler" is set, tab autocompletion will be disabled_

### Initial Buffer

```csharp
string input = ReadLine.Read("(prompt)> ", "", "initial");
```

The editing session will begin with "initial" in the readline buffer.

### Background Processing

```csharp
ReadLine.CheckInterrupt = () =>
{
    bool interrupt = /* whether to interrupt the readline */
    return interrupt;
}
ReadLine.InterruptInterval = 1000; /* milliseconds */
```

Every `InterruptInterval` milliseconds, the library will silently run the `CheckInterrupt()` function. If this returns true, the read operation will stop immediately and return what the user has typed so far.

The `CheckInterrupt()` function should not print anything, as this will be mixed in with the editing line. Do your printing after the read operation stops.

If you want to know whether the operation was interrupted:

```csharp
ReadLine.ReadLineResult info = ReadLine.ReadExt("(prompt)> ");
```

`info.Result` is the string result. `info.Interrupted` is a bool indicating whether the operation was stopped by the interrupt routine or by the user hitting Enter.

By combining background processing and the initial buffer option in a loop, you can achieve the effect of an interruption which prints something and then resumes editing. See the [TimerDemo.cs](src/ReadLine.Demo/TimerDemo.cs) demo.

## Contributing

Contributions are highly welcome. If you have found a bug or if you have a feature request, please report them at this repository issues section.

Things you can help with:
* Achieve better command parity with [GNU Readline](https://en.wikipedia.org/wiki/GNU_Readline).
* Add more test cases.

## License

This project is licensed under the MIT license. See the [LICENSE](LICENSE) file for more info.
