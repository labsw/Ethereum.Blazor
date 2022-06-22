

$localNugetFolder = "C:\Data\NuGet.Local\"

$etherProviderSource = "C:\Data\repo\Ethereum.Blazor\Ether.BlazorProvider\bin\Release"
$nethereumProviderSource = "C:\Data\repo\Ethereum.Blazor\Ether.NethereumProvider\bin\Release"


Write-Host "Copying $etherProviderSource"
Copy-Item -Path "$etherProviderSource\*.nupkg" -Destination "$localNugetFolder" -Recurse

Write-Host "Copying $nethereumProviderSource"
Copy-Item -Path "$nethereumProviderSource\*.nupkg" -Destination "$localNugetFolder" -Recurse

Write-Host "Complete"
