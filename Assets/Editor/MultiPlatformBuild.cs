using UnityEditor;
using UnityEngine;

public class MultiPlatformBuild
{
    public static void PerformBuild()
    {
        string[] scenes = { "Assets/Scenes/LambAndTiger.unity"}; // Add your scene paths here
        Debug.Log("Builds completed successfully!");

        // Standalone Windows
        BuildPlayerOptions winBuildOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = "Builds/PC/WindowsBuild.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };
        BuildPipeline.BuildPlayer(winBuildOptions);

        // Standalone Linux
        BuildPlayerOptions linuxBuildOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = "Builds/Linux/LinuxBuild.x86_64",
            target = BuildTarget.StandaloneLinux64,
            options = BuildOptions.None
        };
        BuildPipeline.BuildPlayer(linuxBuildOptions);

        // Android
        BuildPlayerOptions androidBuildOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = "Builds/Android/AndroidBuild.apk",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };
        BuildPipeline.BuildPlayer(androidBuildOptions);

        Debug.Log("Builds completed successfully!");
    }
}
