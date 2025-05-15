using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PopupSystem.Editor
{
    [InitializeOnLoad]
    public static class ScreenTypeAutoGenerator
    {
        private const string enumPath = "Assets/Submodules/popup-system/Runtime/Scripts/Scenes/Base/Enums/ScreenType.cs";
        private static readonly string[] customValues = { "None", "Core", "Meta" };

        static ScreenTypeAutoGenerator()
        {
            Debug.Log("Generating Screen Type...");
            GenerateScreenTypeEnum();
        }

        private static void GenerateScreenTypeEnum()
        {
            var sceneFiles = Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories);
            var scenes = sceneFiles
                .Select(Path.GetFileNameWithoutExtension)
                .Distinct()
                .Except(customValues)
                .ToList();

            using (var writer = new StreamWriter(enumPath, false))
            {
                writer.WriteLine("namespace PopupSystem.Runtime");
                writer.WriteLine("{");
                writer.WriteLine("    public enum ScreenType");
                writer.WriteLine("    {");

                foreach (var val in customValues)
                {
                    writer.WriteLine($"        {val},");
                }

                foreach (var scene in scenes)
                {
                    writer.WriteLine($"        {scene},");
                }

                writer.WriteLine("    }");
                writer.WriteLine("}");
            }

            AssetDatabase.Refresh();
            Debug.Log("Generation finished.");
        }
    }
}
