What is Ultraviolet?
====================

Ultraviolet is a cross-platform, .NET game development framework written in C# and released under the [MIT License](http://opensource.org/licenses/MIT). It is heavily inspired by Microsoft's XNA Framework, and is intended to be easy for XNA developers to quickly pick up and start using. However, unlike [MonoGame](http://www.monogame.net/) and similar projects, Ultraviolet is not intended to be a drop-in replacement for XNA. Its current implementation is written on top of [SDL2](https://www.libsdl.org/) and [OpenGL](https://www.opengl.org/), but its modular design makes it (relatively) easy to re-implement using other technologies if it becomes necessary to do so in the future.

At present, Ultraviolet officially supports Windows and Linux. Support for additional platforms will be forthcoming in future releases. 

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

Known Issues
============

* __Strong Naming__

  If you're going to build Ultraviolet from source, you're going to need to account for the fact that all of its assemblies are delay signed. The easiest way to do this is to open a Visual Studio command prompt and enter the following command:
  
  ``sn -Vr *,78da2f4877323311``
  
  You may need to run this command in both a 32-bit and 64-bit prompt, and you should restart Visual Studio afterwards if it was already running.
  
  This command registers TwistedLogik's public key token for verification skipping on your machine. This means that the CLR will load assemblies with this public key token even if they are not correctly signed, making it possible to build and debug Ultraviolet assemblies without requiring access to the TwistedLogik private key file.
  
  Be sure to run the command prompt as an administrator. Failure to do so will result in the following error:
  
  ``Failed to open registry key -- Unable to format error message 00000005``

* __Building the Samples__

  If you encounter errors when attempting to build ``Ultraviolet Framework Samples.sln``, make sure that you've built ``TwistedLogik.Ultraviolet.sln`` in ``Release`` mode first.
  
* __General Compatibility__

  Ultraviolet is still in the early stages of its development, and as such it has not yet been fully tested on a wide range of hardware. If you encounter compatibility issues on your machine, please [register an issue on itHub](https://github.com/tlgkccampbell/ultraviolet/issues) so we can try to address it!
  
Documentation
=============

[Reference Documentation](http://uv.twistedlogik.net/reference)

_Overview_

* [Basic Concepts in Ultraviolet](http://uv.twistedlogik.net/index.php?title=Basic_Concepts_in_Ultraviolet)
* [Matrices and Vectors](http://uv.twistedlogik.net/index.php?title=Matrices_and_Vectors)

_Sample Projects_

* [Sample 1 - Creating an Ultraviolet Application](http://uv.twistedlogik.net/index.php?title=Sample_1_-_Creating_an_Ultraviolet_Application)
* [Sample 2 - Handling Input](http://uv.twistedlogik.net/index.php?title=Sample_2_-_Handling_Input)
* [Sample 3 - Rendering Geometry](http://uv.twistedlogik.net/index.php?title=Sample_3_-_Rendering_Geometry)
* [Sample 4 - Content Management](http://uv.twistedlogik.net/index.php?title=Sample_4_-_Content_Management)
* [Sample 5 - Rendering Sprites](http://uv.twistedlogik.net/index.php?title=Sample_5_-_Rendering_Sprites)
* [Sample 6 - Rendering Text](http://uv.twistedlogik.net/index.php?title=Sample_6_-_Rendering_Text)
* [Sample 7 - Playing Music](http://uv.twistedlogik.net/index.php?title=Sample_7_-_Playing_Music)
* [Sample 8 - Playing Sound Effects](http://uv.twistedlogik.net/index.php?title=Sample_8_-_Playing_Sound_Effects)
* [Sample 9 - Managing State with UI Screens](http://uv.twistedlogik.net/index.php?title=Sample_9_-_Managing_State_with_UI_Screens)
* [Sample 10 - Asynchronous Content Loading](http://uv.twistedlogik.net/index.php?title=Sample_10_-_Asynchronous_Content_Loading)

_Advanced Topics_

* Content
  * [Content preprocessing](http://uv.twistedlogik.net/index.php?title=Content_preprocessing)
  * [Content metadata files](http://uv.twistedlogik.net/index.php?title=Content_metadata_files)
* Graphics
  * [Custom shaders and effects](http://uv.twistedlogik.net/index.php?title=Custom_shaders_and_effects)
  * [Custom SpriteBatch implementations](http://uv.twistedlogik.net/index.php?title=Custom_SpriteBatch_implementations)

Project Road Map
================

What follows is a tentative road map for the next several major revisions of Ultraviolet. This list is subject to change at any time.

* __Ultraviolet 1.0__
  * _Miscellaneous_
    * First release
* __Ultraviolet 1.1__
  * _Miscellaneous_
    * Android support
    * Performance improvements
    * Design assembly (i.e. ``TypeConverter`` implementations) for Nucleus
    * Design assembly (i.e. ``TypeConverter`` implementations) for Ultraviolet
  * _Input_
    * New input device: GamePad
    * New input device: TouchPad
  * _Graphics_
    * Support for Direct State Access (DSA)
    * Better support for East Asian character sets in ``SpriteFont``
    * Signed distance field fonts?
* __Ultraviolet 1.2__
  * _UI_
    * Ultraviolet Core Layout Provider
* __Ultraviolet 1.3__
  * _Graphics_
    * Improved 3D rendering support
    * Model class or equivalent
    * Support for model instancing
    * Support for skeletal animation
