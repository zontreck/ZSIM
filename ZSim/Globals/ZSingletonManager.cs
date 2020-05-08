/*
 * 
 * COPYRIGHT THE ZSIMULATOR DEVELOPERS 2020
 * 
 * Licensed under the GPLv2
 * 
 */




using System.Collections.Generic;

namespace ZSim.Frameork
{
    public sealed class ZSingletonManager
    {
        private static readonly object lck = new object();
        private static ZSingletonManager inst = null;

        static ZSingletonManager() { }
        public static ZSingletonManager Instance
        {
            get
            {
                if (inst != null) return inst;
                else
                {
                    lock (lck)
                    {
                        if (inst == null) inst = new ZSingletonManager();
                        return inst;
                    }
                }
            }
        }

        public Dictionary<string, object> Singletons { get; set; } = new Dictionary<string, object>();
    }
}
