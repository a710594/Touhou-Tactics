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
        else if (skill.Track == TrackEnum.Near)
        {
            TrackLabel.text = "軌跡：近戰";
        }

        RangeLabel.text = "射程：" + skill.Range;
    }

    public void SetData(Sub sub)
    {
        NameLabel.text = sub.Name;
        string comment = sub.Comment;
        int index;
        Effect effect = sub.Effect;
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
            effect = sub.Effect.SubEffect;
        }
        CommentLabel.text = comment;
        CDLabel.text = "冷卻：" + sub.CD + "回合";

        if (sub.Track == TrackEnum.None)
        {
            TrackLabel.text = "軌跡：無視地形";
        }
        else if (sub.Track == TrackEnum.Parabola)
        {
            TrackLabel.text = "軌跡：拋物線";
        }
        else if (sub.Track == TrackEnum.Straight)
        {
            TrackLabel.text = "軌跡：直線";
        }
        else if (sub.Track == TrackEnum.Through)
        {
            TrackLabel.text = "軌跡：貫穿";
        }
        else if (sub.Track == TrackEnum.Near)
        {
            TrackLabel.text = "軌跡：近戰";
        }

        RangeLabel.text = "射程：" + sub.Range;
    }

    public void SetData(Spell spell)
    {
        NameLabel.text = spell.Name;
        CommentLabel.text = spell.Comment;

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
        else if (spell.Track == TrackEnum.Near)
        {
            TrackLabel.text = "軌跡：近戰";
        }

        RangeLabel.text = "射程：" + spell.Range;
    }

    public void SetData(ItemCommand item)
    {
        NameLabel.text = item.Name;
        CommentLabel.text = item.Comment;
        CDLabel.text = "";

        if (item.Track == TrackEnum.None)
        {
            TrackLabel.text = "軌跡：無視地形";
        }
        else if (item.Track == TrackEnum.Parabola)
        {
            TrackLabel.text = "軌跡：拋物線";
        }
        else if (item.Track == TrackEnum.Straight)
        {
            TrackLabel.text = "軌跡：直線";
        }
        else if (item.Track == TrackEnum.Through)
        {
            TrackLabel.text = "軌跡：貫穿";
        }
        else if (item.Track == TrackEnum.Near)
        {
            TrackLabel.text = "軌跡：近戰";
        }

        RangeLabel.text = "射程：" + item.Range;
    }
}
