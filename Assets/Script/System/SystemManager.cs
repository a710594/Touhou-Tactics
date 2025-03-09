/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager 
{
    private readonly string _fileName = "SceneInfo";

    private static SystemManager _instance;
    public static SystemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SystemManager();
            }
            return _instance;
        }
    }

    public void Init() 
    {
        SceneInfo info = DataContext.Instance.Load<SceneInfo>(_fileName, DataContext.PrePathEnum.Save);
        if (info == null) 
        {
            info = new SceneInfo();
        }
        Info = info;
    }

    public void Save()
    {
        Info.CurrentScene = SceneController.Instance.CurrentScene;
        DataContext.Instance.Save(Info, _fileName, DataContext.PrePathEnum.Save);
    }

    public void Delete()
    {
        DataContext.Instance.DeleteData(_fileName, DataContext.PrePathEnum.Save);
    }
}*/
