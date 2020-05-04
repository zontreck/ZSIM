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