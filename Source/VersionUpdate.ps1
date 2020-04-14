$Date = [System.DateTime]::Now

$BuildBranch = $Env:BAMBOO_planRepository_branchName
if ([System.String]::IsNullOrWhiteSpace($BuildBranch)) { $BuildBranch = "" } else { $BuildBranch = " ($BuildBranch)" }

$BuildNumber = $Env:BAMBOO_ultraviolet_buildNumber
if ([System.String]::IsNullOrWhiteSpace($BuildNumber)) { $BuildNumber = "0" }

$BuildConfig = $Env:BAMBOO_buildType
if ([System.String]::IsNullOrWhitespace($BuildConfig)) { $BuildConfig = "Debug" }

$BuildVersion = $Env:BAMBOO_buildVersionString
if ([System.String]::IsNullOrWhiteSpace($BuildVersion)) {
    $BuildVersionStringPath = Resolve-Path -Path "VersionString.txt"
    $BuildVersion = [System.IO.File]::ReadAllText($BuildVersionStringPath)
}

$AsmVersion = [System.String]::Format("$BuildVersion.0", $Date)
$AsmFileVersion = [System.String]::Format("$BuildVersion.$BuildNumber", $Date)
$AsmInfoVersion = $AsmFileVersion
if ([System.String]::IsNullOrWhiteSpace($BuildNumber)) {
    $AsmInfoVersion = "$AsmFileVersion dev build"
}
else {
    $AsmInfoVersion = "$AsmFileVersion $BuildConfig build$BuildBranch"
}

Out-File -FilePath "Version.cs" -InputObject @(
    "using System.Reflection;"
    ""
    "[assembly: AssemblyProduct(`"Ultraviolet Framework`")]"
    "[assembly: AssemblyCopyright(`"Copyright (c) Cole Campbell 2014-$($Date.Year)`")]"
    ""
    "[assembly: AssemblyVersion(`"$AsmVersion`")]"
    "[assembly: AssemblyFileVersion(`"$AsmFileVersion`")]"
    "[assembly: AssemblyInformationalVersion(`"$AsmInfoVersion`")]"
)