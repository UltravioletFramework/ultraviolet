@echo off

setlocal EnableDelayedExpansion

if [%BAMBOO_NuGetPush%] == [] (
    echo Skipping publication of NuGet packages...   
) else (

    if [%BAMBOO_NuGetPush%] == [myget] (
        echo Pushing to MyGet.org...
        set _apikey=%MyGetAPIKey%
        
    ) else (
        echo Pushing to NuGet.org...
        set _apikey=%NuGetAPIKey%
    )
    
    for %%f in (*.nupkg) do (
        "nuget.exe" push %%~nf.nupkg -Source %BAMBOO_NuGetPackageSource% -ApiKey !_apikey!
    )
    
    cd Symbols
    
    for %%f in (*.nupkg) do (
        "..\nuget.exe" push %%~nf.nupkg -Source %BAMBOO_NuGetSymbolsSource% -ApiKey !_apikey!
    )
)