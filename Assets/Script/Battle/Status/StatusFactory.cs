using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class StatusFactory
    {
        public static Status GetStatus(int id)
        {
            return GetStatus(DataContext.Instance.StatusDic[id]);
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
}