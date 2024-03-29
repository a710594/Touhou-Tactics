using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image Icon;
    public Text Label;
    public GameObject LabelBG;

    public void SetData(Status status) 
    {
        if (status.Icon != "x")
        {
            Icon.sprite = Resources.Load<Sprite>("Image/" + status.Icon);
        }
        Label.text = status.Name + "\n" + status.Comment;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LabelBG.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LabelBG.SetActive(false);
    }

    private void Awake()
    {
        LabelBG.gameObject.SetActive(false);
    }
}
