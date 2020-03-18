dotnet test .\ReadLine.Tests\ReadLine.Tests.csproj
if ($LastExitCode -ne 0) { $host.SetShouldExit($LastExitCode) }