using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Utility {
    public static T LoadResource<T>(string path) {
        object obj = null;

        Type type = typeof(T);
        if (type == typeof(byte[]) || type == typeof(string))
            type = typeof(TextAsset);

#if UNITY_EDITOR && !UNITY_WEBPLAYER
        obj = AssetDatabase.LoadAssetAtPath("Assets/Patches/" + path, type);
#elif UNITY_STANDALONE_WIN
        if(!path.StartsWith("/")) {
#if UNITY_EDITOR
            path = Path.Combine(Path.Combine(Application.dataPath, "Patches/"), path);
#else
            path = Path.Combine(Path.Combine(Application.dataPath, "../Client/Assets/Patches/"), path);
#endif
        }
        path = path.Replace('\\', '/');

        var www = new WWW("file://" + path);
        CoroutineManager.Perform(PerformLoadResource(www, type));

        while (!www.isDone) {}

        if (type == typeof(byte[]))
            obj = www.bytes;
        else if (type == typeof(string))
            obj = www.text.Replace("\ufeff", "");
        else if (type == typeof(Texture2D))
            obj = www.texture;
        else if (type == typeof(AudioClip))
			obj = www.audioClip;
#endif

        if (obj == null)
            obj = AssetBundleManager.Get().GetAsset(Path.GetFileName(path), type);

        if (obj == null) {
            Debug.LogWarning("Utility::LoadResource() Failed to load " + path);
        } else {
            if (obj.GetType() == typeof(TextAsset)) {
                if (type == typeof(byte[])) {
                    return (T)(object)((TextAsset)obj).bytes;
                } else if (type == typeof(string)) {
                    return (T)(object)((TextAsset)obj).text.Replace("\ufeff", "");
                } else {
                    Debug.LogWarning("Utility::LoadResource() Not supported TextAsset. path=" + path);
                }
            }
        }

        return (T)(object)obj;
    }
}
