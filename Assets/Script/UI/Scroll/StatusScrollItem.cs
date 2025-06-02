using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusScrollItem : ScrollItem
{
    public Text NameLabel;
    public Text RemainTimeLabel;
    public Text CommentLabel;

    public override void SetData(object data)
    {
        Status status = (Status)data;
        NameLabel.text = status.Name;
        RemainTimeLabel.text = status.RemainTime.ToString() + "¦^¦X";
        CommentLabel.text = status.Comment;
    }
}
