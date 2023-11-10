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
        if (status.Icon != "x")
        {
            Icon.sprite = Resources.Load<Sprite>("Image/" + status.Icon);
        }
        Label.text = status.Name + "\n" + status.Comment;
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
