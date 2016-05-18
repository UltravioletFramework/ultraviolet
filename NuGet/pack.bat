del *.nuspec
del *.nupkg

if [%1]==[] ( set UV_BUILD=0 ) else ( set UV_BUILD=%1 )

set UV_VERSION_MAJOR_MINOR=1.3.9
set UV_VERSION=%UV_VERSION_MAJOR_MINOR%.%UV_BUILD%

powershell -Command "(gc TwistedLogik.Gluon.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc TwistedLogik.Gluon.nuspec"
nuget pack TwistedLogik.Gluon.nuspec -Symbols

powershell -Command "(gc TwistedLogik.Nucleus.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc TwistedLogik.Nucleus.nuspec"
nuget pack TwistedLogik.Nucleus.nuspec -Symbols

powershell -Command "(gc TwistedLogik.Ultraviolet.Android.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc TwistedLogik.Ultraviolet.Android.nuspec"
nuget pack TwistedLogik.Ultraviolet.Android.nuspec -Symbols

powershell -Command "(gc TwistedLogik.Ultraviolet.BASS.Native.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc TwistedLogik.Ultraviolet.BASS.Native.nuspec"
nuget pack TwistedLogik.Ultraviolet.BASS.Native.nuspec

powershell -Command "(gc TwistedLogik.Ultraviolet.BASS.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc TwistedLogik.Ultraviolet.BASS.nuspec"
nuget pack TwistedLogik.Ultraviolet.BASS.nuspec -Symbols

powershell -Command "(gc TwistedLogik.Ultraviolet.Desktop.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc TwistedLogik.Ultraviolet.Desktop.nuspec"
nuget pack TwistedLogik.Ultraviolet.Desktop.nuspec -Symbols

powershell -Command "(gc TwistedLogik.Ultraviolet.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc TwistedLogik.Ultraviolet.nuspec"
nuget pack TwistedLogik.Ultraviolet.nuspec -Symbols

powershell -Command "(gc TwistedLogik.Ultraviolet.OpenGL.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc TwistedLogik.Ultraviolet.OpenGL.nuspec"
nuget pack TwistedLogik.Ultraviolet.OpenGL.nuspec -Symbols

powershell -Command "(gc TwistedLogik.Ultraviolet.OSX.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc TwistedLogik.Ultraviolet.OSX.nuspec"
nuget pack TwistedLogik.Ultraviolet.OSX.nuspec

powershell -Command "(gc TwistedLogik.Ultraviolet.SDL2.Native.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc TwistedLogik.Ultraviolet.SDL2.Native.nuspec"
nuget pack TwistedLogik.Ultraviolet.SDL2.Native.nuspec

powershell -Command "(gc TwistedLogik.Ultraviolet.SDL2.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc TwistedLogik.Ultraviolet.SDL2.nuspec"
nuget pack TwistedLogik.Ultraviolet.SDL2.nuspec -Symbols

powershell -Command "(gc TwistedLogik.Ultraviolet.UI.Presentation.Compiler.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc TwistedLogik.Ultraviolet.UI.Presentation.Compiler.nuspec"
nuget pack TwistedLogik.Ultraviolet.UI.Presentation.Compiler.nuspec -Symbols

powershell -Command "(gc TwistedLogik.Ultraviolet.UI.Presentation.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc TwistedLogik.Ultraviolet.UI.Presentation.nuspec"
nuget pack TwistedLogik.Ultraviolet.UI.Presentation.nuspec -Symbols

powershell -Command "(gc TwistedLogik.Ultraviolet.Tools.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc TwistedLogik.Ultraviolet.Tools.nuspec"
nuget pack TwistedLogik.Ultraviolet.Tools.nuspec

powershell -Command "(gc TwistedLogik.Ultraviolet.Game.Desktop.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc TwistedLogik.Ultraviolet.Game.Desktop.nuspec"
nuget pack TwistedLogik.Ultraviolet.Game.Desktop.nuspec

powershell -Command "(gc TwistedLogik.Ultraviolet.Game.Android.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc TwistedLogik.Ultraviolet.Game.Android.nuspec"
nuget pack TwistedLogik.Ultraviolet.Game.Android.nuspec