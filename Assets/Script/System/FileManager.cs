using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class FileManager : MonoBehaviour
{
    private static readonly string _dataPath = "/Data/";
    private static readonly string _savePath = "/Save/";
    private static readonly string _mapExplorePath = "/Map/Explore/";
    private static readonly string _mapBattleRandomPath = "/Map/Battle/Random/";
    private static readonly string _mapBattleFixedPath = "/Map/Battle/Fixed/";

    public enum PathEnum
    {
        None = -1,
        Data,
        Save,
        MapExplore,
        MapBattleFixed,
        MapBattleRandom,
    }

    public void Init()
    {
        if (!Directory.Exists(Application.streamingAssetsPath + "/Save/"))
        {
            DirectoryInfo b = Directory.CreateDirectory(Application.streamingAssetsPath + "/Save/");
        }
    }

    public void Load<T>(string fileName, PathEnum prePathEnum, Action<object> callback)
    {
        try
        {
            string path = GetPath(fileName, prePathEnum);

#if UNITY_WEBGL
            StartCoroutine(LoadWeb<T>(path, callback));
#else
            StartCoroutine(LoadLocal<T>(path, callback));
#endif
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }
    }

    public void Save<T>(T t, string fileName, PathEnum prePathEnum)
    {
        try
        {
            string path = GetPath(fileName, prePathEnum);
            File.WriteAllText(path, JsonConvert.SerializeObject(t));
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    public void Delete(string fileName, PathEnum prePathEnum)
    {
        File.Delete(GetPath(fileName, prePathEnum));
    }

    public bool IsSaveEmpty()
    {
        return !Directory.EnumerateFileSystemEntries(Application.streamingAssetsPath + _savePath).Any();
    }

    private IEnumerator LoadLocal<T>(string path, Action<object> callback)
    {
        yield return new WaitUntil(()=>
        {
            try
            {
                string jsonString = File.ReadAllText(path);
                T info = JsonConvert.DeserializeObject<T>(jsonString);
                callback(info);
                return true;
            }
            catch (Exception ex) 
            {
                Debug.LogWarning(ex);
                callback(null);
                return true;
            }
        });
    }

    private IEnumerator LoadWeb<T>(string path, Action<object> callback)
    {
        // WebGL 需要用 UnityWebRequest
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonString = request.downloadHandler.text;
            try
            {
                T info = JsonConvert.DeserializeObject<T>(jsonString);
                callback(info);
            }
            catch(Exception ex) 
            {
                Debug.LogError(ex);
                callback(null);
            }
        }
        else
        {
            Debug.LogError("無法讀取 JSON：" + request.error);
            callback(null);
        }
    }

    private string GetPath(string fileName, PathEnum prePathEnum)
    {
        string path;
        string prePath = Application.streamingAssetsPath;
        if (prePathEnum == PathEnum.Data)
        {
            prePath += _dataPath;
        }
        else if (prePathEnum == PathEnum.Save)
        {
            prePath += _savePath;
        }
        else if (prePathEnum == PathEnum.MapExplore)
        {
            prePath += _mapExplorePath;
        }
        else if (prePathEnum == PathEnum.MapBattleFixed)
        {
            prePath += _mapBattleFixedPath;
        }
        else if (prePathEnum == PathEnum.MapBattleRandom)
        {
            prePath += _mapBattleRandomPath;
        }

        path = Path.Combine(prePath, fileName + ".json");

        return path;
    }
}
