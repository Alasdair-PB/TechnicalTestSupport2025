using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class PluginManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI proText;

    const string DLL_NAME = "libtimedatalocale.so";

    [DllImport(DLL_NAME)]
    private static extern IntPtr PrintHello();

    [DllImport(DLL_NAME)]
    private static extern int PrintANumber();

    [DllImport(DLL_NAME)]
    private static extern int AddTwoIntegers(int a, int b);

    [DllImport(DLL_NAME)]
    private static extern float AddTwoFloats(float a, float b);

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        
        string hello = Marshal.PtrToStringAuto(PrintHello());
        proText.text = hello;

#else
        proText.text = "JNI call only works on Android device.";
        Debug.Log("JNI call only works on Android device.");
#endif
    }
}
