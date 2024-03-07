using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager
{
    private readonly string _fileName = "FlagInfo";

    private static FlagManager _instance;
    public static FlagManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new FlagManager();
            }
            return _instance;
        }
    }

    public FlagInfo Info;

    public void Init()
    {
        Load();
    }

    public void Load() 
    {
        FlagInfo info = DataContext.Instance.Load<FlagInfo>(_fileName, DataContext.PrePathEnum.Save);
        if (info != null)
        {
            Info = info;
        }
        else
        {
            Info = new FlagInfo();
            Info.Init();
        }
    }

    public void Save()
    {
        DataContext.Instance.Save(Info, _fileName, DataContext.PrePathEnum.Save);
    }

    public void Delete()
    {
        DataContext.Instance.DeleteData(_fileName, DataContext.PrePathEnum.Save);
    }
}
