# Project-Specific tasks.

Properties {
	$Packages = @("Ncrement", "VSSetup", "Pester");
}

Task "Deploy" -alias "publish" -description "This task compiles, test then publishes the solution." `
-depends @("configure", "version", "msbuild", "mstest", "pack", "push-nuget", "tag");

# ===============

Task "Configure-Project" -alias "configure" -description "This initializes the project." `
-depends @("restore");

Task "Package-Solution" -alias "pack" -description "This task generates all deployment packages." `
-depends @("restore") -action {
	if (Test-Path $ArtifactsDir) { Remove-Item $ArtifactsDir -Recurse -Force; }
	New-Item $ArtifactsDir -ItemType Directory | Out-Null;

	$proj = Join-Path $RootDir "src/$SolutionName/*.csproj" | Get-Item;
	Write-Header "dotnet: pack '$($proj.BaseName)'";
	$version = Get-NcrementManifest $ManifestJson | Convert-NcrementVersionNumberToString $Branch -AppendSuffix;
	Exec { &dotnet pack $proj.FullName --output $ArtifactsDir --configuration $Configuration /p:PackageVersion=$version; }
}