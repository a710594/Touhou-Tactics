using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public Button CloseButton;

    public static void Open(string commentText, string confirmText, Action confirmCallback)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/TutorialUI"), Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localPosition = Vector3.zero;
        TutorialUI tutorialUI = obj.GetComponent<TutorialUI>();
        //tutorialUI.Init(commentText, confirmText, confirmCallback);
    }

    private void CloseOnClick() 
    {
        Destroy(gameObject);
    }
    private void Awake()
    {
        CloseButton.onClick.AddListener(CloseOnClick);
    }
}
