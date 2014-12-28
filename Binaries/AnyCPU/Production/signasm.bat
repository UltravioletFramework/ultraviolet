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

sn.exe -R "TwistedLogik.Nucleus.dll" "%TLSN%" 
sn.exe -R "TwistedLogik.Nucleus.Design.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Gluon.dll" "%TLSN%" 
sn.exe -R "TwistedLogik.Ultraviolet.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.Design.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.OpenGL.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.SDL2.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.BASS.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.WindowsForms.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.Desktop.dll" "%TLSN%"
sn.exe -R "TwistedLogik.Ultraviolet.Tooling.dll" "%TLSN%"
sn.exe -R "uvfont.exe" "%TLSN%"
sn.exe -R "uvarchive.exe" "%TLSN%"
