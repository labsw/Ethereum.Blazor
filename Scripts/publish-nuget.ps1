

$solutionPath = "..\Ethereum.Blazor.sln"
$nugetApiKey = "oy2mlxpmvhompqcdyxgaypm7bt4de27una3sbjbapg2yb4"

Write-Host "Build and publish to NuGet"

dotnet build "$solutionPath" -c Release

Write-Host "Complete"
