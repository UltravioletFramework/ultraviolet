### Builds
| Branch       | Integration | Release |
|--------------|-------------|---------|
| master       | ![Build Status](http://dev.twistedlogik.net:8085/plugins/servlet/wittified/build-status/UV-INT)  | ![Build Status](http://dev.twistedlogik.net:8085/plugins/servlet/wittified/build-status/UV-REL) |

What is Ultraviolet?
====================

Ultraviolet is a cross-platform, .NET game development framework written in C# and released under the [MIT License](http://opensource.org/licenses/MIT). It is heavily inspired by Microsoft's XNA Framework, and is intended to be easy for XNA developers to quickly pick up and start using. However, unlike [MonoGame](http://www.monogame.net/) and similar projects, Ultraviolet is not intended to be a drop-in replacement for XNA. Its current implementation is written on top of [SDL2](https://www.libsdl.org/) and [OpenGL](https://www.opengl.org/), but its modular design makes it (relatively) easy to re-implement using other technologies if it becomes necessary to do so in the future.

At present, Ultraviolet officially supports Windows, Linux, Mac OS X, and Android. Support for additional platforms will be forthcoming in future releases. 

Some core features of the Ultraviolet Framework:

 * __A runtime content pipeline__
   
   Easily load game assets using Ultraviolet's content pipeline. Unlike XNA, Ultraviolet's content pipeline operates at runtime, meaning no special Visual Studio projects are required to make it work. Content preprocessing is supported in order to increase efficiency and decrease load times.
 
 * __High-level 2D rendering abstractions__
   
   Familiar classes like SpriteBatch allow you to efficiently render large numbers of 2D sprites. Ultraviolet includes built-in support for texture atlases and XML-driven sprite sheets.
 
 * __High-level 3D rendering abstractions__
   
   Coming in a future release.
 
 * __Low-level rendering functionality__
   
   In addition to the abstractions described above, Ultraviolet's graphics subsystem allows you to push polygons directly to the graphics device, giving you complete control.
 
 * __A powerful text formatting and layout engine__
   
   Do more than draw plain strings of text. Ultraviolet's text formatting engine allows you to change your text's font, style, and color on the fly. The layout engine allows you to easily position and align text wherever you need it.
 
 * __XML-driven object loader for easy content creation__
   
   Ultraviolet's object loader allows you to easily create complicated hierarchies of objects from simple XML files. This is more than just an XML serializer&mdash;because it is integrated with Ultraviolet, it has direct knowledge of your game's content assets and object lists, making it possible to reference them in a simple, flexible, and readable way.

The Ultraviolet Framework's source code is [available on GitHub](https://github.com/tlgkccampbell/ultraviolet). If you're developing on Windows, and you just want to get started making games, you can use the installer provided as part of the [latest release](https://github.com/tlgkccampbell/ultraviolet/releases). It will ensure that you have the necessary DLLs and install Visual Studio templates for developing Ultraviolet applications. 

Requirements
============

Building Ultraviolet requires Visual Studio 2015 or later, or an equivalent version of Mono.

Building the mobile projects requires the appropriate Xamarin tools to be installed.

The following platforms are supported for building the Framework:
* Windows
* Linux (Ubuntu)
* OS X

Please file an issue if you encounter any difficulty building on any of these platforms. Linux distributions other than Ubuntu should work provided that they can run Mono, but only Ubuntu has been thoroughly tested.

Building
========

__On Windows__

From the Developer Command Prompt for VS2015, navigate to the root of the Ultraviolet source tree and run the following command:

    msbuild Ultraviolet.proj
    
This will build the Desktop version of the Framework assemblies and copy them into the `Ultraviolet Framework Samples/Dependencies` directory so that the sample projects can be built.

__On Unix__

With Mono installed, navigate to the root of the Ultraviolet source tree and run the following command:

    xbuild Ultraviolet.proj
    
This will build the Desktop version of the Framework assemblies, plus the OS X compatibility shim if you are building on a Mac, and copy them into the `Ultraviolet Framework Samples/Dependencies` directory so that the sample projects can be built.

__Mobile Platforms__

Building Ultraviolet for iOS and Android requires that Xamarin be installed. As with the desktop version of the Framework, you need to run either `msbuild` or `xbuild` on `Ultraviolet.proj`, but you must also explicitly specify that you want to use one of the mobile build targets, i.e.:

    msbuild Ultraviolet.proj /t:BuildAndroid
    
or

    msbuild Ultraviolet.proj /t:BuildiOS
   
Building the iOS version of the Framework should only be done on a Mac. While Xamarin theoretically supports building iOS assemblies remotely on Windows using a Mac as a server, in practice this doesn't work reliably (at least not for Ultraviolet).
   
__Sample Projects__

The sample projects in the `Ultraviolet Framework Samples` directory cannot be built until the Framework itself has been built and its output files have been copied into the `Ultraviolet Framework Samples/Dependencies` directory. Once that has been done, simply run `msbuild` or `xbuild` on the appropriate solution (`.sln`) file in the samples directory.

Known Issues
============

* __The imported project "C:\Program Files (x86)\MSBuild\Microsoft\VisualStudio\v14.0\CodeSharing\Microsoft.CodeSharing.CSharp.targets" was not found...__

  This was an issue with the MSBuild targets provided by the RTM version of Visual Studio 2015. This issue has been fixed in Visual Studio 2015 Update 1; please make sure you're using the latest update.

Documentation
=============

[Reference Documentation](http://uv.twistedlogik.net/ultraviolet/reference/v1.1)

_Overview_

* [Basic Concepts in Ultraviolet](http://wiki.ultraviolet.tl/index.php?title=Basic_Concepts_in_Ultraviolet)
* [Matrices and Vectors](http://wiki.ultraviolet.tl/index.php?title=Matrices_and_Vectors)

_Sample Projects_

* [Sample 1 - Creating an Ultraviolet Application](http://wiki.ultraviolet.tl/index.php?title=Sample_1_-_Creating_an_Ultraviolet_Application)
* [Sample 2 - Handling Input](http://wiki.ultraviolet.tl/index.php?title=Sample_2_-_Handling_Input)
* [Sample 3 - Rendering Geometry](http://wiki.ultraviolet.tl/index.php?title=Sample_3_-_Rendering_Geometry)
* [Sample 4 - Content Management](http://wiki.ultraviolet.tl/index.php?title=Sample_4_-_Content_Management)
* [Sample 5 - Rendering Sprites](http://wiki.ultraviolet.tl/index.php?title=Sample_5_-_Rendering_Sprites)
* [Sample 6 - Rendering Text](http://wiki.ultraviolet.tl/index.php?title=Sample_6_-_Rendering_Text)
* [Sample 7 - Playing Music](http://wiki.ultraviolet.tl/index.php?title=Sample_7_-_Playing_Music)
* [Sample 8 - Playing Sound Effects](http://wiki.ultraviolet.tl/index.php?title=Sample_8_-_Playing_Sound_Effects)
* [Sample 9 - Managing State with UI Screens](http://wiki.ultraviolet.tl/index.php?title=Sample_9_-_Managing_State_with_UI_Screens)
* [Sample 10 - Asynchronous Content Loading](http://wiki.ultraviolet.tl/index.php?title=Sample_10_-_Asynchronous_Content_Loading)
* [Sample 11 - Game Pads](http://wiki.ultraviolet.tl/index.php?title=Sample_11_-_Game_Pads)
* [Sample 12 - Ultraviolet Presentation Foundation](http://wiki.ultraviolet.tl/index.php?title=Sample_12_-_Ultraviolet_Presentation_Foundation)

_Advanced Topics_

* Data
  * [Data object registries](http://wiki.ultraviolet.tl/index.php?title=Data_object_registries) 
* Content
  * [Content archives](http://wiki.ultraviolet.tl/index.php?title=Content_archives)
  * [Content substitution](http://wiki.ultraviolet.tl/index.php?title=Content_substitution)
  * [Content preprocessing](http://wiki.ultraviolet.tl/index.php?title=Content_preprocessing)
  * [Content metadata files](http://wiki.ultraviolet.tl/index.php?title=Content_metadata_files)
* Graphics
  * [Generating fonts](http://wiki.ultraviolet.tl/index.php?title=Generating_fonts) 
  * [Custom shaders and effects](http://wiki.ultraviolet.tl/index.php?title=Custom_shaders_and_effects)
  * [Custom SpriteBatch implementations](http://wiki.ultraviolet.tl/index.php?title=Custom_SpriteBatch_implementations)
* Android
  * [Working with Ultraviolet on Android](http://wiki.ultraviolet.tl/index.php?title=Working_with_Ultraviolet_on_Android) 
* Ultraviolet Presentation Foundation
  * [UPF Layout Cycle](http://wiki.ultraviolet.tl/index.php?title=UPF_Layout_Cycle)
  * [UPF Layout Containers](http://wiki.ultraviolet.tl/index.php?title=UPF_Layout_Containers)
  * [UPF Elements](http://wiki.ultraviolet.tl/index.php?title=UPF_Elements)
  * [Ultraviolet Markup Language](http://wiki.ultraviolet.tl/index.php?title=Ultraviolet_Markup_Language)
  * [Ultraviolet Style Sheets](http://wiki.ultraviolet.tl/index.php?title=Ultraviolet_Style_Sheets)
  * [Dependency properties](http://wiki.ultraviolet.tl/index.php?title=Dependency_properties)
  * [Routed events](http://wiki.ultraviolet.tl/index.php?title=Routed_events)

Project Road Map
================

What follows is a tentative road map for the next several major revisions of Ultraviolet. This list is subject to change at any time. Items which have been ~~struck through~~ are basically complete, though they may not yet be part of an official release.

* __Ultraviolet 1.4__
  * _Graphics_
    * Improved 3D rendering support
    * Model class or equivalent
    * Support for model instancing
    * Support for skeletal animation
  * Signed distance field fonts
* __Ultraviolet 1.3__
  * _UI_
    * ~~Continued work on UPF~~
* __Ultraviolet 1.2__
  * _UI_
    * ~~Ultraviolet Presentation Foundation (UPF)~~
* __Ultraviolet 1.1__
  * _Miscellaneous_
    * ~~Android support~~
    * ~~Performance improvements~~
    * ~~Design assembly (i.e. ``TypeConverter`` implementations) for Nucleus~~
    * ~~Design assembly (i.e. ``TypeConverter`` implementations) for Ultraviolet~~
  * _Input_
    * ~~New input device: GamePad~~
    * ~~New input device: TouchPad~~
  * _Graphics_
    * ~~Support for Direct State Access (DSA)~~
    * ~~Better support for East Asian character sets in ``SpriteFont``~~
* __Ultraviolet 1.0__
  * _Miscellaneous_
    * ~~First release~~
