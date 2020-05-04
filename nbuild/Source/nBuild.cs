using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;



namespace nBuild.Source
{
    class nBuild
    {
        public static void Main(string[] args)
        {
            args = new[] { "--load", "nbuild.example.json" };
            // Check arguments
            if (args.Length == 0)
            {
                Console.WriteLine("ERROR: --load <nbuild.conf>");

                Console.WriteLine("Writing sample config file to ... nbuild.example.json");
                nSolution example = new nSolution();
                example.SolutionName = "Example";
                example.Projects = new List<ProjectEntry>();
                ProjectEntry exampleEntry = new ProjectEntry
                {
                    ProjectName = "ExampleProject",
                    ProjectFolder = ".",
                    ProjectReferences = new List<ProjectReference>(),
                    Excludes = new List<string>(),
                    OutputType = "exe"
                };
                example.Projects.Add(exampleEntry);

                File.WriteAllText("nbuild.example.json", JsonConvert.SerializeObject(example, Formatting.Indented));
                Environment.Exit(1);
            }
            else
            {
                // Proceed
                if(args[0] == "--load")
                {
                    string configFile = args[1];
                    nSolution sol = JsonConvert.DeserializeObject<nSolution>(File.ReadAllText(configFile));
                    // start constructing the solution
                    if (File.Exists(sol.SolutionName + ".sln")) File.Delete(sol.SolutionName + ".sln");


                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Microsoft Visual Studio Solution File, Format Version 12.00");
                    sb.AppendLine("# Visual Studio Version 16");
                    sb.AppendLine("VisualStudioVersion = 16.0.29613.14");
                    sb.AppendLine("MinimumVisualStudioVersion = 10.0.40219.1");

                    File.AppendAllText(sol.SolutionName+".sln", sb.ToString());

                    sb.Clear();

                    _ = sb;

                    StringBuilder projectList = new StringBuilder();
                    StringBuilder globalSect = new StringBuilder();
                    globalSect.AppendLine("Global");
                    globalSect.AppendLine("GlobalSection(SolutionConfigurationPlatforms) = preSolution");
                    globalSect.AppendLine("Debug|Any CPU = Debug|Any CPU");
                    globalSect.AppendLine("Release|Any CPU = Release|Any CPU");
                    globalSect.AppendLine("EndGlobalSection");
                    globalSect.AppendLine("GlobalSection(ProjectConfigurationPlatforms) = postSolution");
                    string solutionGuid = Guid.NewGuid().ToString();
                    foreach (ProjectEntry entry in sol.Projects)
                    {
                        Console.WriteLine("Generating project: " + entry.ProjectFolder + "/" + entry.ProjectName + ".csproj");
                        string ProjectGuid = Guid.NewGuid().ToString();
                        projectList.AppendLine("Project(\"{" + solutionGuid + "}\") = \"" + entry.ProjectName + "\", \"" + Path.Join(entry.ProjectFolder, entry.ProjectName + ".csproj") + "\", \"{" + ProjectGuid + "}\"");
                        projectList.AppendLine("EndProject");

                        globalSect.AppendLine("{" + ProjectGuid + "}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
                        globalSect.AppendLine("{" + ProjectGuid + "}.Debug|Any CPU.Build.0 = Debug|Any CPU");
                        globalSect.AppendLine("{" + ProjectGuid + "}.Release|Any CPU.ActiveCfg = Release|Any CPU");
                        globalSect.AppendLine("{" + ProjectGuid + "}.Release|Any CPU.Build.0 = Release|Any CPU");

                        // now generate the csproj

                        StringBuilder csproj = new StringBuilder();
                        csproj.AppendLine("<Project Sdk=\"Microsoft.NET.Sdk\">");
                        csproj.AppendLine("<PropertyGroup>" +
                            "\n<OutputType>" + entry.OutputType + "</OutputType>");
                        csproj.AppendLine("<TargetFramework>netcoreapp3.0</TargetFramework>");
                        csproj.AppendLine("<CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>");
                        csproj.AppendLine("<GenerateAssemblyInfo>false</GenerateAssemblyInfo>");
                        csproj.AppendLine("<Configurations>Debug;Release</Configurations>");
                        csproj.AppendLine("<OutputPath>"+Directory.GetCurrentDirectory().Replace("\\", "/")+"/bin</OutputPath>");
                        csproj.AppendLine("</PropertyGroup>");

                        csproj.AppendLine("<ItemGroup>");
                        foreach(ProjectReference reference in entry.ProjectReferences)
                        {
                            csproj.AppendLine("<PackageReference Include=\"" + reference.Reference + "\" />");
                        }
                        csproj.AppendLine("</ItemGroup>");

                        csproj.AppendLine("</Project>");


                        File.WriteAllText(entry.ProjectFolder + "/" + entry.ProjectName + ".csproj", csproj.ToString());
                    }

                    globalSect.AppendLine("EndGlobalSection");
                    globalSect.AppendLine("GlobalSection(SolutionProperties) = preSolution\nHideSolutionNode = FALSE\nEndGlobalSection\nEndGlobal");

                    File.AppendAllText(sol.SolutionName + ".sln", projectList.ToString());
                    File.AppendAllText(sol.SolutionName + ".sln", globalSect.ToString());

                }
            }

        }
    }
}
