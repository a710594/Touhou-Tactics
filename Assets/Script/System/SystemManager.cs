using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager 
{
    private readonly string _fileName = "SystemInfo";

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

    public SystemInfo SystemInfo;

    public void Init() 
    {
        SystemInfo info = DataContext.Instance.Load<SystemInfo>(_fileName, DataContext.PrePathEnum.Save);
        if (info == null) 
        {
            info = new SystemInfo();
        }
        SystemInfo = info;
    }

    public void Save()
    {
        SystemInfo.CurrentScene = SceneController.Instance.CurrentScene;
        DataContext.Instance.Save(SystemInfo, _fileName, DataContext.PrePathEnum.Save);
    }

    public void Delete()
    {
        DataContext.Instance.DeleteData(_fileName, DataContext.PrePathEnum.Save);
    }
}
