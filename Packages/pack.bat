@del /s *.nuspec 2>NUL
@del /s *.nupkg 2>NUL

@if [%1]==[] (set UV_BUILD=0) else (set UV_BUILD=%1)

@for /f %%x in ('powershell -Command "[string]::Format([System.IO.File]::ReadAllText(\"../Source/VersionString.txt\"), [System.DateTime]::Now)"') do @(set UV_VERSION_MAJOR_MINOR=%%x)
@set UV_VERSION=%UV_VERSION_MAJOR_MINOR%.%UV_BUILD%

@echo Creating NuGet packages for Ultraviolet Framework %UV_VERSION%...

powershell -Command "(gc Ultraviolet.OpenGL.Bindings.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.OpenGL.Bindings.nuspec"
nuget pack Ultraviolet.OpenGL.Bindings.nuspec -Symbols
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Core.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Core.nuspec"
nuget pack Ultraviolet.Core.nuspec -Symbols
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Shims.Android.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Shims.Android.nuspec"
nuget pack Ultraviolet.Shims.Android.nuspec -Symbols
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.BASS.Native.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.BASS.Native.nuspec"
nuget pack Ultraviolet.BASS.Native.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.BASS.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.BASS.nuspec"
nuget pack Ultraviolet.BASS.nuspec -Symbols
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Shims.Desktop.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Shims.Desktop.nuspec"
nuget pack Ultraviolet.Shims.Desktop.nuspec -Symbols
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.nuspec"
nuget pack Ultraviolet.nuspec -Symbols
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.OpenGL.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.OpenGL.nuspec"
nuget pack Ultraviolet.OpenGL.nuspec -Symbols
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Shims.macOS.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Shims.macOS.nuspec"
nuget pack Ultraviolet.Shims.macOS.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Shims.iOS.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Shims.iOS.nuspec"
nuget pack Ultraviolet.Shims.iOS.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.SDL2.Native.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.SDL2.Native.nuspec"
nuget pack Ultraviolet.SDL2.Native.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.SDL2.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.SDL2.nuspec"
nuget pack Ultraviolet.SDL2.nuspec -Symbols
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.SDL2.UIKit.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.SDL2.UIKit.nuspec"
nuget pack Ultraviolet.SDL2.UIKit.nuspec -Symbols
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Presentation.Compiler.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Presentation.Compiler.nuspec"
nuget pack Ultraviolet.Presentation.Compiler.nuspec -Symbols
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Presentation.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Presentation.nuspec"
nuget pack Ultraviolet.Presentation.nuspec -Symbols
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Tools.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Tools.nuspec"
nuget pack Ultraviolet.Tools.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Game.Desktop.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Game.Desktop.nuspec"
nuget pack Ultraviolet.Game.Desktop.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Game.Android.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Game.Android.nuspec"
nuget pack Ultraviolet.Game.Android.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Game.iOS.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Game.iOS.nuspec"
nuget pack Ultraviolet.Game.iOS.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Game.macOS.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Game.macOS.nuspec"
nuget pack Ultraviolet.Game.macOS.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Windows.Forms.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Windows.Forms.nuspec"
nuget pack Ultraviolet.Windows.Forms.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Game.Windows.Forms.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Game.Windows.Forms.nuspec"
nuget pack Ultraviolet.Game.Windows.Forms.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%