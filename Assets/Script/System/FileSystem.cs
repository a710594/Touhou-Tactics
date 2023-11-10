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

    public void Save() 
    {
        ItemManager.Instance.Save();
        CharacterManager.Instance.Save();
        Explore.ExploreManager.Instance.Save();
        SystemManager.Instance.Save();
        Debug.Log("¶s¿…¶®•\");
    }
}
