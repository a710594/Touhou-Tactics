using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battle;

public class SkillInfoGroup : MonoBehaviour
{
    public Text NameLabel;
    public Text CommentLabel;
    public Text CDLabel;
    public Text TargetLabel;
    public Text TrackLabel;
    public Text RangeLabel;

    public void SetData(Skill skill) 
    {
        NameLabel.text = skill.Name;
        string comment = skill.Comment;
        int index;
        Skill tempSkill = skill;
        Effect tempEffect = skill.Effect;
        while (comment.Contains("{")) 
        {
            index = comment.IndexOf("{");
            if (tempEffect.Status != null)
            {
                if (tempEffect.Status.Value > 100)
                {
                    comment = comment.Remove(index, 3).Insert(index, (tempEffect.Status.Value - 100).ToString());
                }
                else
                {
                    comment = comment.Remove(index, 3).Insert(index, (100 - tempEffect.Status.Value).ToString());
                }
            }
            else
            {
                comment = comment.Remove(index, 3).Insert(index, tempEffect.Value.ToString());
            }
            if(tempSkill.Effect.SubEffect!=null)
            {
                tempEffect = tempSkill.Effect.SubEffect;
            }
            else if(tempSkill.SubCommand!=null)
            {
                tempSkill = (Skill)tempSkill.SubCommand;
                tempEffect = tempSkill.Effect;
            }
        }
        CommentLabel.text = comment;
        CDLabel.text = "冷卻：" + skill.CD + "回合";
        
        if(skill.Track == TrackEnum.None) 
        {
            TrackLabel.text = "軌跡：無視地形";
        }
        else if (skill.Track == TrackEnum.Parabola)
        {
            TrackLabel.text = "軌跡：拋物線";
        }
        else if (skill.Track == TrackEnum.Straight)
        {
            TrackLabel.text = "軌跡：直線";
        }
        else if (skill.Track == TrackEnum.Through)
        {
            TrackLabel.text = "軌跡：貫穿";
        }

        RangeLabel.text = "射程：" + skill.Range;
    }

    public void SetData(Support support)
    {
        NameLabel.text = support.Name;
        string comment = support.Comment;
        int index;
        Effect effect = support.Effect;
        while (comment.Contains("{"))
        {
            index = comment.IndexOf("{");
            if (effect.Status != null)
            {
                if (effect.Status.Value > 100)
                {
                    comment = comment.Remove(index, 3).Insert(index, (effect.Status.Value - 100).ToString());
                }
                else
                {
                    comment = comment.Remove(index, 3).Insert(index, (100 - effect.Status.Value).ToString());
                }
            }
            else
            {
                comment = comment.Remove(index, 3).Insert(index, effect.Value.ToString());
            }
            effect = support.Effect.SubEffect;
        }
        CommentLabel.text = comment;
        CDLabel.text = "冷卻：" + support.CD + "回合";

        /*if (support.Effect.Target == EffectModel.TargetEnum.All)
        {
            TargetLabel.text = "�ؼСG�����ħ�";
        }
        else if (support.Effect.Target == EffectModel.TargetEnum.Them)
        {
            TargetLabel.text = "�ؼСG�Ĥ�";
        }
        else if (support.Effect.Target == EffectModel.TargetEnum.Us)
        {
            TargetLabel.text = "�ؼСG�ڤ�";
        }*/

        if (support.Track == TrackEnum.None)
        {
            TrackLabel.text = "軌跡：無視地形";
        }
        else if (support.Track == TrackEnum.Parabola)
        {
            TrackLabel.text = "軌跡：拋物線";
        }
        else if (support.Track == TrackEnum.Straight)
        {
            TrackLabel.text = "軌跡：直線";
        }
        else if (support.Track == TrackEnum.Through)
        {
            TrackLabel.text = "軌跡：貫穿";
        }

        RangeLabel.text = "射程：" + support.Range;
    }

    public void SetData(Spell spell)
    {
        NameLabel.text = spell.Name;
        CommentLabel.text = spell.Comment;
        CDLabel.text = "冷卻：" + spell.CD + "回合";

        /*if (spell.Effect.Target == EffectModel.TargetEnum.All)
        {
            TargetLabel.text = "�ؼСG�����ħ�";
        }
        else if (spell.Effect.Target == EffectModel.TargetEnum.Them)
        {
            TargetLabel.text = "�ؼСG�Ĥ�";
        }
        else if (spell.Effect.Target == EffectModel.TargetEnum.Us)
        {
            TargetLabel.text = "�ؼСG�ڤ�";
        }*/

        if (spell.Track == TrackEnum.None)
        {
            TrackLabel.text = "軌跡：無視地形";
        }
        else if (spell.Track == TrackEnum.Parabola)
        {
            TrackLabel.text = "軌跡：拋物線";
        }
        else if (spell.Track == TrackEnum.Straight)
        {
            TrackLabel.text = "軌跡：直線";
        }
        else if (spell.Track == TrackEnum.Through)
        {
            TrackLabel.text = "軌跡：貫穿";
        }

        RangeLabel.text = "射程：" + spell.Range;
    }

    public void SetData(Consumables consumbles)
    {
        NameLabel.text = consumbles.Name;
        CommentLabel.text = consumbles.Comment;
        CDLabel.text = "";

        /*if (consumbles.Effect.Target == EffectModel.TargetEnum.All)
        {
            TargetLabel.text = "�ؼСG�����ħ�";
        }
        else if (consumbles.Effect.Target == EffectModel.TargetEnum.Them)
        {
            TargetLabel.text = "�ؼСG�Ĥ�";
        }
        else if (consumbles.Effect.Target == EffectModel.TargetEnum.Us)
        {
            TargetLabel.text = "�ؼСG�ڤ�";
        }*/

        if (consumbles.Track == TrackEnum.None)
        {
            TrackLabel.text = "軌跡：無視地形";
        }
        else if (consumbles.Track == TrackEnum.Parabola)
        {
            TrackLabel.text = "軌跡：拋物線";
        }
        else if (consumbles.Track == TrackEnum.Straight)
        {
            TrackLabel.text = "軌跡：直線";
        }
        else if (consumbles.Track == TrackEnum.Through)
        {
            TrackLabel.text = "軌跡：貫穿";
        }

        RangeLabel.text = "射程：" + consumbles.Range;
    }

    public void SetData(Food food)
    {
        NameLabel.text = food.Name;
        CommentLabel.text = food.Comment;
        CDLabel.text = "";

        /*if (food.Effect.Target == EffectModel.TargetEnum.All)
        {
            TargetLabel.text = "�ؼСG�����ħ�";
        }
        else if (food.Effect.Target == EffectModel.TargetEnum.Them)
        {
            TargetLabel.text = "�ؼСG�Ĥ�";
        }
        else if (food.Effect.Target == EffectModel.TargetEnum.Us)
        {
            TargetLabel.text = "�ؼСG�ڤ�";
        }*/

        if (food.Track == TrackEnum.None)
        {
            TrackLabel.text = "軌跡：無視地形";
        }
        else if (food.Track == TrackEnum.Parabola)
        {
            TrackLabel.text = "軌跡：拋物線";
        }
        else if (food.Track == TrackEnum.Straight)
        {
            TrackLabel.text =  "軌跡：直線";
        }
        else if (food.Track == TrackEnum.Through)
        {
            TrackLabel.text = "軌跡：貫穿";
        }

        RangeLabel.text = "射程：" + food.Range;
    }
}
