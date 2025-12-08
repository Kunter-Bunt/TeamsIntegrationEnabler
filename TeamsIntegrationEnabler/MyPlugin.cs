using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace TeamsIntegrationEnabler
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Teams Integration Enabler"),
        ExportMetadata("Description", "Tool to activate Microsoft Teams Integration on custom tables"),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAASaSURBVFhH7ZZdbBRVFMf/Z+ZOoAFpiSamdNvO7S6JaAJGShMSoEhqfDEQrJGQ8GKCoC/GxIfyxIsP+qAxUTQgzxBsBIOgPhQJxSoKhPgRjR+z3Lt0RWK0bA1JYXfmHh9mZrk7u5WiJjzoL9nsnP85596z5947e4H/+a9DWWEu9Pb23uc4zssAhpiZieikMWakVCp9n429FS0LKBQK+SiKnmHmJY7jnKtWq/vK5fIMbk5+BkBHJq1ijFmdFpHL5dpc191JRKuI6LLrunuDIChmcpoLkFIOMPPHRLQw1Zj5fBiG68rl8oyU8iiAjY1Zdd5XSm3K5XJtQojTRNSfOpj5GoAhrfUXdoJjG4gDX7MnBwAi6nddd2dibrB9GTYAgBBihz054jEWAnjV1tCqAACrsgLiAVK9qWstGMgKCU16UwFE9HNWQ6xfTh5PZFw2JzG3Meo0FdCqTcx8zXXdvQBgjNkFoJKNSTbhCAA4jvN2suZZXskKblaoVCpnFy9e/DuA+5l5AYDPAWy7ePHiNwAwPT39W3t7+1EiygHoZuYqEX1kjNmanoCrV69OdXR0nASwDEAnEZWZebfWek92vjtOw4YaHBwUk5OT240xa4lIpDozn9Ba7/d9fxDANiJaZOdZTDPzAa31uJRyO4BHUgczh0Q00dPTs398fDxM9YYCfN9/l4iGbY2Za0KIvlqttsZxnIPZnBYwET1WrVa/EkK8TkR3M/ODRNSOeLzDWusn0uD6YPl8fqUx5nxqpzDzJ1rrdb7vB0SUz/pTkr2wD8AepdSPGbfr+/4QEe0CsD6KopWXLl26APsUGGP6GlJu8iviIzSbH8z8ExH112q1EQBDUspTvu9XpJShlLIspTxIRJFS6mEATzuOk0tz6wUw82yt5eS7pT+ZfC0RzfM871sAbwIYTFruAugC8CSAMSnlEdd1R7XWx9L8Vu+BOZO0fRhADzOfAiAt9wwzf2rZALA5DMOxzs7OtlT4RwUQ0b5arRYAOARgge1j5g+11oO2hjhnYP78+S+ldr0AIpqpR80RY8xbnuc9BaBpfxBRBCDK6oiLezaXy3XBLsBxnC+t9Z4LKnnzbck6bgUReZ7nbYZdQLFYnGTm4w2RMfOS72yHfkD8a1Yk9gwzHwYwCmCUiM4k+qj1OZ0mp3kNOzufz3dHUfRZ8p5P+U4p9YDv+2etv2Qw8xGt9bCUMgTgMvOE1nr9bG1HfEvq8jyvnJiHlFJbGzZhsVicFEKsZuZj1nIs6+3t9QEcsGOJ6J7k8Upir0nOPSefdxDfsFKbrcnreU2nIAiCstZ6o+u6PcnVa1MURX8IIfYy89dW6PLknE9Y2u0wgewS3IpCoZALw/ADIlqOeBkeJSIDYCwbC2BUKbVFStlqY/9ijOkrlUrXmzrwVwRBUBZCDDDz88x8DsBzSqkTAN7LxgK4d+nSpV1ZEfFyvVAqla7jdjswG4VCYVEYhmNE1HTna8GLSqndqdF0I/o7TE1N3WhrazsohLiLmR8iolbjXiGiHUqpN2zxX+mATXd39xIhxOPMvCK5il8BMOG67vEgCG5k4+84fwKmDuSQNvRMygAAAABJRU5ErkJggg=="),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "LightGray"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class MyPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new MyPluginControl();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public MyPlugin()
        {
            // If you have external assemblies that you need to load, uncomment the following to 
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}