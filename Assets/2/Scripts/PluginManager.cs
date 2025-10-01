using AOT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class PluginManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI proText;
    const string DLL_NAME = "timedatalocale";

    [DllImport(DLL_NAME)] private static extern int add(int x, int y);
    [DllImport(DLL_NAME)] private static extern IntPtr getLocale();
    [DllImport(DLL_NAME)] private static extern IntPtr getDate();
    [DllImport(DLL_NAME)] private static extern IntPtr getTime();
    [DllImport(DLL_NAME)] private static extern void release();
    [DllImport(DLL_NAME)] private static extern void releaseString(IntPtr intPtr);

    private void GetDeviceInfo()
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass infoManagerClass = new AndroidJavaClass("com.greathookgames.timedatalocale.InfoManager");
        AndroidJavaObject nativeObject = new AndroidJavaObject("com.greathookgames.timedatalocale.InfoManager");

        nativeObject.CallStatic("loadLib");
        nativeObject.Call("init");

        IntPtr localePtr = getLocale();
        IntPtr datePtr = getDate();
        IntPtr timePtr = getTime();

        string locale = Marshal.PtrToStringAnsi(localePtr);
        string date = Marshal.PtrToStringAnsi(datePtr);
        string time = Marshal.PtrToStringAnsi(timePtr);

        releaseString(localePtr);
        releaseString(datePtr);
        releaseString(timePtr);
        proText.text = "res: " + locale + " t: " + time + " d: " + date;

        release();

        unityPlayer.Dispose();
        activity.Dispose();
        infoManagerClass.Dispose();
        nativeObject.Dispose();
    }

    private void PluginAddSample()
    {
        /*string hello = Marshal.PtrToStringAuto(PrintHello());
        int x = add(1, 2); 
        string y = Marshal.PtrToStringAuto(helloworld());*/
    }

    void Start()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            GetDeviceInfo();
        #else
            proText.text = "JNI call only works on Android device.";
            Debug.Log("JNI call only works on Android device.");
        #endif
    }
}
