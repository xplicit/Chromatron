// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

global using System.Net;
global using System.Reflection;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Hosting.Server;
global using Microsoft.AspNetCore.Mvc.Controllers;
global using Microsoft.AspNetCore.Mvc.Infrastructure;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Logging;

global using Chromium.AspNetCore.Bridge;
global using Chromatron.Browser;
global using Chromatron.Core;
global using Chromatron.Core.Configuration;
global using Chromatron.Core.Defaults;
global using Chromatron.Core.Host;
global using Chromatron.Core.Infrastructure;
global using Chromatron.Core.Logging;
global using Chromatron.Core.Network;
global using Chromatron.Core.Owin;
global using Xilium.CefGlue;
