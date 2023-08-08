#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class BuildVersionProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        UpdateVersion();
    }

    private void UpdateVersion()
    {   //nollaa versio jos päivämäärä muuttuu
        string[] splitArray = PlayerSettings.bundleVersion.Split(char.Parse("."));
        string[] splitArray2 = splitArray[0].Split(char.Parse("v"));
        int.TryParse(splitArray[3], out int currentVersion);
        int.TryParse(splitArray[1], out int previusMonth);
        int.TryParse(splitArray[2], out int previusDay);
        int.TryParse(splitArray2[1], out int previusYear);
        if(previusDay != DateTime.Now.Day || previusMonth != DateTime.Now.Month || previusYear != DateTime.Now.Year)
        {
            currentVersion = 0;
        }
        //aseta versio
        int newVersion = currentVersion + 1;
        string date = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day;
        PlayerSettings.macOS.buildNumber = newVersion.ToString();
        PlayerSettings.bundleVersion = string.Format("v{0}.{1}", date, newVersion);
    }
}
#endif