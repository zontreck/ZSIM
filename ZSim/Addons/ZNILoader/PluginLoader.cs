using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Linq;

namespace ZSim.Addons.ZNILoader
{
    public sealed class PluginLoader
    {
        private static readonly object lck = new object();
        private static PluginLoader inst;
        class Plugin
        {
            public AppDomain domain;
            public string PluginName;
            public string DLL;
            public object PluginInstance;
        }
        private List<Plugin> Plugins = new List<Plugin>();
        private Dictionary<string, Timer> _Timers = new Dictionary<string, Timer>();
        static PluginLoader() { 
            
        }
        public static PluginLoader a
        {
            get
            {
                if (inst != null) return inst;
                else
                {
                    lock (lck)
                    {
                        if (inst == null)
                        {
                            inst = new PluginLoader();
                        }
                        return inst;
                    }
                }
            }
        }
        public T LoadDLL<T>(string PluginName, string DLL)
        {
            // load the DLL
            AppDomain newPlugin = AppDomain.CreateDomain(PluginName);
            T plgInstance = (T)newPlugin.CreateInstanceFromAndUnwrap(DLL, typeof(T).FullName);
            Plugin p = new Plugin();
            p.DLL = DLL;
            p.domain = newPlugin;
            p.PluginInstance = plgInstance;
            p.PluginName = PluginName;

            Plugins.Add(p);

            return plgInstance;
        }

        public void Unload(string ASM)
        {
            Timer vTimer = new Timer(vt =>
            {
                try
                {
                    Plugin v = Plugins.Where(r => r.PluginName == ASM).First();
                    AppDomain.Unload(v.domain);


                    Plugins.Remove(v);

                    _Timers.Remove((string)vt);
                }catch(CannotUnloadAppDomainException e)
                {
                    // this will be attempted again
                }
                
            }, ASM,0,30);

            _Timers.Add(ASM, vTimer);
        }

        public static List<T> Scan<T>(Assembly a)
        {
            List<T> plugins = new List<T>();
            foreach(Type v in a.GetTypes())
            {
                Type check = v.GetInterface(typeof(T).Name);
                if (check == null)
                {
                    // not a plugin
                }
                else
                {
                    T plg = (T)Activator.CreateInstance(v);
                    plugins.Add(plg);
                }
            }
            return plugins;
        }

    }
}
