// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Network;

public enum UrlSchemeType
{
    None,
    LocalResource,
    FolderResource,
    AssemblyResource,
    LocalRequest,
    ExternalRequest,
    ExternalBrowser,
    Owin,
    Other
}