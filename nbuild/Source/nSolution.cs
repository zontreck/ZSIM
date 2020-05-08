using System;
using System.Collections.Generic;
using System.Text;

namespace nBuild.Source
{
    public class nSolution
    {
        public List<ProjectEntry> Projects;
        public string SolutionName;
        public List<ProjectReference> GlobalReferences;
    }
}
