# Project-Specific tasks.

Properties {
	#$MigrationDirectory = Resolve-Path $RootDir "src/*/*Migrations" -as [string];
}

Task "Deploy" -alias "publish" -description "This task compiles, test then publishes the solution." `
-depends @("restore", "version", "compile", "test", "pack", "push-nuget", "push-db", "push-web", "push-ps", "tag");

Task "Configure-Project" -alias "configure" -description "This initializes the project." `
-depends @("restore") -action {
    [string]$sln = Resolve-Path "$RootDir/*.sln";
    Write-Header "dotnet restore";
    Exec { &dotnet restore $sln; }
}

Task "Package-Solution" -alias "pack" -description "This task generates all delployment packages." `
-depends @("restore") -action {
	if (Test-Path $ArtifactsDir) { Remove-Item $ArtifactsDir -Recurse -Force; }
	New-Item $ArtifactsDir -ItemType Directory | Out-Null;
	
	$proj = Join-Path $RootDir "src/$SolutionName/*.csproj" | Get-Item;
	Write-Header "dotnet: pack '$($proj.BaseName)'";
	$version = Get-NcrementManifest $ManifestJson | Convert-NcrementVersionNumberToString $Branch -AppendSuffix;
	Exec { &dotnet pack $proj.FullName --output $ArtifactsDir --configuration $Configuration /p:PackageVersion=$version; }

	[string]$nupkg = Join-Path $ArtifactsDir "*.nupkg" | Resolve-Path;
	$zip = [IO.Path]::ChangeExtension($nupkg, ".zip");
	Copy-Item $nupkg -Destination $zip -Force;
	Expand-Archive $zip -DestinationPath "$ArtifactsDir/msbuild-test";
	Remove-Item $zip -Force;
}