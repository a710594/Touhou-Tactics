using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemUI : MonoBehaviour
{
    public Button SaveButton;
    public Button ExitButton;

    public static SystemUI Open()
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/SystemUI"), Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localPosition = Vector3.zero;

        return obj.GetComponent<SystemUI>();
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    private void SaveOnClick()
    {
        ConfirmUI.Open("�n�s�ɶܡH", "�T�w", "����", () =>
        {
            FileSystem.Instance.Save();
            InputMamager.Instance.Unlock();
            Destroy(gameObject);
        }, null);
    }

    private void ExitOnClick()
    {
        ConfirmUI.Open("�n���}�C���ܡH", "�T�w", "����", () =>
        {
            Application.Quit();
        }, null);
    }

    private void Awake()
    {
        SaveButton.onClick.AddListener(SaveOnClick);
        ExitButton.onClick.AddListener(ExitOnClick);
    }
}