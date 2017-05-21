param(
    [string]$p1 = "Debug"
)

dotnet restore
dotnet build ".\src\ReadLine" -c $p1
dotnet build ".\src\ReadLine.Demo" -c $p1
dotnet build ".\test\ReadLine.Tests" -c $p1
