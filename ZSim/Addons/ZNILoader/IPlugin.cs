using System;
using System.Collections.Generic;
using System.Text;

namespace ZSim.Addons.ZNILoader
{
    public interface IPlugin
    {
        public void Start(); // The most basic entry point for a simple plugin
    }


    public class PluginMethod : Attribute
    {
        public string Command;
        public int ArgumentCount;
        public string Usage;
        public string Group;
    }

    public class PluginClass : Attribute
    {
        public string Path;
        public string VersionStr;
        public string ShortName;
        public int APIVersion;
        public string ID;
    }
}
