global using System.Buffers;
global using System.Collections.Concurrent;
global using System.Collections.Specialized;
global using System.Diagnostics;
global using System.Drawing;
global using System.Net;
global using System.Reflection;
global using System.Runtime.InteropServices;
global using System.Text;
global using System.Text.Json;
global using System.Text.RegularExpressions;
global using System.Xml;
global using System.Xml.Linq;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Logging;

global using static Chromatron.NativeHosts.WinHost.Interop;
global using static Chromatron.NativeHosts.WinHost.Interop.User32;
global using static Chromatron.NativeHosts.LinuxHost.InteropLinux;
global using static Chromatron.NativeHosts.MacHost.InteropMac;

global using Chromatron.Browser;
global using Chromatron.Core;
global using Chromatron.Core.Configuration;
global using Chromatron.Core.Defaults;
global using Chromatron.Core.Host;
global using Chromatron.Core.Infrastructure;
global using Chromatron.Core.Network;
global using Chromatron.Core.Logging;
global using Chromatron.Loader;
global using Chromatron.NativeHosts;

global using Xilium.CefGlue;
global using Xilium.CefGlue.Wrapper;

