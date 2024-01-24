// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Tests.ChromatronCore;

public class AppSettingsTests
{
    [Fact]
    public void DefaultAppSettingsTest()
    {
        ChromatronAppUser.App.Properties.Settings.Item1 = "Market 01";
        ChromatronAppUser.App.Properties.Settings.Item2 = "Year 2020";

        var list = new List<string>
        {
            "item0001",
            "item0002",
            "item0003"
        };
        ChromatronAppUser.App.Properties.Settings.TestItems = list;

        var config = new DefaultConfiguration();

        // Delete config file if exists
        DeleteConfigFile(config);

        // Save settings
        ChromatronAppUser.App.Properties.Save(config);

        // Read
        ChromatronAppUser.App.Properties.Settings.Item1 = null;
        ChromatronAppUser.App.Properties.Settings.Item2 = null;
        ChromatronAppUser.App.Properties.Settings.TestItems = null;
        ChromatronAppUser.App.Properties.Read(config);

        var item1 = (string)ChromatronAppUser.App.Properties.Settings.Item1;
        var item2 = (string)ChromatronAppUser.App.Properties.Settings.Item2;
        var testItems = (ArrayList)ChromatronAppUser.App.Properties.Settings.TestItems;
        Assert.NotNull(item1);
        Assert.NotNull(item2);
        Assert.NotNull(testItems);

        Assert.Equal("Market 01", item1);
        Assert.Equal("Year 2020", item2);
        Assert.Equal("item0001", testItems[0]);
        Assert.Equal("item0002", testItems[1]);
        Assert.Equal("item0003", testItems[2]);

        // Delete config file if exists
        // Delete config file if exists
        DeleteConfigFile(config);
    }

    private static void DeleteConfigFile(IChromatronConfiguration config)
    {
        try
        {
            var appSettingsFile = AppSettingInfo.GetSettingsFilePath(config.Platform, config.AppName ?? "chromatron");
            if (!string.IsNullOrWhiteSpace(appSettingsFile))
            {
                if (File.Exists(appSettingsFile))
                {
                    File.Delete(appSettingsFile);
                }
            }
        }
        catch { }
    }
}