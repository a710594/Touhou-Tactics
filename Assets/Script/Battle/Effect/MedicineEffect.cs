using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//©T©w¦^¦å
public class MedicineEffect : Effect
{
    public MedicineEffect(EffectModel data) : base(data)
    {
        //Data = data;
        //if (data.SubID != -1)
        //{
        //    EffectModel subData = DataContext.Instance.EffectDic[data.SubID];
        //    SubEffect = EffectFactory.GetEffect(subData);
        //}
    }

    //for food
    public MedicineEffect(int value)
    {
        Value = value;
        Type = EffectModel.TypeEnum.Medicine;
        Hit = 100;
        Target = EffectModel.TargetEnum.Us;
        Track = EffectModel.TrackEnum.None;
    }

    public override void Use(BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> floatingList, List<BattleCharacterInfo> characterList)
    {
        FloatingNumberData floatingNumberData;
        BattleController.HitType hitType;

        if (Target == EffectModel.TargetEnum.Us)
        {
            hitType = BattleController.HitType.Hit;
        }
        else
        {
            hitType = BattleController.Instance.CheckHit(this, user, target);
        }

        if (hitType != BattleController.HitType.Miss)
        {
            int recover = Value;
            target.SetRecover(recover);
            floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Recover, recover.ToString());
        }
        else
        {
            floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Miss, "Miss");
        }
        floatingList.Add(floatingNumberData);


        if (SubEffect != null && hitType != BattleController.HitType.Miss)
        {
            SubEffect.Use(user, target, floatingList, characterList);
        }
    }

}
