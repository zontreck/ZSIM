using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;



namespace nBuild.Source
{
    class nBuild
    {
        private static readonly object flock = new object();
        public static void Main(string[] args)
        {
            //args = new[] { "--load", "nbuild.example.json" };
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
                    OutputType = "exe",
                    OutputPath = "bin"
                };
                ProjectReference ref1 = new ProjectReference();
                ref1.PackageReference = "ExampleProjRef";

                exampleEntry.ProjectReferences.Add(ref1);

                ProjectReference ref2 = new ProjectReference();
                ref2.ReferenceName = "ExampleComRef";
                ref2.ReferencePath = "../example/path/com.exe";

                exampleEntry.ProjectReferences.Add(ref2);
                example.Projects.Add(exampleEntry);

                lock (flock)
                {

                    File.WriteAllText("nbuild.example.json", JsonConvert.SerializeObject(example, Formatting.Indented));
                    Environment.Exit(1);
                }
            }
            else
            {
                // Proceed
                if(args[0] != "")
                {
                    bool delMode = false;
                    if (args[0] == "--delete") delMode = true;


                    string configFile = args[1];
                    nSolution sol = JsonConvert.DeserializeObject<nSolution>(File.ReadAllText(configFile));
                    // start constructing the solution
                    if (File.Exists(sol.SolutionName + ".sln")) File.Delete(sol.SolutionName + ".sln");


                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Microsoft Visual Studio Solution File, Format Version 12.00");
                    sb.AppendLine("# Visual Studio Version 16");
                    sb.AppendLine("VisualStudioVersion = 16.0.29613.14");
                    sb.AppendLine("MinimumVisualStudioVersion = 10.0.40219.1");

                    lock (flock)
                    {

                        File.AppendAllText(sol.SolutionName + ".sln", sb.ToString());
                    }

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

                        if (entry.Framework == null) entry.Framework = "netcoreapp3.0";

                        StringBuilder csproj = new StringBuilder();
                        csproj.AppendLine("<Project Sdk=\"Microsoft.NET.Sdk\">");
                        csproj.AppendLine("<PropertyGroup>" +
                            "\n<OutputType>" + entry.OutputType + "</OutputType>");
                        csproj.AppendLine("<TargetFramework>"+entry.Framework+"</TargetFramework>");
                        if (entry.UnsafeBlocks)
                            csproj.AppendLine("<AllowUnsafeBlocks>true</AllowUnsafeBlocks>");
                        csproj.AppendLine("<CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>");
                        csproj.AppendLine("<GenerateAssemblyInfo>false</GenerateAssemblyInfo>");
                        csproj.AppendLine("<AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>");
                        csproj.AppendLine("<Configurations>Debug;Release</Configurations>");
                        if (entry.OutputPath == null)
                            csproj.AppendLine("<OutputPath>" + Directory.GetCurrentDirectory().Replace("\\", "/") + "/bin</OutputPath>");
                        else
                            csproj.AppendLine("<OutputPath>" + entry.OutputPath + "</OutputPath>");
                        csproj.AppendLine("</PropertyGroup>");

                        csproj.AppendLine("<ItemGroup>");
                        foreach(ProjectReference reference in sol.GlobalReferences)
                        {
                            if (reference.PackageReference != null && reference.ProjectRef == null)
                            {
                                csproj.Append("\n<PackageReference Include=\"" + reference.PackageReference + "\" ");
                                if (reference.PackageVersion != null)
                                    csproj.Append("Version=\"" + reference.PackageVersion + "\" />");
                                else
                                    csproj.Append(" />\n");
                            }
                            else if (reference.PackageReference == null && reference.ProjectRef == null)
                            {
                                csproj.AppendLine("<Reference Include=\"" + reference.ReferenceName + "\">");
                                csproj.AppendLine("  <HintPath>" + reference.ReferencePath + "</HintPath>");
                                csproj.AppendLine("</Reference>");
                            }
                            else if (reference.PackageReference == null && reference.ProjectRef != null)
                            {
                                if (reference.ProjectRef.Contains(entry.ProjectName + ".csproj")) continue;
                                csproj.AppendLine("<ProjectReference Include=\"" + Path.Combine(Directory.GetCurrentDirectory().Replace("\\", "/"),reference.ProjectRef) + "\" />");
                            }
                            else
                            {
                                csproj.AppendLine("<error>Unknown state</error>");
                            }

                        }
                        foreach(ProjectReference reference in entry.ProjectReferences)
                        {
                            if (reference.PackageReference != null && reference.ProjectRef==null)
                            {
                                csproj.Append("\n<PackageReference Include=\"" + reference.PackageReference + "\" ");
                                if (reference.PackageVersion != null)
                                    csproj.Append("Version=\"" + reference.PackageVersion + "\" />");
                                else
                                    csproj.Append(" />\n");
                            }
                            else if(reference.PackageReference == null && reference.ProjectRef==null)
                            {
                                csproj.AppendLine("<Reference Include=\"" + reference.ReferenceName + "\">");
                                csproj.AppendLine("  <HintPath>" + reference.ReferencePath + "</HintPath>");
                                csproj.AppendLine("</Reference>");
                            } else if(reference.PackageReference==null && reference.ProjectRef != null)
                            {
                                csproj.AppendLine("<ProjectReference Include=\"" + reference.ProjectRef + "\" />");
                            }
                            else
                            {
                                csproj.AppendLine("<error>Unknown state</error>");
                            }
                        }

                        csproj.AppendLine("</ItemGroup>");
                        foreach (string exclu in entry.Excludes)
                        {
                            csproj.AppendLine($"<ItemGroup>\n<Compile Remove=\"{exclu}\" />\n<EmbeddedResource Remove=\"{exclu}\" />\n<None Remove=\"{exclu}\" /></ItemGroup>");
                        }

                        csproj.AppendLine("</Project>");
                        lock (flock)
                        {

                            if (entry.SolutionEntryOnly == false)
                                File.WriteAllText(entry.ProjectFolder + "/" + entry.ProjectName + ".csproj", csproj.ToString());

                            if (delMode) File.Delete(entry.ProjectFolder + "/" + entry.ProjectName + ".csproj");
                        }
                    }

                    globalSect.AppendLine("EndGlobalSection");
                    globalSect.AppendLine("GlobalSection(SolutionProperties) = preSolution\nHideSolutionNode = FALSE\nEndGlobalSection\nEndGlobal");

                    lock (flock)
                    {

                        File.AppendAllText(sol.SolutionName + ".sln", projectList.ToString());
                        File.AppendAllText(sol.SolutionName + ".sln", globalSect.ToString());


                        if (delMode)
                        {
                            Console.WriteLine("Deleted all generated solution files");
                            File.Delete(sol.SolutionName + ".sln");
                        }
                    }
                }
            }

        }
    }
}
