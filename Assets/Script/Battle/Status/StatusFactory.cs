using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusFactory
{
    public static Status GetStatus(StatusModel.TypeEnum type, int id)
    {
        return GetStatus(DataContext.Instance.StatusDic[type][id]);
    }

    public static Status GetStatus(StatusModel data)
    {
        Status status = null;

        if (data.Type == StatusModel.TypeEnum.Provocative)
        {
            status = new ProvocativeStatus(data);
        }
        else if (data.Type == StatusModel.TypeEnum.Poison) 
        {
            status = new Poison(data);
        }
        else if (data.Type == StatusModel.TypeEnum.Sleep)
        {
            status = new Sleep(data);
        }
        else 
        {
            status = new Status(data);
        }

        return status;
    }
}