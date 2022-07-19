

$solutionPath = "..\Ethereum.Blazor.sln"
$nugetApiKey = "xx"

Write-Host "Build and publish to NuGet"

dotnet build "$solutionPath" -c Release

Write-Host "Complete"
