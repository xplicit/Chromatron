// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.NativeHosts.LinuxHost;

public partial class ChromatronLinuxHost
{
    ~ChromatronLinuxHost()
    {
        Dispose(false);
    }

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        // If there are managed resources
        if (disposing)
        {
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}