using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

[assembly: CLSCompliant(true)]
[assembly: AllowPartiallyTrustedCallers]

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle(@"Nucleus Design Library")]
[assembly: AssemblyDescription(
    @"Contains type converters and custom editors for types defined in the Nucleus utility library.")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type. Only Windows
// assemblies support COM.
[assembly: ComVisible(false)]

// On Windows, the following GUID is for the ID of the typelib if this
// project is exposed to COM. On other platforms, it unique identifies the
// title storage container when deploying this assembly to the device.
[assembly: Guid("31577f23-96d7-429d-a670-fa9449ebdd6e")]
