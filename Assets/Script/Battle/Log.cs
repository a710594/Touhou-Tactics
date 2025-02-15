using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Log
    {
        public BattleCharacterController User;
        public BattleCharacterController Target;
        public HitType HitType;
        public Effect Effect;
        public string Text;
        public string FullText;

        public Log(BattleCharacterController user, BattleCharacterController target, Effect effect, HitType hitType, string text) 
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
                targetName = target.Info.Name;
            }

            if (effect.Type == EffectModel.TypeEnum.MagicAttack || effect.Type == EffectModel.TypeEnum.PhysicalAttack || effect.Type == EffectModel.TypeEnum.RatioDamage)
            {
                if (HitType == HitType.Critical)
                {
                    FullText = user.Info.Name + " 對 " + targetName + " 造成了 " + text + " 爆擊傷害";
                }
                else if (HitType == HitType.Hit)
                {
                    FullText = user.Info.Name + " 對 " + targetName + " 造成了 " + text + " 傷害";
                }
                else
                {
                    FullText = user.Info.Name + " 對 " + targetName + " 的攻擊沒有命中";
                }
            }
            else if (effect.Type == EffectModel.TypeEnum.Poison)
            {
                FullText = user.Info.Name + " 使 " + targetName + " 中毒了";
            }
            else if (effect.Type == EffectModel.TypeEnum.Recover || effect.Type == EffectModel.TypeEnum.Medicine || effect.Type == EffectModel.TypeEnum.RecoverAll)
            {
                FullText = user.Info.Name + " 使 " + targetName + " 回復了 " + text + " HP";
            }
            else if(effect.Type == EffectModel.TypeEnum.Purify)
            {
                FullText = user.Info.Name + " 使 " + targetName + " 的異常狀態回復了";
            }
            else if (effect.Type == EffectModel.TypeEnum.Sleep)
            {
                FullText = user.Info.Name + " 使 " + targetName + " 睡著了";
            }
            else
            {
                FullText = user.Info.Name + " 使 " + targetName + " " + text;
            }   
            
        }

        public Log(BattleCharacterInfo target, string text) 
        {
            FullText = "中毒 對 " + target.Name + " 造成了 " + text + " 傷害";
        }
    }
}
