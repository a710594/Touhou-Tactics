using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterUI : MonoBehaviour
{
    public RectTransform RectTransform;
    public ScrollView ScrollView;
    public TipLabel TipLabel;
    public Button CloseButton;

    private static TipLabel _tipLabel;

    public static SelectCharacterUI Open()
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/SelectCharacterUI"), Vector3.zero, Quaternion.identity);
        GameObject canvas = GameObject.Find("Canvas");
        obj.transform.SetParent(canvas.transform);
        SelectCharacterUI selectCharacterUI = obj.GetComponent<SelectCharacterUI>();
        selectCharacterUI.RectTransform.offsetMax = Vector3.zero;
        selectCharacterUI.RectTransform.offsetMin = Vector3.zero;
        selectCharacterUI.ScrollView.SetData(new List<object>(CharacterManager.Instance.Info.CharacterList));
        _tipLabel = selectCharacterUI.TipLabel;

        return selectCharacterUI;
    }

    public static void SetTip(string str) 
    {
        _tipLabel.SetLabel(str);
    }

    private void Close()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        CloseButton.onClick.AddListener(Close);
    }
}
