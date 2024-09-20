using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public Action CloseHandler;

    public RectTransform RectTransform;
    public ScrollView ScrollView;
    public TipLabel TipLabel;
    public Button CloseButton;
    public Text LvLabel;
    public Text ExpLabel;

    private static TipLabel _tipLabel;

    public static CharacterUI Open()
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/CharacterUI"), Vector3.zero, Quaternion.identity);
        GameObject canvas = GameObject.Find("Canvas");
        obj.transform.SetParent(canvas.transform);
        CharacterUI characterUI = obj.GetComponent<CharacterUI>();
        characterUI.RectTransform.offsetMax = Vector3.zero;
        characterUI.RectTransform.offsetMin = Vector3.zero;
        characterUI.ScrollView.SetData(new List<object>(CharacterManager.Instance.Info.CharacterList));
        characterUI.ScrollView.SetIndex(0);
        characterUI.LvLabel.text = "∂§•Óµ•Ø≈°G" + CharacterManager.Instance.Info.Lv;
        characterUI.ExpLabel.text = "∏g≈Á≠»°G" + CharacterManager.Instance.Info.Exp + "/" + CharacterManager.Instance.NeedExp(CharacterManager.Instance.Info.Lv);
        _tipLabel = characterUI.TipLabel;

        return characterUI;
    }

    public static void SetTip(string str) 
    {
        _tipLabel.SetLabel(str);
    }

    public void Close()
    {
        if (CloseHandler != null)
        {
            CloseHandler();
        }

        Destroy(gameObject);
    }

    private void Awake()
    {
        CloseButton.onClick.AddListener(Close);
    }
}
