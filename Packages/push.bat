@echo off

setlocal EnableDelayedExpansion

if [%BAMBOO_NuGetPush%] == [] (
    echo Skipping publication of NuGet packages...   
) else (

    echo Pushing to NuGet.org...
    set _apikey=%NuGetAPIKey%
    
    for %%f in (*.nupkg) do (
        "nuget.exe" push %%~nf.nupkg -Source https://api.nuget.org/v3/index.json -ApiKey !_apikey!
        @if %errorlevel% neq 0 @exit /b %errorlevel%
    )     
)