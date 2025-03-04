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
        EventManager.Instance.Load();
    }

    public void Save() 
    {
        ItemManager.Instance.Save();
        CharacterManager.Instance.Save();
        Explore.ExploreManager.Instance.Save();
        SystemManager.Instance.Save();
        FlagManager.Instance.Save();
        EventManager.Instance.Save();
    }

    public void Delete() 
    {
        ItemManager.Instance.Delete();
        CharacterManager.Instance.Delete();
        Explore.ExploreManager.Instance.Delete();
        SystemManager.Instance.Delete();
        FlagManager.Instance.Delete();
        EventManager.Instance.Delete();
    }
}
