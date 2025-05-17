using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{

    public class ItemCommand : Command
    {
        public int ItemID = 0;
        public int Amount = 0;
        public Food Food = null;

        public ItemCommand() { }

        public ItemCommand(int id, int amount)
        {
            ItemModel itemData = DataTable.Instance.ItemDic[id];
            ConsumablesModel consumablesData = DataTable.Instance.ConsumablesDic[id];
            ID = id;
            ItemID = id;
            Name = itemData.Name;
            Comment = itemData.Comment;
            Amount = amount;
            if (consumablesData.EffectID != -1)
            {
                Effect = EffectFactory.GetEffect(consumablesData.EffectID);
            }

            Hit = -1;
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
            Food = food;
            Name = food.Name;
            Comment = food.Comment;
            Effect = EffectFactory.GetEffect(food);

            Hit = -1;
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