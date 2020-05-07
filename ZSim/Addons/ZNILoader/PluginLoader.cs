using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ZSim.Addons.ZNILoader
{
    public class PluginLoader
    {

        public static Assembly LoadDLL(string DLL)
        {
            // load the DLL
            return Assembly.LoadFile(DLL);
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
