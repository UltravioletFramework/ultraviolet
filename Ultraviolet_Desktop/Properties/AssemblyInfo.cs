using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

[assembly: CLSCompliant(true)]
[assembly: AllowPartiallyTrustedCallers]

#if DESKTOP
#if SIGNED
[assembly: InternalsVisibleTo("Ultraviolet.Design, PublicKey=" +
    "00240000048000009400000006020000002400005253413100040000010001005dd0e010413925" +
    "79d63e81ea2cce6eeb67e8bde9256a39ba0ae06d5c96eef50905c7ee69ac28905ef5f2c9a8efce" +
    "6cc414dafe1ef66180873448e75c875dafa6d976c9642cc1dbf14ec53c97d81046059d7a17f0ed" +
    "30184ead039903031f7d8cbd02fa5021796e92dd810141ad3288ace425af60305ed8b090910d12" +
    "59a204da")]
[assembly: InternalsVisibleTo("Ultraviolet.Desktop, PublicKey=" +
    "00240000048000009400000006020000002400005253413100040000010001005dd0e010413925" +
    "79d63e81ea2cce6eeb67e8bde9256a39ba0ae06d5c96eef50905c7ee69ac28905ef5f2c9a8efce" +
    "6cc414dafe1ef66180873448e75c875dafa6d976c9642cc1dbf14ec53c97d81046059d7a17f0ed" +
    "30184ead039903031f7d8cbd02fa5021796e92dd810141ad3288ace425af60305ed8b090910d12" +
    "59a204da")]
#else
[assembly: InternalsVisibleTo("Ultraviolet.Design")]
[assembly: InternalsVisibleTo("Ultraviolet.Desktop")]
#endif
#endif

#if ANDROID
#if SIGNED
[assembly: InternalsVisibleTo("Ultraviolet.Android, PublicKey=" +
    "00240000048000009400000006020000002400005253413100040000010001005dd0e010413925" +
    "79d63e81ea2cce6eeb67e8bde9256a39ba0ae06d5c96eef50905c7ee69ac28905ef5f2c9a8efce" +
    "6cc414dafe1ef66180873448e75c875dafa6d976c9642cc1dbf14ec53c97d81046059d7a17f0ed" +
    "30184ead039903031f7d8cbd02fa5021796e92dd810141ad3288ace425af60305ed8b090910d12" +
    "59a204da")]
#else
[assembly: InternalsVisibleTo("Ultraviolet.Android")]
#endif
[assembly: Android.LinkerSafe]
#endif

#if IOS
[assembly: Foundation.Preserve(typeof(Object), AllMembers = true)]
[assembly: Foundation.Preserve(typeof(Nullable), AllMembers = true)]
[assembly: Foundation.LinkerSafe]
#endif

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle(@"Ultraviolet Core Library")]
[assembly: AssemblyDescription(
    @"Defines the core types that make up the Ultraviolet Framework API.")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type. Only Windows
// assemblies support COM.
[assembly: ComVisible(false)]

// On Windows, the following GUID is for the ID of the typelib if this
// project is exposed to COM. On other platforms, it unique identifies the
// title storage container when deploying this assembly to the device.
[assembly: Guid("d417c400-6ade-4f96-b45e-26bc7d685d36")]
