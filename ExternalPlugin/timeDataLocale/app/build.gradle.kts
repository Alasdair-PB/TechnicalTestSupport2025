plugins {
    id("com.android.library")
    kotlin("android")
}

android {
    namespace = "com.greathookgames.timedatalocale"

    compileSdk = 36
    defaultConfig {
        minSdk = 24
        ndk {
            abiFilters += listOf("arm64-v8a")

        }
        externalNativeBuild {
            cmake {
                cppFlags += "-fms-extensions"
                cFlags += "-fms-extensions"
            }
        }

    }


    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_11
        targetCompatibility = JavaVersion.VERSION_11
    }

    kotlinOptions {
        jvmTarget = "11"
    }
    externalNativeBuild {
        cmake {
            path = file("src/main/cpp/CMakeLists.txt")
            version = "3.22.1"

        }
    }
}

dependencies {

}