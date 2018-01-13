@ECHO OFF

ECHO.

IF "%TLSN%"=="" (
	ECHO Strong name key location not specified, strong naming skipped.
	GOTO :eof
)
ECHO Found strong name key: 
ECHO %TLSN%
ECHO.
ECHO Strong naming assemblies...

IF "%1"=="sign_android" GOTO sign_android
IF "%1"=="sign_ios" GOTO sign_ios
IF "%1"=="sign_netcore" GOTO sign_netcore
IF "%1"=="test_nucleus" GOTO test_nucleus
IF "%1"=="test_ultraviolet" GOTO test_ultraviolet
IF "%1"=="test_uvss" GOTO test_uvss

sn.exe -R "Ultraviolet.Core.dll" "%TLSN%" 
sn.exe -R "Ultraviolet.Core.Design.dll" "%TLSN%"
sn.exe -R "Ultraviolet.OpenGL.Bindings.dll" "%TLSN%" 
sn.exe -R "Ultraviolet.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Presentation.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Presentation.Uvss.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Presentation.Compiler.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Presentation.RoslynCompiler.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Design.dll" "%TLSN%"
sn.exe -R "Ultraviolet.OpenGL.dll" "%TLSN%"
sn.exe -R "Ultraviolet.SDL2.dll" "%TLSN%"
sn.exe -R "Ultraviolet.BASS.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Windows.Forms.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Shims.Desktop.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Shims.macOS.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Tooling.dll" "%TLSN%"
sn.exe -R "uvfont.exe" "%TLSN%"
sn.exe -R "uvarchive.exe" "%TLSN%"
GOTO :eof

:sign_android
sn.exe -R "Ultraviolet.Core.dll" "%TLSN%" 
sn.exe -R "Ultraviolet.OpenGL.Bindings.dll" "%TLSN%" 
sn.exe -R "Ultraviolet.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Presentation.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Presentation.Uvss.dll" "%TLSN%"
sn.exe -R "Ultraviolet.OpenGL.dll" "%TLSN%"
sn.exe -R "Ultraviolet.SDL2.dll" "%TLSN%"
sn.exe -R "Ultraviolet.BASS.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Shims.Android.dll" "%TLSN%"
GOTO :eof

:sign_ios
sn.exe -R "Ultraviolet.Core.dll" "%TLSN%" 
sn.exe -R "Ultraviolet.OpenGL.Bindings.dll" "%TLSN%" 
sn.exe -R "Ultraviolet.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Presentation.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Presentation.Uvss.dll" "%TLSN%"
sn.exe -R "Ultraviolet.OpenGL.dll" "%TLSN%"
sn.exe -R "Ultraviolet.SDL2.dll" "%TLSN%"
sn.exe -R "Ultraviolet.SDL2.UIKit.dll" "%TLSN%"
sn.exe -R "Ultraviolet.BASS.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Shims.iOS.dll" "%TLSN%"
GOTO :eof

:sign_netcore
sn.exe -R "Ultraviolet.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Core.dll" "%TLSN%" 
sn.exe -R "Ultraviolet.OpenGL.dll" "%TLSN%"
sn.exe -R "Ultraviolet.OpenGL.Bindings.dll" "%TLSN%" 
sn.exe -R "Ultraviolet.Presentation.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Presentation.Uvss.dll" "%TLSN%"
sn.exe -R "Ultraviolet.SDL2.dll" "%TLSN%"
sn.exe -R "Ultraviolet.BASS.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Shims.NETCore.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Tooling.dll" "%TLSN%"
GOTO :eof

:test_nucleus
sn.exe -R "Ultraviolet.Core.dll" "%TLSN%" 
GOTO :eof

:test_ultraviolet
sn.exe -R "Ultraviolet.Core.dll" "%TLSN%" 
sn.exe -R "Ultraviolet.OpenGL.Bindings.dll" "%TLSN%" 
sn.exe -R "Ultraviolet.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Presentation.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Presentation.Uvss.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Presentation.Compiler.dll" "%TLSN%"
sn.exe -R "Ultraviolet.OpenGL.dll" "%TLSN%"
sn.exe -R "Ultraviolet.SDL2.dll" "%TLSN%"
sn.exe -R "Ultraviolet.BASS.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Shims.Desktop.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Core.TestFramework.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Core.Tests.dll" "%TLSN%"
sn.exe -R "Ultraviolet.TestFramework.dll" "%TLSN%"
sn.exe -R "Ultraviolet.Tests.dll" "%TLSN%"
GOTO :eof

:test_uvss
sn.exe -R "Ultraviolet.Core.dll" "%TLSN%" 
sn.exe -R "Ultraviolet.Presentation.Uvss.dll" "%TLSN%"
GOTO :eof