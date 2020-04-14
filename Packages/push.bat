@echo off

setlocal EnableDelayedExpansion

if [%BAMBOO_NuGetPush%] == [] (
    echo Skipping publication of NuGet packages...   
) else (

    echo Pushing to NuGet.org...
    set _apikey=%NuGetAPIKey%
    
    for %%f in (*.nupkg) do (
        "nuget.exe" push %%~nf.nupkg -ApiKey !_apikey!
        @if %errorlevel% neq 0 @exit /b %errorlevel%
    )
        
    for %%f in (*.snupkg) do (
        "..\nuget.exe" push %%~nf.snupkg -ApiKey !_apikey!
        @if %errorlevel% neq 0 @exit /b %errorlevel%
    )
)