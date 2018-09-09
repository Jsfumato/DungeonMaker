using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;


public class AssetBundleManager : MonoBehaviour
{
	public delegate void StatusCallback(bool success, string message);
	public delegate void Callback(bool success);
	public delegate void CallbackWithMsg(bool success, string msg);
	public delegate void ProgressCallback(float v);

    private List<AssetBundle> assetBundles = new List<AssetBundle>();
    private long _version;

    //
    private static string []ASSET_NAMES = new []{
		"Commons", "Units", "Maps"
	};
	
	//
	private static AssetBundleManager _singleton;
	public static AssetBundleManager Get() {
		if(!_singleton) {
			var obj = new GameObject("[AssetBundleManager]");
			DontDestroyOnLoad(obj);
			_singleton = obj.AddComponent<AssetBundleManager>();
		}
		return _singleton;
	}
	
    public UnityEngine.Object GetAsset(string name, Type type) {
        foreach (var ab in assetBundles) {
            var obj = ab.LoadAsset(name, type);
            if (obj)
                return obj;
        }

        return null;
    }

    //
 //   public void FetchStatus(StatusCallback onFinish) {
	//	StopAllCoroutines ();
	//	StartCoroutine (_FetchStatus (onFinish));
	//}
	
//	private IEnumerator _FetchStatus(StatusCallback onFinish) {
//		string prefix = "192.167.0.45/";

//		using (var www = new WWW(prefix + "status.json?t=" + UnityEngine.Random.value)) {
//			yield return www;
			
//			try {
//				var json = JSON.Parse (www.text) as JSONClass;
//				_version = json["version"].AsLong;

//				if(json.ContainsKey("inspection")) {
//					onFinish(false, json["inspection"].AsString.L ());
//					yield break;
//				}
//			} catch(Exception e) {
//				Debug.LogWarning(e);
//				onFinish(false, "Update failed. Please check your internet connection.");
//				yield break;
//			}
//		}
		
//		if (onFinish != null)
//			onFinish (true, null);
//	}

//	public void LoadAll(Callback _onFinish) {	

//		Callback onFinish = delegate(bool value) {
//            try {
//				ResourceManager.Get().Initialize();
//			} catch(Exception e) {
//				Debug.LogWarning(e);
				
//				if(_onFinish != null) {
//					_onFinish(false);
//				}
//				return;
//			}

//			if(_onFinish != null) {
//				_onFinish(value);
//			}
//			return;
//		};

//		StartCoroutine(
//			Load(ASSET_NAMES, delegate(bool success) {
//				if (onFinish != null)
//					onFinish (success);
//			}));
//	}
	
//	IEnumerator Load(string []names, Callback onFinish) {

//		yield return null;

//		//
//		foreach (var ab in assetBundles)
//			ab.Unload(true);
//		assetBundles.Clear ();

//		//
//		bool success = true;
//		foreach (var name in names) {
//			using (var www = new WWW("file://" + Path.Combine(persistentDataPath, name + ".unity3d"))) {

//				yield return www;

//				try {
//					if (www.assetBundle != null) {
//						assetBundles.Add(www.assetBundle);
//						Debug.Log(string.Format("Loaded {0}.unity3d", name));
//					}
//					else {
//						success = false;

//						Clear();
//						Debug.LogWarning(string.Format("Failed to load {0}.unity3d", name));
//						break;
//					}
//				}
//				catch (Exception e) {
//					Debug.LogException(e);
//					success = false;
//					Clear();
//					Debug.LogWarning(string.Format("Failed to load {0}.unity3d", name));
//					break;
//				}
//			}
//		}
		
////		if(success)
////			loadedAll = true;
//		onFinish(success);
//	}
	
//	public void DownloadAll(ProgressCallback onProgress, CallbackWithMsg onFinish) {
//		//
//		if (Config.ignoreAssetBundle) {
//			if (onFinish != null)
//				onFinish (true, "");
			
//			return;
//		}

//		StartCoroutine(
//			Download(ASSET_NAMES, onProgress, onFinish)
//			);		
//	}

//	public void DownloadMeta( Action<long> onFinish) {
//		//
//		if (Config.ignoreAssetBundle) {
//			if (onFinish != null)
//				onFinish (0);

//			return;
//		}

//		StartCoroutine(DownloadMeta(ASSET_NAMES, onFinish));		
//	}

//	IEnumerator DownloadMeta(string []names, Action<long> onFinish) {
		
//		string msg = "";
//		string postfix = "";
//		long fileSize = 0;

//		string prefix = Constants.UPDATE_URL_PREFIX;
//		string folder = "android/";

//		if (Constants.DEVELOPMENT_MODE) {
//			prefix = Constants.UPDATE_URL_PREFIX_DEV;
//			#if UNITY_IPHONE
//			folder = "ios_beta/";
//			#elif UNITY_ANDROID
//			folder = "android_beta/";
//			#endif
//		}
//		else {
//			#if UNITY_IPHONE
//			folder = "ios/";
//			#elif UNITY_ANDROID
//			folder = "android/";
//			#endif
//		}

//		int i = 0;
//		foreach(var name in names) {
//			var www = new WWW(prefix + folder + name + postfix + ".md5?t=" + _version);
//			yield return www;

//			if(www.error != null) {
//				Debug.LogWarning(www.error);
//				fileSize = -1;
//				msg = www.error;
//				www.Dispose();
//				break;
//			}

//			string md5 = www.text;
//			www.Dispose();

//			string configKey = string.Format("AssetHash_{0}", name);
//			if(PlayerPrefs.GetString(configKey, null) == md5)
//				continue;

//			www = new WWW(prefix + folder + name + postfix + ".info?t=" + _version);
//			yield return www;

//			if(www.error != null) {
//				Debug.LogWarning(www.error);
//				fileSize = -1;
//				msg = www.error;
//				www.Dispose();
//				break;
//			}

//			long size = long.Parse(www.text);
//			www.Dispose();
			
//			fileSize += size;
//		} 

//		onFinish(fileSize);
//	}

//	IEnumerator Download(string []names, ProgressCallback onProgress, CallbackWithMsg onFinish) {

//		bool success = true;
//		string msg = "";
//		string postfix = "";

//		string prefix = Constants.UPDATE_URL_PREFIX;
//		string folder = "android/";

//		if (Constants.DEVELOPMENT_MODE) {
//			prefix = Constants.UPDATE_URL_PREFIX_DEV;
//#if UNITY_IPHONE
//			folder = "ios_beta/";
//#elif UNITY_ANDROID
//			folder = "android_beta/";
//#endif
//		}
//		else {
//#if UNITY_IPHONE
//			folder = "ios/";
//#elif UNITY_ANDROID
//			folder = "android/";
//#endif
//		}

//		int i = 0;
//		foreach(var name in names) {
//			var www = new WWW(prefix + folder + name + postfix + ".md5?t=" + _version);
//			yield return www;
			
//			if(www.error != null) {
//				Debug.LogWarning(www.error);
//				success = false;
//				msg = www.error;
//				www.Dispose();
//				break;
//			}
			
//			string md5 = www.text;
//			Debug.Log (string.Format("Patch MD5 {0}.unity3d = {1}", name, md5));
//			www.Dispose();
			
//			//
//			string configKey = string.Format("AssetHash_{0}", name);
//			if(PlayerPrefs.GetString(configKey, null) == md5) {
//				Debug.Log(string.Format("Skipping {0}.unity3d = {1}", name, PlayerPrefs.GetString(configKey, null)));
				
//				i++;
//				if(onProgress != null)
//					onProgress(i/(float)names.Length);
//				continue;
//			}
			
//			www = new WWW(prefix + folder + name + postfix + ".unity3d?t=" + _version);
//			while(!www.isDone) {
//				if(www.error != null)  
//					break;
//				yield return null;
//				if(onProgress != null)
//					onProgress((i + www.progress)/(float)names.Length);
//			}
			
//			if(www.error != null) {
//				success = false;
//				msg = www.error;
//				www.Dispose();
//				break;
//			}
			
//			byte []bytes = www.bytes;
//			www.Dispose();
			
//			if(bytes != null) {
				
//				if(md5.CompareTo(Utility.MD5(bytes)) != 0) {
//					success = false;
//					msg = string.Format("Failed to download {0}.unity3d", name);
//					Debug.LogWarning(msg);
//					break;
//				}
				
//				try {
//					var path = Path.Combine (persistentDataPath, name + ".unity3d");
//					File.WriteAllBytes(path, bytes);
					
//					#if UNITY_IPHONE
//					UnityEngine.iOS.Device.SetNoBackupFlag(path);
//					#endif
//				} catch(Exception e) {
//					Debug.LogException(e);
//					msg = string.Format("Failed to save {0}.unity3d", name);
//					Debug.LogWarning(msg);
//					success = false;
//					break;
//				}
				
//				i++;
//				onProgress(i/(float)names.Length);
				
//				//
//				PlayerPrefs.SetString(configKey, md5);
				
//				//
//				Debug.Log (string.Format("Downloaded {0}.unity3d", name));
//			} else {
//				msg = string.Format("Failed to download {0}.unity3d", name);
//				Debug.LogWarning(msg);
//				success = false;
//				break;
//			}
//		} 
		
//		PlayerPrefs.Save();
//		onFinish(success, msg);
//	}
}

