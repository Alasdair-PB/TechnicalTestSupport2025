package com.greathookgames.timedatalocale;
import java.io.File;
import java.time.LocalDate;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
public class InfoManager {
    public InfoManager() {}

    public static String getLocale() {
        return java.util.Locale.getDefault().toString();
    }

    public static String getDate(){
        LocalDate localDate = LocalDate.now();
        DateTimeFormatter formatter = DateTimeFormatter.ofPattern("dd LLLL yyyy");
        return localDate.format(formatter);
    }

    public static String getTime(){
        LocalTime localTime = LocalTime.now();
        DateTimeFormatter formatter = DateTimeFormatter.ofPattern("HH:mm");
        return localTime.format(formatter);
    }

    public static void loadLib() {
        System.loadLibrary("timedatalocale");
    }
    public native void init();
    public void release(){
        // No data to release, but leaving this here for workflow sample
    }
}
