@del /s *.nuspec 2>NUL
@del /s *.nupkg 2>NUL

@if [%1]==[] (set UV_BUILD=0) else (set UV_BUILD=%1)

@for /f %%x in (../VersionString.txt) do @(set UV_VERSION_MAJOR_MINOR=%%x)
@set UV_VERSION=%UV_VERSION_MAJOR_MINOR%.%UV_BUILD%

@echo Creating NuGet packages for Ultraviolet Framework %UV_VERSION%...

powershell -Command "(gc Ultraviolet.OpenGL.Bindings.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.OpenGL.Bindings.nuspec"
nuget pack Ultraviolet.OpenGL.Bindings.nuspec -Symbols

powershell -Command "(gc Ultraviolet.Core.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Core.nuspec"
nuget pack Ultraviolet.Core.nuspec -Symbols

powershell -Command "(gc Ultraviolet.Android.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Android.nuspec"
nuget pack Ultraviolet.Android.nuspec -Symbols

powershell -Command "(gc Ultraviolet.BASS.Native.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.BASS.Native.nuspec"
nuget pack Ultraviolet.BASS.Native.nuspec

powershell -Command "(gc Ultraviolet.BASS.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.BASS.nuspec"
nuget pack Ultraviolet.BASS.nuspec -Symbols

powershell -Command "(gc Ultraviolet.Desktop.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Desktop.nuspec"
nuget pack Ultraviolet.Desktop.nuspec -Symbols

powershell -Command "(gc Ultraviolet.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.nuspec"
nuget pack Ultraviolet.nuspec -Symbols

powershell -Command "(gc Ultraviolet.OpenGL.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.OpenGL.nuspec"
nuget pack Ultraviolet.OpenGL.nuspec -Symbols

powershell -Command "(gc Ultraviolet.OSX.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.OSX.nuspec"
nuget pack Ultraviolet.OSX.nuspec

powershell -Command "(gc Ultraviolet.SDL2.Native.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.SDL2.Native.nuspec"
nuget pack Ultraviolet.SDL2.Native.nuspec

powershell -Command "(gc Ultraviolet.SDL2.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.SDL2.nuspec"
nuget pack Ultraviolet.SDL2.nuspec -Symbols

powershell -Command "(gc Ultraviolet.SDL2.UIKit.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.SDL2.UIKit.nuspec"
nuget pack Ultraviolet.SDL2.UIKit.nuspec -Symbols

powershell -Command "(gc Ultraviolet.Presentation.Compiler.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Presentation.Compiler.nuspec"
nuget pack Ultraviolet.Presentation.Compiler.nuspec -Symbols

powershell -Command "(gc Ultraviolet.Presentation.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Presentation.nuspec"
nuget pack Ultraviolet.Presentation.nuspec -Symbols

powershell -Command "(gc Ultraviolet.Tools.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Tools.nuspec"
nuget pack Ultraviolet.Tools.nuspec

powershell -Command "(gc Ultraviolet.Game.Desktop.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Game.Desktop.nuspec"
nuget pack Ultraviolet.Game.Desktop.nuspec

powershell -Command "(gc Ultraviolet.Game.Android.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Game.Android.nuspec"
nuget pack Ultraviolet.Game.Android.nuspec

powershell -Command "(gc Ultraviolet.Game.iOS.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Game.iOS.nuspec"
nuget pack Ultraviolet.Game.iOS.nuspec

powershell -Command "(gc Ultraviolet.Game.OSX.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Game.OSX.nuspec"
nuget pack Ultraviolet.Game.OSX.nuspec