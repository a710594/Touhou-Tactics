using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{

    public class ItemCommand : Command
    {
        public ItemCommand() { }

        public ItemCommand(int id, int amount)
        {
            ItemModel itemData = DataTable.Instance.ItemDic[id];
            ConsumablesModel consumablesData = DataTable.Instance.ConsumablesDic[id];
            ID = id;
            Name = itemData.Name;
            Comment = itemData.Comment;
            if (consumablesData.EffectID != -1)
            {
                Effect = EffectFactory.GetEffect(consumablesData.EffectID);
            }

            Hit = 100;
            Range = 1;
            Target = TargetEnum.Us;
            AreaType = AreaTypeEnum.Point;
            Track = TrackEnum.None;
            ArrayList = Utility.GetAreaList("");
            Particle = consumablesData.Particle;
            Shake = consumablesData.Shake;
        }

        public ItemCommand(Food food)
        {
            Name = food.Name;
            Comment = food.Comment;
            Effect = EffectFactory.GetEffect(food);

            Hit = 100;
            Range = 1;
            Target = TargetEnum.Us;
            AreaType = AreaTypeEnum.Point;
            Track = TrackEnum.None;
            ArrayList = Utility.GetAreaList("");
            Particle = "Cure";
            Shake = false;
        }
    }
}