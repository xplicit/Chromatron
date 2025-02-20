﻿// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Browser;

internal static class CefSettingsExtension
{
    /// <summary>
    /// Extension method to apply custom settings from <see cref="IChromatronConfiguration.CustomSettings"/>.
    /// </summary>
    /// <param name="cefSettings"><see cref="CefSettings"/> class to extend.</param>
    /// <param name="customSettings">The custom settings type of <see cref="IChromatronConfiguration.CustomSettings"/>.</param>
    public static void Update(this CefSettings cefSettings, IDictionary<string, string>? customSettings)
    {
        if (cefSettings is null || customSettings is null || !customSettings.Any())
        {
            return;
        }

        foreach (var setting in customSettings)
        {
            bool boolResult;
            int intResult;

            if (string.IsNullOrWhiteSpace(setting.Value))
            {
                continue;
            }

            switch (setting.Key.ToUpperInvariant())
            {
                case CefSettingKeys.NOSANDBOX:
                    if (setting.Value.TryParseBoolean(out boolResult))
                    {
                        cefSettings.NoSandbox = boolResult;
                    }

                    break;

                case CefSettingKeys.BROWSERSUBPROCESSPATH:
                    cefSettings.BrowserSubprocessPath = setting.Value;
                    break;

                case CefSettingKeys.MULTITHREADEDMESSAGELOOP:
                    if (setting.Value.TryParseBoolean(out boolResult))
                    {
                        cefSettings.MultiThreadedMessageLoop = boolResult;
                    }

                    break;

                case CefSettingKeys.EXTERNALMESSAGEPUMP:
                    if (setting.Value.TryParseBoolean(out boolResult))
                    {
                        cefSettings.ExternalMessagePump = boolResult;
                    }

                    break;

                case CefSettingKeys.WINDOWLESSRENDERINGENABLED:
                    if (setting.Value.TryParseBoolean(out boolResult))
                    {
                        cefSettings.WindowlessRenderingEnabled = boolResult;
                    }

                    break;

                case CefSettingKeys.COMMANDLINEARGSDISABLED:
                    if (setting.Value.TryParseBoolean(out boolResult))
                    {
                        cefSettings.CommandLineArgsDisabled = boolResult;
                    }

                    break;

                case CefSettingKeys.CACHEPATH:
                    cefSettings.CachePath = setting.Value;
                    break;

                case CefSettingKeys.USERDATAPATH:
                    cefSettings.UserDataPath = setting.Value;
                    break;

                case CefSettingKeys.PERSISTSESSIONCOOKIES:
                    if (setting.Value.TryParseBoolean(out boolResult))
                    {
                        cefSettings.PersistSessionCookies = boolResult;
                    }

                    break;

                case CefSettingKeys.PERSISTUSERPREFERENCES:
                    if (setting.Value.TryParseBoolean(out boolResult))
                    {
                        cefSettings.PersistUserPreferences = boolResult;
                    }

                    break;

                case CefSettingKeys.USERAGENT:
                    cefSettings.UserAgent = setting.Value;
                    break;

                case CefSettingKeys.LOCALE:
                    cefSettings.Locale = setting.Value;
                    break;

                case CefSettingKeys.CEFLOGFILE:
                case CefSettingKeys.LOGFILE:
                    cefSettings.LogFile = setting.Value;
                    break;

                case CefSettingKeys.LOGSEVERITY:
                    switch (setting.Value.ToUpperInvariant())
                    {
                        case LogSeverityOption.DEFAULT:
                            cefSettings.LogSeverity = CefLogSeverity.Default;
                            break;
                        case LogSeverityOption.VERBOSE:
                            cefSettings.LogSeverity = CefLogSeverity.Verbose;
                            break;
                        case LogSeverityOption.INFO:
                            cefSettings.LogSeverity = CefLogSeverity.Info;
                            break;
                        case LogSeverityOption.ERROR:
                            cefSettings.LogSeverity = CefLogSeverity.Warning;
                            break;
                        case LogSeverityOption.EXTERNAL:
                            cefSettings.LogSeverity = CefLogSeverity.Error;
                            break;
                        case LogSeverityOption.FATAL:
                            cefSettings.LogSeverity = CefLogSeverity.Fatal;
                            break;
                        case LogSeverityOption.DISABLE:
                            cefSettings.LogSeverity = CefLogSeverity.Disable;
                            break;
                    }

                    break;

                case CefSettingKeys.JAVASCRIPTFLAGS:
                    cefSettings.JavaScriptFlags = setting.Value;
                    break;

                case CefSettingKeys.RESOURCESDIRPATH:
                    cefSettings.ResourcesDirPath = setting.Value;
                    break;

                case CefSettingKeys.LOCALESDIRPATH:
                    cefSettings.LocalesDirPath = setting.Value;
                    break;

                case CefSettingKeys.PACKLOADINGDISABLED:
                    if (setting.Value.TryParseBoolean(out boolResult))
                    {
                        cefSettings.PackLoadingDisabled = boolResult;
                    }

                    break;

                case CefSettingKeys.REMOTEDEBUGGINGPORT:
                    if (setting.Value.TryParseInteger(out intResult))
                    {
                        cefSettings.RemoteDebuggingPort = intResult;
                    }

                    break;

                case CefSettingKeys.UNCAUGHTEXCEPTIONSTACKSIZE:
                    if (setting.Value.TryParseInteger(out intResult))
                    {
                        cefSettings.UncaughtExceptionStackSize = intResult;
                    }

                    break;

                case CefSettingKeys.ACCEPTLANGUAGELIST:
                    cefSettings.AcceptLanguageList = setting.Value;
                    break;

                // Not supported by CefGlue
                case CefSettingKeys.FOCUSEDNODECHANGEDENABLED:
                    break;

                // MacOS Only
                case CefSettingKeys.FRAMEWORKDIRPATH:
                    cefSettings.FrameworkDirPath = setting.Value;
                    break;
                // MacOS Only
                case CefSettingKeys.MAINBUNDLEPATH:
                    cefSettings.MainBundlePath = setting.Value;
                    break;
                default:
                    break;
            }
        }
    }
}