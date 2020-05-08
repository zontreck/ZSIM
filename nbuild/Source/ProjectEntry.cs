using System;
using System.Collections.Generic;
using System.Text;

namespace nBuild.Source
{
    public class ProjectEntry
    {
        public string ProjectName;
        public string ProjectFolder;
        public string OutputType;
        public List<ProjectReference> ProjectReferences;
        public List<string> Excludes;
        public string OutputPath;

        public bool SolutionEntryOnly = false; // <-- set this if you want to only add this to the solution, not generate a csproj
        public bool UnsafeBlocks = false;
        public string Framework = "netcoreapp3.0";

    }
}
/*
 * 
 * {
 *      "ProjectName": "ZSim",
 *      "ProjectFolder": "ZSim/Region/Application",
 *      "ProjectReferences": [
 *          {
 *              "Reference": "libremetaverse"
 *          }
 *      ],
 *      "Excludes": [
 *          "*.obj"
 *      ]
 * }
 * 
 */