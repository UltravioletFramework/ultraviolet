using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: CLSCompliant (true)]

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle (@"Platform Compatibility Shim for macOS")]
[assembly: AssemblyDescription (
    @"Provides Ultraviolet type implementations which are specific to the Xamarin.Mac framework.")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type. Only Windows
// assemblies support COM.
[assembly: ComVisible (false)]

// On Windows, the following GUID is for the ID of the typelib if this
// project is exposed to COM. On other platforms, it unique identifies the
// title storage container when deploying this assembly to the device.
[assembly: Guid ("54678ab0-1bf5-4f0f-a706-a3d0785ef481")]