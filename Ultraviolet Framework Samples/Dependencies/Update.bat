echo off
set DIR=..\..\Binaries\%2\%1

if not exist %DIR% goto notexist

if %2 EQU Android (

	copy "..\..\Dependencies\Newtonsoft.Json.dll" %2\Newtonsoft.Json.dll /Y
	if %errorlevel% GTR 0 goto fail
	copy "%DIR%\TwistedLogik.Gluon.dll" %2\TwistedLogik.Gluon.dll /Y
	if %errorlevel% GTR 0 goto fail
	copy "%DIR%\TwistedLogik.Nucleus.dll" %2\TwistedLogik.Nucleus.dll /Y
	if %errorlevel% GTR 0 goto fail
	copy "%DIR%\TwistedLogik.Ultraviolet.dll" %2\TwistedLogik.Ultraviolet.dll /Y
	if %errorlevel% GTR 0 goto fail
	copy "%DIR%\TwistedLogik.Ultraviolet.OpenGL.dll" %2\TwistedLogik.Ultraviolet.OpenGL.dll /Y
	if %errorlevel% GTR 0 goto fail
	copy "%DIR%\TwistedLogik.Ultraviolet.SDL2.dll" %2\TwistedLogik.Ultraviolet.SDL2.dll /Y
	if %errorlevel% GTR 0 goto fail
	copy "%DIR%\TwistedLogik.Ultraviolet.BASS.dll" %2\TwistedLogik.Ultraviolet.BASS.dll /Y
	if %errorlevel% GTR 0 goto fail
	copy "%DIR%\TwistedLogik.Ultraviolet.UI.Presentation.dll" %2\TwistedLogik.Ultraviolet.UI.Presentation.dll /Y
	if %errorlevel% GTR 0 goto fail

	rem platform compatibility shim
	copy "%DIR%\TwistedLogik.Ultraviolet.%2.dll" %2\TwistedLogik.Ultraviolet.%2.dll /Y
	if %errorlevel% GTR 0 goto fail
	
	rem java bootstrapper
	copy "%DIR%\Org.Libsdl.App.dll" %2\Org.Libsdl.App.dll /Y
	if %errorlevel% GTR 0 goto fail

	copy "%DIR%\TwistedLogik.Nucleus.xml" %2\TwistedLogik.Nucleus.xml /Y > nul
	copy "%DIR%\TwistedLogik.Ultraviolet.xml" %2\TwistedLogik.Ultraviolet.xml /Y > nul
	copy "%DIR%\TwistedLogik.Ultraviolet.OpenGL.xml" %2\TwistedLogik.Ultraviolet.OpenGL.xml /Y > nul
	
) ELSE (

	copy "..\..\Dependencies\Newtonsoft.Json.dll" Newtonsoft.Json.dll /Y
	if %errorlevel% GTR 0 goto fail
	copy "%DIR%\TwistedLogik.Gluon.dll" TwistedLogik.Gluon.dll /Y
	if %errorlevel% GTR 0 goto fail
	copy "%DIR%\TwistedLogik.Nucleus.dll" TwistedLogik.Nucleus.dll /Y
	if %errorlevel% GTR 0 goto fail
	copy "%DIR%\TwistedLogik.Ultraviolet.dll" TwistedLogik.Ultraviolet.dll /Y
	if %errorlevel% GTR 0 goto fail
	copy "%DIR%\TwistedLogik.Ultraviolet.OpenGL.dll" TwistedLogik.Ultraviolet.OpenGL.dll /Y
	if %errorlevel% GTR 0 goto fail
	copy "%DIR%\TwistedLogik.Ultraviolet.SDL2.dll" TwistedLogik.Ultraviolet.SDL2.dll /Y
	if %errorlevel% GTR 0 goto fail
	copy "%DIR%\TwistedLogik.Ultraviolet.BASS.dll" TwistedLogik.Ultraviolet.BASS.dll /Y
	if %errorlevel% GTR 0 goto fail
	copy "%DIR%\TwistedLogik.Ultraviolet.UI.Presentation.dll" TwistedLogik.Ultraviolet.UI.Presentation.dll /Y
	if %errorlevel% GTR 0 goto fail
	
	rem platform compatibility shim
	copy "%DIR%\TwistedLogik.Ultraviolet.Desktop.dll" TwistedLogik.Ultraviolet.Desktop.dll /Y
	if %errorlevel% GTR 0 goto fail

	copy "%DIR%\TwistedLogik.Nucleus.xml" TwistedLogik.Nucleus.xml /Y > nul
	copy "%DIR%\TwistedLogik.Ultraviolet.xml" TwistedLogik.Ultraviolet.xml /Y > nul
	copy "%DIR%\TwistedLogik.Ultraviolet.OpenGL.xml" TwistedLogik.Ultraviolet.OpenGL.xml /Y > nul
	
)

exit 0

:notexist

echo Binary directory %DIR% not found, please 'Rebuild all' in library project first!
exit 1

:fail
echo Copying failed, please try rebuilding everything in the library project.
exit 2