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
        Effect effect = skill.Effect;
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
            effect = skill.Effect.SubEffect;
        }
        CommentLabel.text = comment;
        CDLabel.text = "�N�o�G" + skill.CD + "�^�X";
        
        /*if (skill.Effect.Target == EffectModel.TargetEnum.All)
        {
            TargetLabel.text = "�ؼСG�����ħ�";
        }
        else if(skill.Effect.Target == EffectModel.TargetEnum.Them) 
        {
            TargetLabel.text = "�ؼСG�Ĥ�";
        }
        else if (skill.Effect.Target == EffectModel.TargetEnum.Us)
        {
            TargetLabel.text = "�ؼСG�ڤ�";
        }*/

        if(skill.Track == TrackEnum.None) 
        {
            TrackLabel.text = "�y��G�L���a��";
        }
        else if (skill.Track == TrackEnum.Parabola)
        {
            TrackLabel.text = "�y��G�ߪ��u";
        }
        else if (skill.Track == TrackEnum.Straight)
        {
            TrackLabel.text = "�y��G���u";
        }
        else if (skill.Track == TrackEnum.Through)
        {
            TrackLabel.text = "�y��G�e��";
        }

        RangeLabel.text = "�g�{�G" + skill.Range;
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
        CDLabel.text = "�N�o�G" + support.CD + "�^�X";

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
            TrackLabel.text = "�y��G�L���a��";
        }
        else if (support.Track == TrackEnum.Parabola)
        {
            TrackLabel.text = "�y��G�ߪ��u";
        }
        else if (support.Track == TrackEnum.Straight)
        {
            TrackLabel.text = "�y��G���u";
        }
        else if (support.Track == TrackEnum.Through)
        {
            TrackLabel.text = "�y��G�e��";
        }

        RangeLabel.text = "�g�{�G" + support.Range;
    }

    public void SetData(Spell spell)
    {
        NameLabel.text = spell.Name;
        CommentLabel.text = spell.Comment;
        CDLabel.text = "�N�o�G" + spell.CD + "�^�X";

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
            TrackLabel.text = "�y��G�L���a��";
        }
        else if (spell.Track == TrackEnum.Parabola)
        {
            TrackLabel.text = "�y��G�ߪ��u";
        }
        else if (spell.Track == TrackEnum.Straight)
        {
            TrackLabel.text = "�y��G���u";
        }
        else if (spell.Track == TrackEnum.Through)
        {
            TrackLabel.text = "�y��G�e��";
        }

        RangeLabel.text = "�g�{�G" + spell.Range;
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
            TrackLabel.text = "�y��G�L���a��";
        }
        else if (consumbles.Track == TrackEnum.Parabola)
        {
            TrackLabel.text = "�y��G�ߪ��u";
        }
        else if (consumbles.Track == TrackEnum.Straight)
        {
            TrackLabel.text = "�y��G���u";
        }
        else if (consumbles.Track == TrackEnum.Through)
        {
            TrackLabel.text = "�y��G�e��";
        }

        RangeLabel.text = "�g�{�G" + consumbles.Range;
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
            TrackLabel.text = "�y��G�L���a��";
        }
        else if (food.Track == TrackEnum.Parabola)
        {
            TrackLabel.text = "�y��G�ߪ��u";
        }
        else if (food.Track == TrackEnum.Straight)
        {
            TrackLabel.text = "�y��G���u";
        }
        else if (food.Track == TrackEnum.Through)
        {
            TrackLabel.text = "�y��G�e��";
        }

        RangeLabel.text = "�g�{�G" + food.Range;
    }
}
