//place this script in the Editor folder within Assets.
using System;
using System.Diagnostics;
using UnityEditor;


//to be used on the command line:
//$ Unity -quit -batchmode -executeMethod WebGLBuilder.build

class WebGLBuilder
{
    static void build()
    {
        var scene = GetArg("-scene");
        Console.WriteLine("scene:" + scene);

        var outputPath = GetArg("-outputPath");
        Console.WriteLine("outputPath:" + outputPath);

        if (!scene.Contains("/"))
        {
            scene = "Assets/Scene/" + scene;
        }

        var list = new System.Collections.Generic.List<string>();

        foreach (var item in scene.Split(','))
        {
            if (item != null && !string.IsNullOrEmpty(item.Trim()))
            {
                list.Add(item.Trim() + ".unity");
            }
        }

        if (string.IsNullOrEmpty(outputPath))
        {
            outputPath = "Release";
        }
        
        BuildPipeline.BuildPlayer(list.ToArray(), outputPath, BuildTarget.WebGL, BuildOptions.None);
    }

    private static string GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return null;
    }

}