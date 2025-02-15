using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ActionButton : MonoBehaviour
{
    public Action ClickHandler;

    public Button Button;
    public Image Image;
    public TipLabel TipLabel;

    protected bool _canUse;
    protected string _tipText;
    protected Color _canUseColor = new Color32(255, 236, 191, 255);
    protected Color _notUseColor = new Color32(200, 180, 140, 255);

    public virtual void SetColor(BattleCharacterInfo character) 
    { 
    }

    protected void OnClick() 
    {
        if(_canUse) 
        {
            if (ClickHandler != null) 
            {
                ClickHandler();
            }
        }
        else
        {
            TipLabel.SetLabel(_tipText);
        }
    }

    private void Awake()
    {
        Button.onClick.AddListener(OnClick);
    }
}
