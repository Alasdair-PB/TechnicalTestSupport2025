#include "native-lib.h"

int add(int num1, int num2)
{
    return num1 + num2;
}

bool get_env(JNIEnv ** env) {
    if (jvm == nullptr) return false;
    int status = jvm->GetEnv((void**) env, JNI_VERSION_1_4);
    if (status != JNI_OK) {
        status = jvm->AttachCurrentThread(env, NULL);
        if(status != JNI_OK)
            return false;
    }
    return true;
}

void release_env(void) {
    JNIEnv *env ;
    int status = jvm->GetEnv((void**)&env, JNI_VERSION_1_4);
    if (status == JNI_EDETACHED) {
        jvm->DetachCurrentThread();
    }
}

const char* getStringMethod(const char* methodName)
{
    JNIEnv *env;
    if (!get_env(&env))
        return nullptr;

    jclass localClass = env->FindClass("com/greathookgames/timedatalocale/InfoManager");
    jmethodID mid = env->GetStaticMethodID(localClass, methodName, "()Ljava/lang/String;");
    auto jstr = (jstring)env->CallStaticObjectMethod(localClass, mid); //CallVoidMethod etc
    const char* temp = env->GetStringUTFChars(jstr, nullptr);
    char* result = strdup(temp);
    env->ReleaseStringUTFChars(jstr, temp);
    return result; // Creates and returns heap copy allowing jstr to be disposed
}

void releaseString(const char* str) {
    if (str) {
        free((void*)str);
    }
}

const char* getLocale() {
    return getStringMethod("getLocale");
}

const char* getDate() {
    return getStringMethod("getDate");
}

const char* getTime() {
    return getStringMethod("getTime");
}

void release()
{
    JNIEnv *env;
    if (!get_env(&env))
        return;
    jclass localClass = env->FindClass("com/greathookgames/timedatalocale/InfoManager");
    jmethodID mt = env->GetMethodID(localClass, "release", "()V");
    env->CallVoidMethod(jobj, mt);
    env->DeleteLocalRef(localClass);
    if (jobj != nullptr) {
        env->DeleteGlobalRef(jobj);
        jobj = nullptr;
    }
    release_env();
}
