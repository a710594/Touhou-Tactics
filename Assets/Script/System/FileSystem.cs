using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSystem
{
    private static FileSystem _instance;
    public static FileSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new FileSystem();
            }
            return _instance;
        }
    }

    public void Init() 
    {
        SystemManager.Instance.Init();
        DataContext.Instance.Init();
        SceneController.Instance.Init();
        ItemManager.Instance.Init();
        CharacterManager.Instance.Init();
        InputMamager.Instance.Init();
        FlagManager.Instance.Init();
    }

    public void Save() 
    {
        ItemManager.Instance.Save();
        CharacterManager.Instance.Save();
        Explore.ExploreManager.Instance.Save();
        SystemManager.Instance.Save();
        FlagManager.Instance.Save();
        Debug.Log("存檔成功");
    }

    public void Delete() 
    {
        ItemManager.Instance.Delete();
        CharacterManager.Instance.Delete();
        Explore.ExploreManager.Instance.Delete();
        SystemManager.Instance.Delete();
        FlagManager.Instance.Delete();
        Debug.Log("刪除存檔");
    }
}
