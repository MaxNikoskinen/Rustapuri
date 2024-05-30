using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

    [SerializeField] private TMP_Text settingsToggle;

    private bool isSettingOn = false;

    private void Start()
    {
        ChangeToggle(PlayerPrefs.GetInt("AlwaysOnTop", 0) != 0);
    }

    public void ButtonAction()
    {
        if(isSettingOn)
        {
            ChangeToggle(false);
        }
        else
        {
            ChangeToggle(true);
        }
    }

    public void ChangeToggle(bool value)
    {
        #if !UNITY_EDITOR
        IntPtr hWnd = GetActiveWindow();

        if(value) //päällä
        {
            RectInt rect = new RectInt();
            GetWindowRect(hWnd, ref rect);
            rect.width = rect.width - rect.x;
            rect.height = rect.height - rect.y;
            SetWindowPos(hWnd, HWND_TOPMOST, rect.x, rect.y, rect.width, rect.height, 0);
            settingsToggle.text = "Kyllä";
            isSettingOn = true;
        }
        else      //pois
        {
            RectInt rect = new RectInt();
            GetWindowRect(hWnd, ref rect);
            rect.width = rect.width - rect.x;
            rect.height = rect.height - rect.y;
            SetWindowPos(hWnd, HWND_NOTOPMOST, rect.x, rect.y, rect.width, rect.height, 0);
            settingsToggle.text = "Ei";
            isSettingOn = false;
        }
        PlayerPrefs.SetInt("AlwaysOnTop", value ? 1 : 0);
        #endif
    }
}
