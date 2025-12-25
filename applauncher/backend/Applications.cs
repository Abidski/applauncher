using System;
using System.Collections;
using Microsoft.Win32;
using System.Linq;

namespace applauncher.backend;

public static class Applications
{
    private static List<App> apps = new List<App>();

    //Taken from Claude.ai
    public static List<App> GetDesktopApps()
    
    {
        {
            var registryPaths = new[]
            {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
            };

            foreach (var path in registryPaths)
            {
                try
                {
                    using (var key = Registry.LocalMachine.OpenSubKey(path))
                    {
                        if (key == null) continue;

                        foreach (var subKeyName in key.GetSubKeyNames())
                        {
                            using (var subKey = key.OpenSubKey(subKeyName))
                            {
                                var appPath = subKey?.GetValue("DisplayIcon")?.ToString() ??
                                              subKey?.GetValue("InstallLocation")?.ToString();


                                if (appPath == null) continue;
                                // Skip Windows updates and system component
                                /*
                                var systemComponent = subKey.GetValue("SystemComponent");
                                if (systemComponent != null && systemComponent.ToString() == "1") continue;
                                */
                                string fPath = "";
                                if(appPath.Last().Equals('0'))
                                {
                                    fPath = appPath.Substring(0, appPath.Length - 2);
                                }
                                else
                                {
                                    fPath = appPath;
                                }
                                if (string.IsNullOrEmpty(fPath) || !fPath.EndsWith(".exe"))
                                {
                                    continue;
                                }
                                var displayName = subKey?.GetValue("DisplayName")?.ToString();
                                if (string.IsNullOrWhiteSpace(displayName))
                                {
                                    var indexOfSlash = fPath.LastIndexOf('\\');
                                    if (indexOfSlash != -1)
                                    {
                                        var fileName = fPath.Substring(indexOfSlash + 1);
                                        if (fileName.Length > 4)
                                        {
                                            displayName = fileName.Substring(0,fileName.Length -4 );
                                        }
                                        else
                                        {
                                            displayName = "error";
                                        }
                                    }
                                    
                                }
                                
                                
                                apps.Add(new App(displayName, fPath));
                                Console.WriteLine(fPath);
                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }


        }
        return apps;
    }
    public class App
    {

        private string Name { get; set; }
        public string Path { get; set; }
        public App(string name, string path)
        {
            Name = !string.IsNullOrWhiteSpace(name) ? name : "Unknow App";
            Path = path ?? "";
        }

        public override string ToString()
        {
            return Name;
        }
    }
}