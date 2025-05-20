## Setup
- Unity version: [6000.0.40f1](https://unity.com/releases/editor/whats-new/6000.0.40#installs)
- Android Module

## Issue
- Unity build via script [BuildPipeline.BuildPlayer](https://docs.unity3d.com/ScriptReference/BuildPipeline.BuildPlayer.html) always sets default texture format to PVRTC
- PVRTC causes major performance issues on Android.

## Steps to make the build
- Switch to Android platform
- Make sure Build Settings has **ASTC** in `Texture Compression Formats`
- Change `Platform Settings > Android Settings > Texture Compression` to `ASTC`
- In Menu, go to `Tools > Build` [Build Script here](https://github.com/AvinashP/Unity6TextureCompressionBug/blob/main/Assets/Editor/Scripts/BuildCLI.cs)
- Check `Builds` folder outside Assets.

## How to verify default texture format is set to PVRTC
- Open ./ApkTool folder in terminal (Its outside Assets in this repository)
- Run below command (Make sure you have java command setup in PATH)
- ` .\apktool.bat d PathInYourMachine\TextureFormatText\Builds\TextureCompressionTest.apk`
- If will decompile the APK in same folder
- Open `AndroidManifest.xml` in some editor
- You will notice below entry
- ` <supports-gl-texture android:name="GL_IMG_texture_compression_pvrtc"/>`
- Also after build, build setting will reset to have PVRTC in `Platform Settings`

You can verify this in other ways too. Feel free to run profiler or read Android manifest.
