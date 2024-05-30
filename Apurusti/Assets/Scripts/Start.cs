
using UnityEngine;
using UnityEngine.Rendering;
using System.Runtime.InteropServices;
public class Start
{
    //Get variables
    [DllImport("user32.dll")]
    public static extern System.IntPtr GetActiveWindow();

    //Window name
    [DllImport("user32.dll")]
    public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);
    
    //Dark title bar
    [DllImport("dwmapi.dll")]
    public static extern int DwmSetWindowAttribute(System.IntPtr hwnd, int attr, ref bool attrValue, int attrSize);


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void OnBeforeSplashScreen()
    {
        #if !UNITY_EDITOR
        //Get variables
        var windowPtr = GetActiveWindow();

        //Window name
        SetWindowText(windowPtr, Application.productName + " " + Application.version);

        //Dark title bar
        var value = true;
        DwmSetWindowAttribute(windowPtr, 20, ref value, System.Runtime.InteropServices.Marshal.SizeOf(value));

        //Skip splash
        System.Threading.Tasks.Task.Run(AsyncSkip);
        #endif

        Application.targetFrameRate = PlayerPrefs.GetInt("FPS", 30);
    }

    private static void AsyncSkip()
    {
        SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
    }
}