echo off
set DIR=..\..\Binaries\%2\%1

if not exist %DIR% goto notexist

xcopy "%DIR%\Newtonsoft.Json.dll" Newtonsoft.Json.dll /Y
if %errorlevel% GTR 0 goto fail
xcopy "%DIR%\TwistedLogik.Gluon.dll" TwistedLogik.Gluon.dll /Y
if %errorlevel% GTR 0 goto fail
xcopy "%DIR%\TwistedLogik.Nucleus.dll" TwistedLogik.Nucleus.dll /Y
if %errorlevel% GTR 0 goto fail
xcopy "%DIR%\TwistedLogik.Nucleus.xml" TwistedLogik.Nucleus.xml /Y
rem this doesn't seem to exist in debug builds, but everything still builds. is it necessary?
if %errorlevel% GTR 0 echo "Could not copy!"
xcopy "%DIR%\TwistedLogik.Ultraviolet.dll" TwistedLogik.Ultraviolet.dll /Y
if %errorlevel% GTR 0 goto fail
xcopy "%DIR%\TwistedLogik.Ultraviolet.xml" TwistedLogik.Ultraviolet.xml /Y
rem this doesn't seem to exist in debug builds, but everything still builds. is it necessary?
if %errorlevel% GTR 0 echo "Could not copy!"
xcopy "%DIR%\TwistedLogik.Ultraviolet.OpenGL.dll" TwistedLogik.Ultraviolet.OpenGL.dll /Y
if %errorlevel% GTR 0 goto fail
xcopy "%DIR%\TwistedLogik.Ultraviolet.OpenGL.xml" TwistedLogik.Ultraviolet.OpenGL.xml /Y
rem this doesn't seem to exist in debug builds, but everything still builds. is it necessary?
if %errorlevel% GTR 0 echo "Could not copy!"
xcopy "%DIR%\TwistedLogik.Ultraviolet.SDL2.dll" TwistedLogik.Ultraviolet.SDL2.dll /Y
if %errorlevel% GTR 0 goto fail
xcopy "%DIR%\TwistedLogik.Ultraviolet.SDL2.dll.config" TwistedLogik.Ultraviolet.SDL2.dll.config /Y
if %errorlevel% GTR 0 goto fail
xcopy "%DIR%\TwistedLogik.Ultraviolet.BASS.dll" TwistedLogik.Ultraviolet.BASS.dll /Y
if %errorlevel% GTR 0 goto fail
xcopy "%DIR%\TwistedLogik.Ultraviolet.BASS.dll.config" TwistedLogik.Ultraviolet.BASS.dll.config /Y
if %errorlevel% GTR 0 goto fail
xcopy "%DIR%\TwistedLogik.Ultraviolet.Desktop.dll" TwistedLogik.Ultraviolet.Desktop.dll /Y
if %errorlevel% GTR 0 goto fail
xcopy "%DIR%\TwistedLogik.Ultraviolet.UI.Presentation.dll" TwistedLogik.Ultraviolet.UI.Presentation.dll /Y
if %errorlevel% GTR 0 goto fail

exit 0

:notexist

echo Binary directory %DIR% not found, please 'Rebuild all' in library project first!
exit 1

:fail
echo Copying failed, please try rebuilding everything in the library project.
exit 2