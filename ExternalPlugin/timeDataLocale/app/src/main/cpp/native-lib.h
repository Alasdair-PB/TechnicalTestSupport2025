#include <jni.h>
#include <string>

#ifndef TIMEDATALOCALE_NATIVE_LIB_H
#define TIMEDATALOCALE_NATIVE_LIB_H

static JavaVM *jvm = nullptr;
static jobject jobj;

JNIEXPORT jint JNICALL JNI_OnLoad(JavaVM* vm, void* reserved)
{
    jvm = vm;
    return JNI_VERSION_1_4;
}

extern "C"
{
    bool get_env(JNIEnv ** env);
    void release_env(void);
    void releaseString(const char* str);
    void release();
    const char* getStringMethod(const char* methodName);

    const char* getLocale();
    const char* getDate();
    const char* getTime();

    int add(int num1, int num2);

    /*JNIEXPORT int JNICALL randomCall(JNIEnv* env, jobject) {
        std::string hello = "Hello from C++";
        jstring x = env->NewStringUTF(hello.c_str());
        return 0;
    }*/
}

extern "C"
JNIEXPORT void JNICALL
Java_com_greathookgames_timedatalocale_InfoManager_init(JNIEnv *env, jobject obj) {
    jobj = env->NewGlobalRef(obj);
}

#endif
