param(
    [string]$p1 = "Debug"
)

dotnet restore
dotnet build ".\ReadLine" -c $p1
dotnet build ".\ReadLine.Demo" -c $p1
dotnet build ".\ReadLine.Tests" -c $p1
