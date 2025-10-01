using AOT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PluginManager : MonoBehaviour
{
    [SerializeField] UIDocument rootDocumet;

    private const string DLL_NAME = "timedatalocale";
    private Label dateLabel;
    private Label timeLabel;
    private Label localeLabel;

    [DllImport(DLL_NAME)] private static extern int add(int x, int y);
    [DllImport(DLL_NAME)] private static extern IntPtr getLocale();
    [DllImport(DLL_NAME)] private static extern IntPtr getDate();
    [DllImport(DLL_NAME)] private static extern IntPtr getTime();
    [DllImport(DLL_NAME)] private static extern void release();
    [DllImport(DLL_NAME)] private static extern void releaseString(IntPtr intPtr);

    private void AssignUIReferences()
    {
        VisualElement root = rootDocumet.rootVisualElement;
        root.BringToFront();
        dateLabel = root.Q("Date_Label") as Label;
        timeLabel = root.Q("Time_Label") as Label;
        localeLabel = root.Q("Locale_Label") as Label;
    }

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

        dateLabel.text = date;
        localeLabel.text = locale;
        timeLabel.text = time;

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
        AssignUIReferences();
        #if UNITY_ANDROID && !UNITY_EDITOR
            GetDeviceInfo();
        #else
            dateLabel.text = "JNI call only works on Android device.";
            Debug.Log("JNI call only works on Android device.");
        #endif
    }
}
