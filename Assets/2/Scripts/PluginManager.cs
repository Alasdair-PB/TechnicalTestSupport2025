using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class PluginManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI proText;

    const string DLL_NAME = "timedatalocale";



    [DllImport(DLL_NAME)]
    private static extern int add(int x, int y);

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        
        //string hello = Marshal.PtrToStringAuto(PrintHello());
        int x = add(1, 2); 
        proText.text = "" + (x);

#else
        proText.text = "JNI call only works on Android device.";
        Debug.Log("JNI call only works on Android device.");
#endif
    }
}
