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
IF "%1"=="test_nucleus" GOTO test_nucleus
IF "%1"=="test_ultraviolet" GOTO test_ultraviolet
IF "%1"=="test_uvss" GOTO test_uvss

sn.exe -R "Ultraviolet.Core.dll" "%TLSN%" 
sn.exe -R "Ultraviolet.Core.Design.dll" "%TLSN%"
sn.exe -R "Ultraviolet.OpenGL.Bindings.dll" "%TLSN%" 
sn.exe -R "TwistedLogik.Ultraviolet.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.UI.Presentation.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.UI.Presentation.Uvss.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.UI.Presentation.Compiler.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.UI.Presentation.RoslynCompiler.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.Design.dll" "%TLSN%"
sn.exe -R "Ultraviolet.OpenGL.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.SDL2.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.BASS.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.WindowsForms.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.Desktop.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.OSX.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.Tooling.dll" "%TLSN%"
sn.exe -R "uvfont.exe" "%TLSN%"
sn.exe -R "uvarchive.exe" "%TLSN%"
GOTO :eof

:sign_android
sn.exe -R "Ultraviolet.Core.dll" "%TLSN%" 
sn.exe -R "Ultraviolet.OpenGL.Bindings.dll" "%TLSN%" 
sn.exe -R "TwistedLogik.Ultraviolet.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.UI.Presentation.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.UI.Presentation.Uvss.dll" "%TLSN%"
sn.exe -R "Ultraviolet.OpenGL.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.SDL2.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.BASS.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.Android.dll" "%TLSN%"
GOTO :eof

:sign_ios
sn.exe -R "Ultraviolet.Core.dll" "%TLSN%" 
sn.exe -R "Ultraviolet.OpenGL.Bindings.dll" "%TLSN%" 
sn.exe -R "TwistedLogik.Ultraviolet.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.UI.Presentation.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.UI.Presentation.Uvss.dll" "%TLSN%"
sn.exe -R "Ultraviolet.OpenGL.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.SDL2.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.SDL2.UIKit.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.BASS.dll" "%TLSN%"
GOTO :eof

:test_nucleus
sn.exe -R "Ultraviolet.Core.dll" "%TLSN%" 
GOTO :eof

:test_ultraviolet
sn.exe -R "Ultraviolet.Core.dll" "%TLSN%" 
sn.exe -R "Ultraviolet.OpenGL.Bindings.dll" "%TLSN%" 
sn.exe -R "TwistedLogik.Ultraviolet.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.UI.Presentation.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.UI.Presentation.Uvss.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.UI.Presentation.Compiler.dll" "%TLSN%"
sn.exe -R "Ultraviolet.OpenGL.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.SDL2.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.BASS.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.Desktop.dll" "%TLSN%"
GOTO :eof

:test_uvss
sn.exe -R "Ultraviolet.Core.dll" "%TLSN%" 
sn.exe -R "TwistedLogik.Ultraviolet.UI.Presentation.Uvss.dll" "%TLSN%"
GOTO :eof