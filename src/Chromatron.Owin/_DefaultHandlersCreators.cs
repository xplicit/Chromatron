// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Owin;

internal sealed class DefaultOwinSchemeHandlerFactory : OwinSchemeHandlerFactory, IDefaultOwinCustomHandler
{
    public DefaultOwinSchemeHandlerFactory(IOwinPipeline owinPipeline, IChromatronErrorHandler errorHandler) : base(owinPipeline, errorHandler)
    {
    }
}