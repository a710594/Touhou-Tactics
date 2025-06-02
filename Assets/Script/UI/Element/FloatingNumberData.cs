using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class FloatingNumberData
    {
        public string Text;
        public Color Color;

        public FloatingNumberData(string text, EffectModel.TypeEnum effectType, HitType hitType)
        {
            if (hitType == HitType.Miss)
            {
                Text = "Miss";
                Color = Color.black;
            }
            else
            {
                Text = text;

                if (effectType == EffectModel.TypeEnum.MagicAttack || effectType == EffectModel.TypeEnum.PhysicalAttack || effectType == EffectModel.TypeEnum.RatioDamage)
                {
                    if (hitType == HitType.Critical)
                    {
                        Color = Color.yellow;
                    }
                    else if (hitType == HitType.Hit)
                    {
                        Color = Color.red;
                    }
                    else
                    {
                        Color = Color.blue;
                    }
                }
                else if (effectType == EffectModel.TypeEnum.Poison)
                {
                    Color = new Color32(180, 0, 180, 255);
                }
                else if (effectType == EffectModel.TypeEnum.Recover || effectType == EffectModel.TypeEnum.Medicine || effectType == EffectModel.TypeEnum.Purify || effectType == EffectModel.TypeEnum.RecoverAll)
                {
                    Color = Color.green;
                }
                else if (effectType == EffectModel.TypeEnum.Sleep)
                {
                    Color = new Color32(0, 150, 200, 255);
                }
                else
                {
                    Color = Color.black;
                }
            }
        }
    }
}
