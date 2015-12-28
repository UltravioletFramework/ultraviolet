#!/bin/bash
# Copies sample dependencies from the Binaries folder

cp "../../Version.cs" "../Version.cs"

cp "../../Binaries/AnyCPU/Release/Newtonsoft.Json.dll" Newtonsoft.Json.dll
cp "../../Binaries/AnyCPU/Release/TwistedLogik.Gluon.dll" TwistedLogik.Gluon.dll
cp "../../Binaries/AnyCPU/Release/TwistedLogik.Nucleus.dll" TwistedLogik.Nucleus.dll
cp "../../Binaries/AnyCPU/Release/TwistedLogik.Nucleus.xml" TwistedLogik.Nucleus.xml 2>/dev/null
cp "../../Binaries/AnyCPU/Release/TwistedLogik.Ultraviolet.dll" TwistedLogik.Ultraviolet.dll
cp "../../Binaries/AnyCPU/Release/TwistedLogik.Ultraviolet.xml" TwistedLogik.Ultraviolet.xml 2>/dev/null
cp "../../Binaries/AnyCPU/Release/TwistedLogik.Ultraviolet.OpenGL.dll" TwistedLogik.Ultraviolet.OpenGL.dll
cp "../../Binaries/AnyCPU/Release/TwistedLogik.Ultraviolet.OpenGL.xml" TwistedLogik.Ultraviolet.OpenGL.xml 2>/dev/null
cp "../../Binaries/AnyCPU/Release/TwistedLogik.Ultraviolet.SDL2.dll" TwistedLogik.Ultraviolet.SDL2.dll
cp "../../Binaries/AnyCPU/Release/TwistedLogik.Ultraviolet.BASS.dll" TwistedLogik.Ultraviolet.BASS.dll
cp "../../Binaries/AnyCPU/Release/TwistedLogik.Ultraviolet.Desktop.dll" TwistedLogik.Ultraviolet.Desktop.dll
cp "../../Binaries/AnyCPU/Release/TwistedLogik.Ultraviolet.UI.Presentation.dll" TwistedLogik.Ultraviolet.UI.Presentation.dll
cp "../../Binaries/AnyCPU/Release/TwistedLogik.Ultraviolet.UI.Presentation.Uvss.dll" TwistedLogik.Ultraviolet.UI.Presentation.Uvss.dll
cp "../../Binaries/AnyCPU/Release/TwistedLogik.Ultraviolet.UI.Presentation.Compiler.dll" TwistedLogik.Ultraviolet.UI.Presentation.Compiler.dll

cp "../../Binaries/AnyCPU/Release/Newtonsoft.Json.dll" Android/Newtonsoft.Json.dll
cp "../../Binaries/Android/Release/TwistedLogik.Gluon.dll" Android/TwistedLogik.Gluon.dll
cp "../../Binaries/Android/Release/TwistedLogik.Nucleus.dll" Android/TwistedLogik.Nucleus.dll
cp "../../Binaries/Android/Release/TwistedLogik.Nucleus.xml" Android/TwistedLogik.Nucleus.xml 2>/dev/null
cp "../../Binaries/Android/Release/TwistedLogik.Ultraviolet.dll" Android/TwistedLogik.Ultraviolet.dll
cp "../../Binaries/Android/Release/TwistedLogik.Ultraviolet.xml" Android/TwistedLogik.Ultraviolet.xml 2>/dev/null
cp "../../Binaries/Android/Release/TwistedLogik.Ultraviolet.OpenGL.dll" Android/TwistedLogik.Ultraviolet.OpenGL.dll
cp "../../Binaries/Android/Release/TwistedLogik.Ultraviolet.OpenGL.xml" Android/TwistedLogik.Ultraviolet.OpenGL.xml 2>/dev/null
cp "../../Binaries/Android/Release/TwistedLogik.Ultraviolet.SDL2.dll" Android/TwistedLogik.Ultraviolet.SDL2.dll
cp "../../Binaries/Android/Release/TwistedLogik.Ultraviolet.BASS.dll" Android/TwistedLogik.Ultraviolet.BASS.dll
cp "../../Binaries/Android/Release/TwistedLogik.Ultraviolet.Android.dll" Android/TwistedLogik.Ultraviolet.Android.dll
cp "../../Binaries/Android/Release/TwistedLogik.Ultraviolet.UI.Presentation.dll" Android/TwistedLogik.Ultraviolet.UI.Presentation.dll
cp "../../Binaries/Android/Release/TwistedLogik.Ultraviolet.UI.Presentation.Uvss.dll" Android/TwistedLogik.Ultraviolet.UI.Presentation.Uvss.dll
cp "../../Binaries/Android/Release/Org.Libsdl.App.dll" Android/Org.Libsdl.App.dll
