using System;

namespace ZSim{
    public class Globals{
        private enum BuildType{
            Dev,
            Alpha,
            Beta,
            Internal,
            PR,
            Release,
            RC
        }
        public static string Version => "1029";
        public static string VerNormal => "1.0.2.9";
        public static string VersionInfo{
            get{
                return "ZSim "+Version+" " + BuildType.Dev;
            }
        }
    }
}