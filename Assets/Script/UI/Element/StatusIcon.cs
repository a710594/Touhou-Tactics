using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image Icon;
    public Text Label;

    public void SetData(Status status) 
    {
        if (status.Data.Icon != "x")
        {
            Icon.sprite = Resources.Load<Sprite>("Image/" + status.Data.Icon);
        }
        Label.text = status.Data.Name + "\n" + status.Data.Comment;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Label.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Label.gameObject.SetActive(false);
    }

    private void Awake()
    {
        Label.gameObject.SetActive(false);
    }
}
