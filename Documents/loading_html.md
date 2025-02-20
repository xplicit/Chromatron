
# Loading Html

Chromatron html loading (start url) can be done in [3 different](https://github.com/xplicit/Chromatron/blob/5086087dfc03d27cbd84699359cd4891024cfcd1/src/Chromatron.Core/Helpers/ConfigKeys.cs#L3) ways.

- A real website URL
- Local/Embedded Resource Loading
- File Protocol

## A Real Website URL

This will launch actual website URL. This may not necessarily be commonly used as Chromatron is focused on loading local HTML5 files for Single Page Applications (SPAs). This URL should also be of scheme and domain combination that is not registered as external URL scheme. 

If the URL is pre-regisered as an external url scheme type, the URL will be opened by the OS default browser. Please see more info at [Registering Url Schemes](https://github.com/xplicit/Chromatron/blob/master/Documents/registering_url_schemes.md).

To load/start url and its assets, you can do it in configuration object:

````csharp
    public class DefaultConfiguration : IChromatronConfiguration
    {
        public DefaultConfiguration()
        {
           StartUrl = "https://google.com";
        }
    }
````

## Local Resource Loading

This is the preferred way of loading local HTML5 files. 

Loading html and associated files via LocalResource needs a custom resource scheme handler. For more info on scheme handling, please see [Registering Resource Handlers](https://github.com/xplicit/Chromatron/blob/master/Documents/registering_resource_handlers.md).

To load start url and its assets, you can do it in configuration object:


````csharp
    public class DefaultConfiguration : IChromatronConfiguration
    {
        public DefaultConfiguration()
        {
           StartUrl = "local://app/chromely.html";
        }
    }

````

## File Protocol

Local HTML5 files can also be loaded using file protocol (**file:///**). Using file protocol (**file:///**) is discouraged for security reasons. One issue might be Cross-Origin domain. Although not the preferred way, some developers may find need for it. 

To load start url and its assets, you can do it in configuration object:

````csharp
    public class DefaultConfiguration : IChromelyConfiguration
    {
        public DefaultConfiguration()
        {
			    var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
			    var startUrl = $"file:///{appDirectory}app/chromely.html";
			    StartUrl = startUrl;
        }
    }

````

Notes:
- Folder "app" is a physical folder in the executable folder. Usually this is the primary folder for all html and associated files. Sample - [app folder](https://github.com/chromelyapps/demo-projects/tree/master/regular-chromely/CrossPlatDemo/app). Usually all files in this folder are copied to executable (bin) folder.
- For Angular/React/Vue this is usually the "dist" folder. So the start url usually is [local://dist/index.html](https://github.com/chromelyapps/demo-projects/blob/98732be68154623dd9d7977cf6cbe29e2eed82a0/angular-react-vue/ChromelyAngular/chromelyconfig.json#L4). The "dist" folder is where Webpack, Parcel and other bundlers publish the bundled (end product) for browsers consumption.
