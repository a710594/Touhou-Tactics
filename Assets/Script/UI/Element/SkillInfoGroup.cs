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
        NameLabel.text = skill.Data.Name;
        CommentLabel.text = skill.Data.Comment;
        CDLabel.text = "�N�o�G" + skill.Data.CD + "�^�X";
        
        if (skill.Effect.Target == EffectModel.TargetEnum.All)
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
        }

        if(skill.Effect.Track == EffectModel.TrackEnum.None) 
        {
            TrackLabel.text = "�y��G�L���a��";
        }
        else if (skill.Effect.Track == EffectModel.TrackEnum.Parabola)
        {
            TrackLabel.text = "�y��G�ߪ��u";
        }
        else if (skill.Effect.Track == EffectModel.TrackEnum.Straight)
        {
            TrackLabel.text = "�y��G���u";
        }
        else if (skill.Effect.Track == EffectModel.TrackEnum.Through)
        {
            TrackLabel.text = "�y��G�e��";
        }

        RangeLabel.text = "�g�{�G" + skill.Effect.Range;
    }

    public void SetData(Support support)
    {
        NameLabel.text = support.Data.Name;
        CommentLabel.text = support.Data.Comment;
        CDLabel.text = "�N�o�G" + support.Data.CD + "�^�X";

        if (support.Effect.Target == EffectModel.TargetEnum.All)
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
        }

        if (support.Effect.Track == EffectModel.TrackEnum.None)
        {
            TrackLabel.text = "�y��G�L���a��";
        }
        else if (support.Effect.Track == EffectModel.TrackEnum.Parabola)
        {
            TrackLabel.text = "�y��G�ߪ��u";
        }
        else if (support.Effect.Track == EffectModel.TrackEnum.Straight)
        {
            TrackLabel.text = "�y��G���u";
        }
        else if (support.Effect.Track == EffectModel.TrackEnum.Through)
        {
            TrackLabel.text = "�y��G�e��";
        }

        RangeLabel.text = "�g�{�G" + support.Effect.Range;
    }

    public void SetData(Card card)
    {
        NameLabel.text = card.Data.Name;
        CommentLabel.text = card.Data.Comment;
        CDLabel.text = "����PP�G" + card.Data.PP;

        if (card.Effect.Target == EffectModel.TargetEnum.All)
        {
            TargetLabel.text = "�ؼСG�����ħ�";
        }
        else if (card.Effect.Target == EffectModel.TargetEnum.Them)
        {
            TargetLabel.text = "�ؼСG�Ĥ�";
        }
        else if (card.Effect.Target == EffectModel.TargetEnum.Us)
        {
            TargetLabel.text = "�ؼСG�ڤ�";
        }

        if (card.Effect.Track == EffectModel.TrackEnum.None)
        {
            TrackLabel.text = "�y��G�L���a��";
        }
        else if (card.Effect.Track == EffectModel.TrackEnum.Parabola)
        {
            TrackLabel.text = "�y��G�ߪ��u";
        }
        else if (card.Effect.Track == EffectModel.TrackEnum.Straight)
        {
            TrackLabel.text = "�y��G���u";
        }
        else if (card.Effect.Track == EffectModel.TrackEnum.Through)
        {
            TrackLabel.text = "�y��G�e��";
        }

        RangeLabel.text = "�g�{�G" + card.Effect.Range;
    }

    public void SetData(Consumables consumbles)
    {
        NameLabel.text = consumbles.ItemData.Name;
        CommentLabel.text = consumbles.ItemData.Comment;
        CDLabel.text = "";

        if (consumbles.Effect.Target == EffectModel.TargetEnum.All)
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
        }

        if (consumbles.Effect.Track == EffectModel.TrackEnum.None)
        {
            TrackLabel.text = "�y��G�L���a��";
        }
        else if (consumbles.Effect.Track == EffectModel.TrackEnum.Parabola)
        {
            TrackLabel.text = "�y��G�ߪ��u";
        }
        else if (consumbles.Effect.Track == EffectModel.TrackEnum.Straight)
        {
            TrackLabel.text = "�y��G���u";
        }
        else if (consumbles.Effect.Track == EffectModel.TrackEnum.Through)
        {
            TrackLabel.text = "�y��G�e��";
        }

        RangeLabel.text = "�g�{�G" + consumbles.Effect.Range;
    }

    public void SetData(Food food)
    {
        NameLabel.text = food.Name;
        CommentLabel.text = food.Comment;
        CDLabel.text = "";

        if (food.Effect.Target == EffectModel.TargetEnum.All)
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
        }

        if (food.Effect.Track == EffectModel.TrackEnum.None)
        {
            TrackLabel.text = "�y��G�L���a��";
        }
        else if (food.Effect.Track == EffectModel.TrackEnum.Parabola)
        {
            TrackLabel.text = "�y��G�ߪ��u";
        }
        else if (food.Effect.Track == EffectModel.TrackEnum.Straight)
        {
            TrackLabel.text = "�y��G���u";
        }
        else if (food.Effect.Track == EffectModel.TrackEnum.Through)
        {
            TrackLabel.text = "�y��G�e��";
        }

        RangeLabel.text = "�g�{�G" + food.Effect.Range;
    }
}
