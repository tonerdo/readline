dotnet test .\test\ReadLine.Tests
if ($LastExitCode -ne 0) { $host.SetShouldExit($LastExitCode) }