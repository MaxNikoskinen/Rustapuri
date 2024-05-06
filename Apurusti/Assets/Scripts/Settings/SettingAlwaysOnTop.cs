using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;

public class SettingAlwaysOnTop : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    public static extern long GetWindowRect(IntPtr hWnd, ref RectInt lpRect);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    [SerializeField] private Toggle settingToggle;

    public void ChangeToggle(bool value)
    {
        #if !UNITY_EDITOR
        IntPtr hWnd = GetActiveWindow();

        if(value) //päällä
        {
            RectInt rect = new RectInt();
            GetWindowRect(hWnd, ref rect);
            Debug.Log($"x:{rect.x} y:{rect.y} h:{rect.width} w:{rect.height}");
            SetWindowPos(hWnd, HWND_TOPMOST, rect.x, rect.y, rect.width, rect.height, 0);
        }
        else      //pois
        {
            
        }
        #endif
    }
}
