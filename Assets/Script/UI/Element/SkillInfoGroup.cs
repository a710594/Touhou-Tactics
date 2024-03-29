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
        CDLabel.text = "冷卻：" + skill.Data.CD + "回合";
        
        if (skill.Effect.Target == EffectModel.TargetEnum.All)
        {
            TargetLabel.text = "目標：不分敵我";
        }
        else if(skill.Effect.Target == EffectModel.TargetEnum.Them) 
        {
            TargetLabel.text = "目標：敵方";
        }
        else if (skill.Effect.Target == EffectModel.TargetEnum.Us)
        {
            TargetLabel.text = "目標：我方";
        }

        if(skill.Effect.Track == EffectModel.TrackEnum.None) 
        {
            TrackLabel.text = "軌跡：無視地形";
        }
        else if (skill.Effect.Track == EffectModel.TrackEnum.Parabola)
        {
            TrackLabel.text = "軌跡：拋物線";
        }
        else if (skill.Effect.Track == EffectModel.TrackEnum.Straight)
        {
            TrackLabel.text = "軌跡：直線";
        }
        else if (skill.Effect.Track == EffectModel.TrackEnum.Through)
        {
            TrackLabel.text = "軌跡：貫穿";
        }

        RangeLabel.text = "射程：" + skill.Effect.Range;
    }

    public void SetData(Support support)
    {
        NameLabel.text = support.Data.Name;
        CommentLabel.text = support.Data.Comment;
        CDLabel.text = "冷卻：" + support.Data.CD + "回合";

        if (support.Effect.Target == EffectModel.TargetEnum.All)
        {
            TargetLabel.text = "目標：不分敵我";
        }
        else if (support.Effect.Target == EffectModel.TargetEnum.Them)
        {
            TargetLabel.text = "目標：敵方";
        }
        else if (support.Effect.Target == EffectModel.TargetEnum.Us)
        {
            TargetLabel.text = "目標：我方";
        }

        if (support.Effect.Track == EffectModel.TrackEnum.None)
        {
            TrackLabel.text = "軌跡：無視地形";
        }
        else if (support.Effect.Track == EffectModel.TrackEnum.Parabola)
        {
            TrackLabel.text = "軌跡：拋物線";
        }
        else if (support.Effect.Track == EffectModel.TrackEnum.Straight)
        {
            TrackLabel.text = "軌跡：直線";
        }
        else if (support.Effect.Track == EffectModel.TrackEnum.Through)
        {
            TrackLabel.text = "軌跡：貫穿";
        }

        RangeLabel.text = "射程：" + support.Effect.Range;
    }

    public void SetData(Card card)
    {
        NameLabel.text = card.Data.Name;
        CommentLabel.text = card.Data.Comment;
        CDLabel.text = "消耗PP：" + card.Data.PP;

        if (card.Effect.Target == EffectModel.TargetEnum.All)
        {
            TargetLabel.text = "目標：不分敵我";
        }
        else if (card.Effect.Target == EffectModel.TargetEnum.Them)
        {
            TargetLabel.text = "目標：敵方";
        }
        else if (card.Effect.Target == EffectModel.TargetEnum.Us)
        {
            TargetLabel.text = "目標：我方";
        }

        if (card.Effect.Track == EffectModel.TrackEnum.None)
        {
            TrackLabel.text = "軌跡：無視地形";
        }
        else if (card.Effect.Track == EffectModel.TrackEnum.Parabola)
        {
            TrackLabel.text = "軌跡：拋物線";
        }
        else if (card.Effect.Track == EffectModel.TrackEnum.Straight)
        {
            TrackLabel.text = "軌跡：直線";
        }
        else if (card.Effect.Track == EffectModel.TrackEnum.Through)
        {
            TrackLabel.text = "軌跡：貫穿";
        }

        RangeLabel.text = "射程：" + card.Effect.Range;
    }

    public void SetData(Consumables consumbles)
    {
        NameLabel.text = consumbles.ItemData.Name;
        CommentLabel.text = consumbles.ItemData.Comment;
        CDLabel.text = "";

        if (consumbles.Effect.Target == EffectModel.TargetEnum.All)
        {
            TargetLabel.text = "目標：不分敵我";
        }
        else if (consumbles.Effect.Target == EffectModel.TargetEnum.Them)
        {
            TargetLabel.text = "目標：敵方";
        }
        else if (consumbles.Effect.Target == EffectModel.TargetEnum.Us)
        {
            TargetLabel.text = "目標：我方";
        }

        if (consumbles.Effect.Track == EffectModel.TrackEnum.None)
        {
            TrackLabel.text = "軌跡：無視地形";
        }
        else if (consumbles.Effect.Track == EffectModel.TrackEnum.Parabola)
        {
            TrackLabel.text = "軌跡：拋物線";
        }
        else if (consumbles.Effect.Track == EffectModel.TrackEnum.Straight)
        {
            TrackLabel.text = "軌跡：直線";
        }
        else if (consumbles.Effect.Track == EffectModel.TrackEnum.Through)
        {
            TrackLabel.text = "軌跡：貫穿";
        }

        RangeLabel.text = "射程：" + consumbles.Effect.Range;
    }

    public void SetData(Food food)
    {
        NameLabel.text = food.Name;
        CommentLabel.text = food.Comment;
        CDLabel.text = "";

        if (food.Effect.Target == EffectModel.TargetEnum.All)
        {
            TargetLabel.text = "目標：不分敵我";
        }
        else if (food.Effect.Target == EffectModel.TargetEnum.Them)
        {
            TargetLabel.text = "目標：敵方";
        }
        else if (food.Effect.Target == EffectModel.TargetEnum.Us)
        {
            TargetLabel.text = "目標：我方";
        }

        if (food.Effect.Track == EffectModel.TrackEnum.None)
        {
            TrackLabel.text = "軌跡：無視地形";
        }
        else if (food.Effect.Track == EffectModel.TrackEnum.Parabola)
        {
            TrackLabel.text = "軌跡：拋物線";
        }
        else if (food.Effect.Track == EffectModel.TrackEnum.Straight)
        {
            TrackLabel.text = "軌跡：直線";
        }
        else if (food.Effect.Track == EffectModel.TrackEnum.Through)
        {
            TrackLabel.text = "軌跡：貫穿";
        }

        RangeLabel.text = "射程：" + food.Effect.Range;
    }
}
