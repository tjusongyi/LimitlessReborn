using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Helper class, to move the /Plugins/ folder from the /Best HTTP (Pro|Basic)/ folder to the root /Assets/ folder without any user interaction.
/// It's using the InitializeOnLoad attribute to call the static contstructor of this class where we will move all files and folder to the right place.
/// </summary>
[InitializeOnLoad]
class BestHTTPInstaller
{
    private static bool IsPro = true;
    private static string PluginsDir { get { return Path.Combine(Application.dataPath, "Plugins"); } }
    private static string BestHTTPPluginsDir { get { return Path.Combine(Application.dataPath, Path.Combine(string.Format("Best HTTP ({0})", IsPro ? "Pro" : "Basic"), "Plugins")); } }

    static BestHTTPInstaller()
    {
        try
        {
            // For the time of the package creation the Plugins dir must stay in the original location, so if this folder exists, we do nothing.
            if (Directory.Exists(Path.Combine(Application.dataPath, "__Build__")))
                return;

            // Go away, nothing to see here.
            if (!Directory.Exists(BestHTTPPluginsDir))
                return;

            // Move the directory and all of it's content to the destination. It will overwrite all old files.
            MoveDirectory(BestHTTPPluginsDir, PluginsDir);

            // Refresh the Project explorer
            AssetDatabase.Refresh();
        }
        catch(Exception e)
        {
            Debug.LogError(string.Format("BestHTTP - Installation of the Plugins folder failed. You have to move the /Plugins/ folder from the BestHTTP folder to the root /Assets/ folder.\nThe error was: {0}\n{1}", e.Message, e.StackTrace));
        }
    }

    public static void MoveDirectory(string source, string target)
    {
        var stack = new Stack<Folders>();
        stack.Push(new Folders(source, target));

        while (stack.Count > 0)
        {
            var folders = stack.Pop();
            Directory.CreateDirectory(folders.Target);
            foreach (var file in Directory.GetFiles(folders.Source, "*.*"))
            {
                string targetFile = Path.Combine(folders.Target, Path.GetFileName(file.Replace(".dll_", ".dll")));
                if (File.Exists(targetFile))
                    File.Delete(targetFile);
                File.Move(file, targetFile);
            }

            foreach (var folder in Directory.GetDirectories(folders.Source))
                stack.Push(new Folders(folder, Path.Combine(folders.Target, Path.GetFileName(folder))));
        }
        Directory.Delete(source, true);
    }

    private class Folders
    {
        public string Source { get; private set; }
        public string Target { get; private set; }

        public Folders(string source, string target)
        {
            Source = source;
            Target = target;
        }
    }
}