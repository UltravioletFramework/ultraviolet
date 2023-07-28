_For questions and discussion, check out our [Discord](https://discord.gg/ehQMumHJ)._

What is Ultraviolet?
====================

[![Join the chat at https://discord.gg/ehQMumHJ]](https://discord.gg/ehQMumHJ)

Ultraviolet is a cross-platform, .NET game development framework written in C# and released under the [MIT License](http://opensource.org/licenses/MIT). It is heavily inspired by Microsoft's XNA Framework, and is intended to be easy for XNA developers to quickly pick up and start using. However, unlike [MonoGame](http://www.monogame.net/) and similar projects, Ultraviolet is not intended to be a drop-in replacement for XNA. Its current implementation is written on top of [SDL2](https://www.libsdl.org/) and [OpenGL](https://www.opengl.org/), but its modular design makes it (relatively) easy to re-implement using other technologies if it becomes necessary to do so in the future.

At present, Ultraviolet officially supports Windows, Linux, and macOS using .NET 6, as well as Android through Xamarin. Support for iOS is no longer provided due to a lack of the requisite development hardware. If anyone wants to take responsibility for providing this support, please [Create an issue](https://github.com/UltravioletFramework/ultraviolet/issues).

Some core features of the Ultraviolet Framework:

 * __A runtime content pipeline__
   
   Easily load game assets using Ultraviolet's content pipeline. Unlike XNA, Ultraviolet's content pipeline operates at runtime, meaning no special Visual Studio projects are required to make it work. Content preprocessing is supported in order to increase efficiency and decrease load times.
 
 * __High-level 2D rendering abstractions__
   
   Familiar classes like SpriteBatch allow you to efficiently render large numbers of 2D sprites. Ultraviolet includes built-in support for texture atlases and XML-driven sprite sheets.
 
 * __High-level 3D rendering abstractions__
   
   Built-in support for [glTF 2.0 models](https://www.khronos.org/gltf/) and skinned animation makes it easy to get started with 3D rendering. Alternatively, you can write your own GLSL shader programs to take full control of the rendering process, and support for additional model types can be provided by extending the runtime content pipeline.
 
 * __Low-level rendering functionality__
   
   In addition to the abstractions described above, Ultraviolet's graphics subsystem allows you to push polygons directly to the graphics device, giving you complete control.
 
 * __A powerful text formatting and layout engine__
   
   Do more than draw plain strings of text. Ultraviolet's text formatting engine allows you to change your text's font, style, and color on the fly. The layout engine allows you to easily position and align text wherever you need it.
 
 * __XML-driven object loader for easy content creation__
   
   Ultraviolet's object loader allows you to easily create complicated hierarchies of objects from simple XML files. This is more than just an XML serializer&mdash;because it is integrated with Ultraviolet, it has direct knowledge of your game's content assets and object lists, making it possible to reference them in a simple, flexible, and readable way.

The Ultraviolet Framework's source code is [available on GitHub](https://github.com/UltravioletFramework/ultraviolet).

Getting Started
===============

If you don't want to build Ultraviolet yourself, official packages are available through [NuGet](https://www.nuget.org/packages?q=ultraviolet).

The wiki contains a [quick start guide](https://github.com/UltravioletFramework/ultraviolet/wiki/Getting-Started) for development using .NET 6.

A [dedicated repository](https://github.com/UltravioletFramework/ultraviolet-samples) contains a number of sample projects which demonstrate various features of the Framework.

Requirements
============

Ultraviolet can be used with any version of .NET which supports .NET Standard 2.1.

Building Ultraviolet requires .NET 6 SDK.

Building the mobile projects requires the appropriate Xamarin tools to be installed.

The following platforms are supported for building the Framework:
* Windows
* Linux (Ubuntu)
* Android
* macOS

Please file an issue if you encounter any difficulty building on any of these platforms. Linux distributions other than Ubuntu should work, assuming that they can run .NET 6 and you can provide appropriate versions of the native dependencies, but only Ubuntu has been thoroughly tested.

Building
========

__Desktop Platforms__

The `Sources` folder contains several solution files for the various platforms which Ultraviolet supports. Alternatively, you can run `msbuild Ultraviolet.proj` from the command line in the repository's root directory; this will automatically select and build the correct solution for your current platform, and additionally will copy the build results into a single `Binaries` folder.

__Mobile Platforms__

Building Ultraviolet for Android requires that Xamarin be installed. As with the desktop version of the Framework, you can either build the appropriate solution file or `Ultraviolet.proj`, but in the latter case you must also explicitly specify that you want to use one of the mobile build targets, i.e.:

    msbuild Ultraviolet.proj /t:BuildAndroid
