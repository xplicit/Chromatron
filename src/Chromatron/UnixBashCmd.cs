// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron;

public static class UnixBashCmd
{
    public static void Exec(string cmd)
    {
        var escapedArgs = cmd.Replace("\"", "\\\"");

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "/bin/bash",
                Arguments = $"-c \"{escapedArgs}\""
            }
        };

        process.Start();
        process.WaitForExit();
    }
}