using System;

namespace ZSim{
    public class Globals{
        public const string Version = "1029";
        public const string VerNormal = "1.0.2.9";
        public const string VersionInfo = "ZSim " + Version + " Dev";

        /*
         * The above version number should only use the following Build identifiers
         * 
         * Dev
         * PR
         * RC
         * Alpha
         * Beta
         * Release
         * 
         */

        public const int VERSIONINFO_VERSION_LENGTH = 27;

        /// <value>
        /// This is the external interface version.  It is separate from the OpenSimulator project version.
        ///
        /// </value>
        /// Commented because it's not used anymore, see below for new
        /// versioning method.
        //public readonly static int MajorInterfaceVersion = 8;

        /// <summary>
        /// This rules versioning regarding teleports, and compatibility between simulators in that regard.
        /// </summary>
        ///
        /// <remarks>
        /// The protocol version that we will use for outgoing transfers
        /// Valid values are
        /// "SIMULATION/0.8"
        ///     - up to 45 avatar textures - 11 baked
        /// "SIMULATION/0.7"
        ///     TP uses call back again
        /// "SIMULATION/0.3"
        ///   - supports teleports to variable-sized regions
        ///   - Older versions can teleport to this one, but only if the destination region
        ///     is 256x256
        /// "SIMULATION/0.2"
        ///   - A source simulator which only implements "SIMULATION/0.1" can still teleport here
        ///   - this protocol is more efficient than "SIMULATION/0.1"
        /// "SIMULATION/0.1"
        ///   - this is an older teleport protocol used in OpenSimulator 0.7.5 and before.
        /// </remarks>
        public readonly static float SimulationServiceVersionAcceptedMin = 0.3f;
        public readonly static float SimulationServiceVersionAcceptedMax = 0.8f;
        public readonly static float SimulationServiceVersionSupportedMin = 0.3f;
        public readonly static float SimulationServiceVersionSupportedMax = 0.8f;
    }
}