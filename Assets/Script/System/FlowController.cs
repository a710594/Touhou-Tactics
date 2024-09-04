using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowController
{
    private readonly string _fileName = "FlowInfo";

    private static FlowController _instance;
    public static FlowController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new FlowController();
            }
            return _instance;
        }
    }

    public FlowInfo Info;

    public void Load() 
    {
        FlowInfo info = DataContext.Instance.Load<FlowInfo>(_fileName, DataContext.PrePathEnum.Save);
        if (info != null)
        {
            Info = info;
        }
        else
        {
            Info = new FlowInfo();
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
