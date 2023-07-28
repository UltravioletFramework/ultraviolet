@del /s *.nuspec 2>NUL
@del /s *.nupkg 2>NUL
@del /s *.snupkg 2>NUL

@if [%1]==[] (set UV_BUILD=0) else (set UV_BUILD=%1)

@for /f %%x in ('powershell -Command "[string]::Format([System.IO.File]::ReadAllText(\"../Source/VersionString.txt\"), [System.DateTime]::Now)"') do @(set UV_VERSION_MAJOR_MINOR=%%x)
@set UV_VERSION=%UV_VERSION_MAJOR_MINOR%.%UV_BUILD%

@echo Creating NuGet packages for Ultraviolet Framework %UV_VERSION%...

powershell -Command "(gc Ultraviolet.OpenGL.Environment.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.OpenGL.Environment.nuspec"
nuget pack Ultraviolet.OpenGL.Environment.nuspec -Symbols -SymbolPackageFormat snupkg
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.OpenGL.Bindings.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.OpenGL.Bindings.nuspec"
nuget pack Ultraviolet.OpenGL.Bindings.nuspec -Symbols -SymbolPackageFormat snupkg
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Core.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Core.nuspec"
nuget pack Ultraviolet.Core.nuspec -Symbols -SymbolPackageFormat snupkg
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Shims.Android.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Shims.Android.nuspec"
nuget pack Ultraviolet.Shims.Android.nuspec -Symbols -SymbolPackageFormat snupkg
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.BASS.Native.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.BASS.Native.nuspec"
nuget pack Ultraviolet.BASS.Native.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.BASS.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.BASS.nuspec"
nuget pack Ultraviolet.BASS.nuspec -Symbols -SymbolPackageFormat snupkg
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.FMOD.Native.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.FMOD.Native.nuspec"
nuget pack Ultraviolet.FMOD.Native.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.FMOD.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.FMOD.nuspec"
nuget pack Ultraviolet.FMOD.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Shims.NETCore.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Shims.NETCore.nuspec"
nuget pack Ultraviolet.Shims.NETCore.nuspec -Symbols -SymbolPackageFormat snupkg
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.nuspec"
nuget pack Ultraviolet.nuspec -Properties NoWarn=NU5104 -Symbols -SymbolPackageFormat snupkg
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.OpenGL.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.OpenGL.nuspec"
nuget pack Ultraviolet.OpenGL.nuspec -Symbols -SymbolPackageFormat snupkg
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.SDL2.Native.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.SDL2.Native.nuspec"
nuget pack Ultraviolet.SDL2.Native.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.SDL2.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.SDL2.nuspec"
nuget pack Ultraviolet.SDL2.nuspec -Symbols -SymbolPackageFormat snupkg
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Presentation.Compiler.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Presentation.Compiler.nuspec"
nuget pack Ultraviolet.Presentation.Compiler.nuspec -Symbols -SymbolPackageFormat snupkg
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Presentation.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Presentation.nuspec"
nuget pack Ultraviolet.Presentation.nuspec -Symbols -SymbolPackageFormat snupkg
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.FreeType2.Native.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.FreeType2.Native.nuspec"
nuget pack Ultraviolet.FreeType2.Native.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.FreeType2.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.FreeType2.nuspec"
nuget pack Ultraviolet.FreeType2.nuspec -Symbols -SymbolPackageFormat snupkg
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.ImGuiViewProvider.Native.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.ImGuiViewProvider.Native.nuspec"
nuget pack Ultraviolet.ImGuiViewProvider.Native.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.ImGuiViewProvider.Bindings.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.ImGuiViewProvider.Bindings.nuspec"
nuget pack Ultraviolet.ImGuiViewProvider.Bindings.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.ImGuiViewProvider.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.ImGuiViewProvider.nuspec"
nuget pack Ultraviolet.ImGuiViewProvider.nuspec -Symbols -SymbolPackageFormat snupkg
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Tools.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Tools.nuspec"
nuget pack Ultraviolet.Tools.nuspec
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Windows.Forms.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Windows.Forms.nuspec"
nuget pack Ultraviolet.Windows.Forms.nuspec -Symbols -SymbolPackageFormat snupkg
@if %errorlevel% neq 0 @exit /b %errorlevel%

powershell -Command "(gc Ultraviolet.Image.nuspe_) -replace 'UV_VERSION', '%UV_VERSION%' | sc Ultraviolet.Image.nuspec"
nuget pack Ultraviolet.Image.nuspec -Symbols -SymbolPackageFormat snupkg
@if %errorlevel% neq 0 @exit /b %errorlevel%