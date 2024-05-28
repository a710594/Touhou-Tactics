using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Log
    {
        public BattleCharacterInfo User;
        public BattleCharacterInfo Target;
        public BattleController.HitType HitType;
        public Effect Effect;
        public string Text;
        public string FullText;

        public Log(BattleCharacterInfo user, BattleCharacterInfo target, Effect effect, BattleController.HitType hitType, string text) 
        {
            User = user;
            Target = target;
            Effect = effect;
            HitType = hitType;
            Text = text;
        
            string targetName;
            if(user == target)
            {
                targetName = "自己";
            }
            else
            {
                targetName = target.Name;
            }

            if (effect.Type == EffectModel.TypeEnum.MagicAttack || effect.Type == EffectModel.TypeEnum.PhysicalAttack)
            {
                if (HitType == BattleController.HitType.Critical)
                {
                    FullText = user.Name + " 對 " + targetName + " 造成了 " + text + " 爆擊傷害";
                }
                else if (HitType == BattleController.HitType.Hit)
                {
                    FullText = user.Name + " 對 " + targetName + " 造成了 " + text + " 傷害";
                }
                else
                {
                    FullText = user.Name + " 對 " + targetName + " 的攻擊沒有命中";
                }
            }
            else if (effect.Type == EffectModel.TypeEnum.Poison)
            {
                FullText = user.Name + " 使 " + targetName + " 中毒了";
            }
            else if (effect.Type == EffectModel.TypeEnum.Recover || effect.Type == EffectModel.TypeEnum.Medicine || effect.Type == EffectModel.TypeEnum.Purify)
            {
                FullText = user.Name + " 使 " + targetName + " 回復了 " + text + " HP";
            }
            else if (effect.Type == EffectModel.TypeEnum.Sleep)
            {
                FullText = user.Name + " 使 " + targetName + " 睡著了";
            }
            else
            {
                FullText = user.Name + " 使 " + targetName + " " + text;
            }   
            
        }
    }
}
